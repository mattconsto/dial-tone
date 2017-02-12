using UnityEngine;
using System.Collections;
using System.Timers;

public class Call {
	public enum State {
		NULLSTATE,
		UNANSWERED,
		OPERATOR_CONNECTED, 
		DISCONNECTED_AFTER_OPERATOR, 
		TALKING, 
		DISCONNECT_POSITIVE,
		DISCONNECT_NEGATIVE,
		WRONG_PERSON_CONNECTED,

	}

	private State state = State.UNANSWERED;
	private State previousState = State.NULLSTATE;

	// Flags for timing out
	private bool unansweredTimedout = false;
	private bool afterOperatorTimedout = false;
	private bool convoTimedout = false;
	private bool wrongConvoTimedout = false;
	private bool disconnectGraceTimedout = false;
    private bool spokenToOperator = false;

	private bool afterOperatorTimeoutCalled = false;


	// TODO TUNE THE TIMERS
	public double unansweredTimeoutTime = 10000;
	public double afterOperatorTimeoutTime = 10000;
	public double convoTimeoutTime = 10000;
	public double wrongConvoTimeoutTime = 2000;
	public double disconnectGraceTime = 5000; // grace period after call finished to block new calls on this connection.

	private Timer unansweredTimer;
	private Timer afterOperatorTimer;
	private Timer convoTimer;
	private Timer wrongConvoTimer;
	private Timer disconnectGraceTimer;


	public string incomingPort;
	public string targetPort;
	public Conversation operatorConv;
    public Conversation tappedConv;

    public Call() {
		// Init timers
		unansweredTimer = new Timer(unansweredTimeoutTime);
		afterOperatorTimer = new Timer(afterOperatorTimeoutTime);
		convoTimer = new Timer(convoTimeoutTime);
		wrongConvoTimer = new Timer(wrongConvoTimeoutTime);
		disconnectGraceTimer = new Timer(disconnectGraceTime);


		unansweredTimer.Elapsed += (sender, args) => {
			unansweredTimedout = true;
			unansweredTimer.Stop ();
		};
		afterOperatorTimer.Elapsed += (sender, args) => {
			afterOperatorTimedout = true;
			afterOperatorTimer.Stop ();
		};
		convoTimer.Elapsed += (sender, args) => {
			convoTimedout = true;
			convoTimer.Stop ();
		};
		wrongConvoTimer.Elapsed += (sender, args) => {
			wrongConvoTimedout = true;
			wrongConvoTimer.Stop ();
		};
		disconnectGraceTimer.Elapsed += (sender, args) => {
			disconnectGraceTimedout = true;
			disconnectGraceTimer.Stop ();
		};
	}

	public bool handleState(string connectedTo, SocketController controller, string operatorSocket, string tapconnection,ConversationHandler convHandle) {
		if (previousState != state) {
			Debug.Log(incomingPort + " has changed from " + previousState + " to " + state);
		}

		switch (state) {
		case State.UNANSWERED:
			if (previousState != state) {
				controller.setLED (incomingPort, Socket.LEDColor.Red);
				unansweredTimer.Start ();
				previousState = state;
			}

			if (unansweredTimedout) {
				state = State.DISCONNECT_NEGATIVE;
			} else if (connectedTo == operatorSocket) {
				state = State.OPERATOR_CONNECTED;
			} else if (connectedTo != null) {
				state = State.WRONG_PERSON_CONNECTED;
			}
			break;
		case State.OPERATOR_CONNECTED:
			if (previousState != state) {
				Debug.Log ("OH HI THE ANSWER IS " + targetPort);
                    convHandle.setConversation(operatorConv);
				controller.setLED (incomingPort, Socket.LEDColor.Green);
				controller.setLED (operatorSocket, Socket.LEDColor.Green);

				// Only call once, they can jump back to this state, and it shouldn't restart the timer.
				if (!afterOperatorTimeoutCalled) {
					afterOperatorTimeoutCalled = true;
					afterOperatorTimer.Start ();
				}

				previousState = state;
			}
                //EVERY FRAME

            if (afterOperatorTimedout) { // connected too long
				state = State.DISCONNECT_NEGATIVE;
			} else if (connectedTo != operatorSocket) {
				state = State.DISCONNECTED_AFTER_OPERATOR;
			}
			break;
		case State.DISCONNECTED_AFTER_OPERATOR:
			if (previousState != state) {
				controller.setLED (incomingPort, Socket.LEDColor.Red);
				controller.setLED (operatorSocket, Socket.LEDColor.Off);
				previousState = state;
			}
			if (afterOperatorTimedout) { // connected too long
				state = State.DISCONNECT_NEGATIVE;
			} else if (connectedTo == targetPort || (connectedTo == operatorSocket && tapconnection == targetPort)) {
				state = State.TALKING;
			} else if (connectedTo == operatorSocket) {
				state = State.OPERATOR_CONNECTED;
			} else if (connectedTo != null) {
				state = State.WRONG_PERSON_CONNECTED;
			}
			break;
		case State.TALKING:
			if (previousState != state) {
				controller.setLED (incomingPort, Socket.LEDColor.Green);
				controller.setLED (targetPort, Socket.LEDColor.Green);

				// Lock both
				controller.LockSocket (incomingPort);
				controller.LockSocket (targetPort);

				convoTimer.Start ();
				previousState = state;
			}
			if (convoTimedout) { // convo success
				state = State.DISCONNECT_POSITIVE;
			}
			break;
		case State.DISCONNECT_POSITIVE:
			if (previousState != state) {
				controller.happyAt (incomingPort);
				controller.setLED (incomingPort, Socket.LEDColor.Off);
				controller.setLED (targetPort, Socket.LEDColor.Off);

				// Unlock both
				controller.UnlockSocket (incomingPort);
				controller.UnlockSocket (targetPort);

				disconnectGraceTimer.Start ();
				previousState = state;
			}
			// TODO Positive feedback
			if (disconnectGraceTimedout) {
				controller.unreserveForCall (incomingPort);
				controller.unreserveForCall (targetPort);
				// we want to die, return false!
				return false;
			}
			break;
		case State.DISCONNECT_NEGATIVE:
			if (previousState != state) {
				controller.sadAt (incomingPort);
				controller.setLED (incomingPort, Socket.LEDColor.Off);
				controller.setLED (targetPort, Socket.LEDColor.Off);
				if (connectedTo != null) {
					controller.setLED (connectedTo, Socket.LEDColor.Off);
					controller.UnlockSocket (connectedTo);
					controller.unreserveForCall (connectedTo);
				}

				// Unlock both
				controller.UnlockSocket (incomingPort);
				controller.UnlockSocket (targetPort);

				disconnectGraceTimer.Start ();
				previousState = state;
			}
			if (disconnectGraceTimedout) {
				controller.unreserveForCall (incomingPort);
				controller.unreserveForCall (targetPort);
				// we want to die, return false!
				return false;
			}
			break;
		case State.WRONG_PERSON_CONNECTED:
			if (previousState != state) {
				controller.setLED (incomingPort, Socket.LEDColor.Green);
				controller.setLED (connectedTo, Socket.LEDColor.Green);

				// Lock both
				controller.LockSocket (incomingPort);
				controller.LockSocket (connectedTo);
				controller.reserveForCall (connectedTo);

				wrongConvoTimer.Start ();
				previousState = state;
			}
			if (wrongConvoTimedout) {
				state = State.DISCONNECT_NEGATIVE;
			}
			break;
		}
		// We still want to live, return true!
		return true;
	}
}

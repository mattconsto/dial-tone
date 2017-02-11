using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	//public diagloge
	public float betweenCalls_MIN = 2f;
	public float betweenCalls_MAX = 5f;
	ConversationLoader loader = new ConversationLoader ();
	public SocketController sockControl;
	public TextWriter txtwrite;

	int day = 1;
	int callsToday = 0;
	int maxCallsToday = 5;
	int storycall = 2;
	List<PendingConnection> calls = new List<PendingConnection>();
	bool pairReady = false;
	bool toOperator = false;
	bool inconversation = false;
	Conversation curconv;

	enum GAMESTATE {START, DRIVE, INTRO,INTRO_INCALL,DAY,DAY_WAITINGONCONNECT}
	GAMESTATE gamestate = GAMESTATE.START;


	//Day Start & beeping
	//Story call
	//Story call end -> main loop
	//waiting for call
	//connection made

	void Awake()
	{
		loader.init ();
	}
	void Update()
	{
		if (!loader.finishedLoading)
			return;
		//setGameState ();
		manageCalls ();
		manageConnections (Time.deltaTime);
	}
	void manageCalls()
	{
		if (day == 1) {
			//at 3 semi-random times start a new pending (Flashing LED > OP > CONN)
			float t = Random.Range(betweenCalls_MIN,betweenCalls_MAX);
			for(int i=0;i<3;i++)
			{
				Invoke("startNewPair",t);
				t+=Random.Range(betweenCalls_MIN,betweenCalls_MAX);
			}

		}
	}
	void manageConnections(float deltaTime)
	{
		//Ask andy code for connection complete for the pair
		//Once connection made

		//if tooperator
			//display connection pair request text
				//if this is the story, display the story text instead
		//else
			//don't display call unless tapped

		for (int i=0; i<calls.Count; i++) {
			if(!calls[i].spokenToOperator && sockControl.getConnectedTo(calls[i].incomingPort).name=="operator" && !inconversation)
			{
				calls[i].spokenToOperator = true;
				//get operator[story] conversation next & display
				curconv = loader.getNextConversation();
				StartCoroutine(sendConversation());
				inconversation = true;
				sockControl.setLED (calls[i].incomingPort, "GREEN");
				sockControl.setLED ("operator", "GREEN");
			}
			else if(calls[i].spokenToOperator && sockControl.getConnectedTo(calls[i].incomingPort) != null && sockControl.getConnectedTo(calls[i].incomingPort).name==calls[i].targetPort)
			{
				calls[i].connected = true;
				sockControl.setLED (calls[i].incomingPort, "GREEN");
				sockControl.setLED (calls[i].targetPort, "GREEN");
			}
			else if(calls[i].spokenToOperator && sockControl.getConnectedTo(calls[i].incomingPort) != null && sockControl.getConnectedTo(calls[i].incomingPort).name!=calls[i].targetPort)
			{
				//DROP CALL
				sockControl.setLED (calls[i].incomingPort, "BLACK");
				calls.RemoveAt(i);
			}
			else if(calls[i].connected)
			{
				calls[i].timeLeft -= deltaTime;
				if(calls[i].timeLeft < 0)
				{
					sockControl.setLED (calls[i].incomingPort, "BLACK");
					sockControl.setLED (calls[i].targetPort, "BLACK");
					calls.RemoveAt(i);
				}
			}
		}

		//Display the LED for the incoming call

	}
	IEnumerator sendConversation()
	{
		bool hasnext = curconv.hasNextSentance ();
		while(hasnext)
		{
			if(!txtwrite.speaking)
			{
				SentanceObject sent = curconv.getNextSentance();
				hasnext = curconv.hasNextSentance ();
				Debug.Log("[Story]"+sent.content);
				txtwrite.Say(string.Format(sent.content,sent.targetPort),sent.textColor,sent.Alignment);
			}
			yield return new WaitForFixedUpdate();
		}
		inconversation = false;
	}
	void startNewPair()
	{	
		if (callsToday < maxCallsToday) {
			chosePorts();
			pairReady = true;
		}
	}
		
	void chosePorts()
	{
		toOperator = !toOperator;//calls go to operator, then to target ect
		//if toOperator, one port needs to be operator port
		PendingConnection pending = new PendingConnection ();
		Socket socketobj = getAvailablePort ();
		if (socketobj == null)
			return;
		pending.targetPort = socketobj.name;
		socketobj = getAvailablePort ();
		if (socketobj == null)
			return;
		pending.incomingPort = socketobj.name;
		pending.conv = loader.getRandomConversation ();
		
		//Display the input socket as lit up
		sockControl.setLED (pending.incomingPort, "RED");
		//tell andy code to listen for connection
		
	}
	Socket getAvailablePort()
	{
		List<Socket> list = sockControl.getAllSockets ();
		foreach (Socket sock in list) {
			if(!sock.markedForUse)
			{
				sock.markedForUse = true;
				return sock;
			}
		}
		return null;
	}
	void setGameState()
	{
		if (gamestate == GAMESTATE.START)
			gamestate = GAMESTATE.DRIVE;
		if (gamestate == GAMESTATE.DRIVE) {
			//if driving scene complete
			gamestate = GAMESTATE.INTRO;
		}
		if (gamestate == GAMESTATE.INTRO) {
			loader.getStory(day);
			Debug.Log("story conversation displayed here");
			//display the story conversation
			gamestate = GAMESTATE.INTRO_INCALL;
		}
		if (gamestate == GAMESTATE.INTRO_INCALL) {
			//if call ended
			gamestate = GAMESTATE.DAY;
		}
		if (gamestate == GAMESTATE.DAY) {
			//if toOperator & call complete or !toOperator
			if(!pairReady)
			{
				Invoke("startNewPair",Random.Range(betweenCalls_MIN,betweenCalls_MAX));
			}
		}
	}
}

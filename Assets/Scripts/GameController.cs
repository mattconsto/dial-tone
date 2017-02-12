using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour {

	//public diagloge
	public float betweenCalls_MIN = 2.0f;
	public float betweenCalls_MAX = 5.0f;
	ConversationLoader loader = new ConversationLoader ();
	public SocketController sockControl;
	public TextWriter txtwrite;
	List<string> socketList;
    public BookManager bookMngr;
	public static string OPERATOR_NAME = "operator";
    public ConversationHandler conversationHandler;

    public int score = 0;
	int day = 1;
	int callsToday = 0;
	int maxCallsToday = 1;
	int storycall = 2;
	List<Call> calls = new List<Call>();
	bool pairReady = false;
	bool inconversation = false;
	bool hasInited = false;
	Conversation curconv;
	float timeElapsed = 0;
	float callDelay = 0;
    bool opConnected = false;

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

	/// <summary>
	/// Randomly gen names and map them to sockets.
	/// </summary>
	void assignNames(){
		foreach (string socket in socketList) {
			for (int i = 0; i < 3; i++) {
				sockControl.addName (socket, loader.getRandomName ());
			}
		}
	}


	void Update()
	{
		if (!hasInited){
			Debug.Log("INITING SOCKET LIST");
			Debug.Log("Soceket COunt:"+sockControl.getAllSockets().Count);
			hasInited = true;
			// Remove the operator from the socket list.
			socketList = sockControl.getAllSockets().Where(x => x != OPERATOR_NAME).ToList();
            bookMngr.populate(socketList);
			assignNames ();
		}
      //  Debug.Log("LENGHT" + socketList.Count);
        if (!loader.finishedLoading)
			return;
		//setGameState ();
		timeElapsed += Time.deltaTime;
		if(timeElapsed >= callDelay)
		{
			//Debug.Log(timeElapsed);
			manageCalls ();
			callDelay = Random.Range(betweenCalls_MIN,betweenCalls_MAX);
		}
		manageConnections (Time.deltaTime);
	}
	void manageCalls()
	{
		if (day == 1) {
			timeElapsed = 0;
			//at 3 semi-random times start a new pending (Flashing LED > OP > CONN)
			//for(int i=0;i<3;i++)
			//{
				//Invoke("startNewPair",t);

			if (calls.Count < maxCallsToday) {
				startNewPair();
			}
				//t+=Random.Range(betweenCalls_MIN,betweenCalls_MAX);
			//}

		}
	}
	void manageConnections(float deltaTime)
	{
		List<Call> callsToDelete = new List<Call>();
		foreach (Call call in calls) {
			string connected = sockControl.getConnectedTo (call.incomingPort);
            string tapConnection = sockControl.getConnectedTo("tappingSocket");
            bool keepAlive = call.handleState (connected, sockControl, OPERATOR_NAME,tapConnection, conversationHandler);

			if (!keepAlive) {
				callsToDelete.Add (call);
			}

		}

		foreach (Call call in callsToDelete) {
			Debug.Log ("Killing " + call.incomingPort);
			calls.Remove (call);
		}
		// !!!!!!!!!WARNING: HORRIBLE CODE BELOW!!!!!!!!!!!!

		//Ask andy code for connection complete for the pair
		//Once connection made

		//if tooperator
			//display connection pair request text
				//if this is the story, display the story text instead
		//else
//			//don't display call unless tapped
//		//Debug.Log("MANAGING()");
//		for (int i=0; i<calls.Count; i++) {
//			// Polling loop
//
//			//if(sockControl.getConnectedTo(calls[i].incomingPort)!=null)
//				//Debug.Log("ID:"+i+"PORT:"+calls[i].incomingPort+" connected to "+sockControl.getConnectedTo(calls[i].incomingPort).name+" Target:"+calls[i].targetPort);
//
//
//			if(!calls[i].spokenToOperator && sockControl.getConnectedTo(calls[i].incomingPort)!=null && sockControl.getConnectedTo(calls[i].incomingPort).name=="operator" && !inconversation)
//			{
//				//Debug.Log("CONNTECED CORRECTLY");
//				calls[i].spokenToOperator = true;
//				//get operator[story] conversation next & display
//				curconv = loader.getNextConversation();
//				curconv.setFormatter(sockControl.getSocket(calls[i].targetPort).getRandomName());
//				Debug.Log("targetPort::"+calls[i].targetPort);
//				StartCoroutine(sendConversation());
//				inconversation = true;
//				sockControl.setLED (calls[i].incomingPort, "GREEN");
//				sockControl.setLED ("operator", "GREEN");
//                opConnected = true;
//			}
//			else if(calls[i].spokenToOperator && sockControl.getConnectedTo(calls[i].incomingPort) != null && sockControl.getConnectedTo(calls[i].incomingPort).name==calls[i].targetPort)
//			{
//                //Debug.Log("CONNTECED CORRECTLY");
//                score++;
//				calls[i].connected = true;
//				sockControl.setLED (calls[i].incomingPort, "GREEN");
//				sockControl.setLED (calls[i].targetPort, "GREEN");
//			}
//			else if(calls[i].spokenToOperator && sockControl.getConnectedTo(calls[i].incomingPort) != null && (sockControl.getConnectedTo(calls[i].incomingPort).name!=calls[i].targetPort && sockControl.getConnectedTo(calls[i].incomingPort).name != "operator"))
//			{
//				//DROP CALL
//				sockControl.setLED (calls[i].incomingPort, "OFF");
//                sockControl.getSocket(calls[i].incomingPort).markedForUse = false;
//                sockControl.getSocket(calls[i].targetPort).markedForUse = false;
//                calls.RemoveAt(i);
//                Debug.Log("Dropped Call");
//			}
//			if(calls[i].connected)
//			{
//                Debug.Log("ID:" + i + " t:" + calls[i].timeLeft);
//				calls[i].timeLeft -= deltaTime;
//				if(calls[i].timeLeft < 0)
//				{
//					sockControl.setLED (calls[i].incomingPort, "OFF");
//					sockControl.setLED (calls[i].targetPort, "OFF");
//					calls.RemoveAt(i);
//				}
//                else
//                {
//                    if(sockControl.getConnectedTo(calls[i].incomingPort) == null)
//                    {
//                        sockControl.setLED(calls[i].incomingPort, "OFF");
//                        sockControl.setLED(calls[i].targetPort, "OFF");
//                        sockControl.getSocket(calls[i].incomingPort).markedForUse = false;
//                        sockControl.getSocket(calls[i].targetPort).markedForUse = false;
//                        calls.RemoveAt(i);
//                    }
//                }
//			}
//            if(opConnected && sockControl.getConnectedTo("operator") == null)
//            {
//                opConnected = false;
//                txtwrite.Say("[Disconnected]",new Color(),"left",true);
//            }
//		}

		//Display the LED for the incoming call

	}
	
	void startNewPair()
	{	
		choosePorts();
		pairReady = true;
	}
		
	void choosePorts()
	{
		//if toOperator, one port needs to be operator port
		Call call = new Call ();
		string socketA = getAvailablePort ();
		if (socketA == null) { // nothing available
			return;
		}
		sockControl.reserveForCall (socketA);
		string socketB =  getAvailablePort ();
		if (socketA == null) {// nothing available, clear A.
			sockControl.reserveForCall (socketA);
			return;
		}
		sockControl.reserveForCall (socketB);

		call.targetPort = socketA;
		call.incomingPort = socketB;
		call.operatorConv = loader.getRandomOperatorConversation ();
        call.operatorConv.setFormatter(sockControl.getSocket(call.targetPort).getRandomName());
        
        call.tappedConv = loader.getRandomTappedConversation();
        //Display the input socket as lit up
        //tell andy code to listen for connection

        calls.Add(call);
	}
	string getAvailablePort()
	{
		// Shuffle sockets
		var result = socketList.OrderBy(item => Random.Range(-1,1));
		foreach (string sock in result) {
			if(!sockControl.isReservedForCall(sock))
			{
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

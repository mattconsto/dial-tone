using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour {

	//public diagloge
	public float betweenCalls_MIN = 2f;
	public float betweenCalls_MAX = 5f;
	ConversationLoader loader = new ConversationLoader ();
	public SocketController sockControl;
	public TextWriter txtwrite;
	List<Socket> socketList;

    public int score = 0;
	int day = 1;
	int callsToday = 0;
	int maxCallsToday = 5;
	int storycall = 2;
	List<PendingConnection> calls = new List<PendingConnection>();
	bool pairReady = false;
	bool toOperator = false;
	bool inconversation = false;
	bool hasInited = false;
	Conversation curconv;
	float timeElapsed = 0;
	float callDelay = 0;

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
		if (!hasInited){
			Debug.Log("INITING SOCKET LIST");
			Debug.Log("Soceket COunt:"+sockControl.getAllSockets().Count);
			hasInited = true;
			socketList = sockControl.getAllSockets().Where(x => x.name != "operator").ToList();
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
				startNewPair();
				//t+=Random.Range(betweenCalls_MIN,betweenCalls_MAX);
			//}

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
		//Debug.Log("MANAGING()");
		for (int i=0; i<calls.Count; i++) {
			if(sockControl.getConnectedTo(calls[i].incomingPort)!=null)
				Debug.Log("ID:"+i+"PORT:"+calls[i].incomingPort+" connected to "+sockControl.getConnectedTo(calls[i].incomingPort).name+" Target:"+calls[i].targetPort);
			if(!calls[i].spokenToOperator && sockControl.getConnectedTo(calls[i].incomingPort)!=null && sockControl.getConnectedTo(calls[i].incomingPort).name=="operator" && !inconversation)
			{
				//Debug.Log("CONNTECED CORRECTLY");
				calls[i].spokenToOperator = true;
				//get operator[story] conversation next & display
				curconv = loader.getNextConversation();
				curconv.setFormatter(calls[i].targetPort);
				Debug.Log("targetPort::"+calls[i].targetPort);
				StartCoroutine(sendConversation());
				inconversation = true;
				sockControl.setLED (calls[i].incomingPort, "GREEN");
				sockControl.setLED ("operator", "GREEN");
			}
			else if(calls[i].spokenToOperator && sockControl.getConnectedTo(calls[i].incomingPort) != null && sockControl.getConnectedTo(calls[i].incomingPort).name==calls[i].targetPort)
			{
                //Debug.Log("CONNTECED CORRECTLY");
                score++;
				calls[i].connected = true;
				sockControl.setLED (calls[i].incomingPort, "GREEN");
				sockControl.setLED (calls[i].targetPort, "GREEN");
			}
			else if(calls[i].spokenToOperator && sockControl.getConnectedTo(calls[i].incomingPort) != null && (sockControl.getConnectedTo(calls[i].incomingPort).name!=calls[i].targetPort && sockControl.getConnectedTo(calls[i].incomingPort).name != "operator"))
			{
				//DROP CALL
				sockControl.setLED (calls[i].incomingPort, "OFF");
				calls.RemoveAt(i);
                Debug.Log("Dropped Call");
			}
			else if(calls[i].connected)
			{
				calls[i].timeLeft -= deltaTime;
				if(calls[i].timeLeft < 0)
				{
					sockControl.setLED (calls[i].incomingPort, "OFF");
					sockControl.setLED (calls[i].targetPort, "OFF");
					calls.RemoveAt(i);
				}
			}
		}

		//Display the LED for the incoming call

	}
	IEnumerator sendConversation()
	{
		bool hasnext = curconv.hasNextSentance ();
		Debug.Log("HasNext: "+hasnext);
		while(hasnext)
		{
			if(!txtwrite.speaking)
			{
				SentanceObject sent = curconv.getNextSentance();
				hasnext = curconv.hasNextSentance ();
				Debug.Log("[Story]"+sent.content);
				//curconv.getNextSentance().content = string.Format(curconv.getNextSentance().content,curconv.t);
				//curconv.reset();
				txtwrite.Say(sent.content,sent.textColor,sent.Alignment);
			}
			yield return new WaitForFixedUpdate();
		}
		inconversation = false;
		Debug.Log("NO LONGER IN CONVERSATION");
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
		
		calls.Add(pending);
	}
	Socket getAvailablePort()
	{
		var result = socketList.OrderBy(item => Random.Range(-1,1));
		foreach (Socket sock in result) {
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

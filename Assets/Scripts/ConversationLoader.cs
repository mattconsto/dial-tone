using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class ConversationLoader {

	ArrayList conversations = new ArrayList ();
	ArrayList drives = new ArrayList ();
	ArrayList requests = new ArrayList ();
	ArrayList operatorConversations = new ArrayList ();
    ArrayList story = new ArrayList();


    ArrayList forenames = new ArrayList ();
	ArrayList surnames = new ArrayList ();
	public bool finishedLoading = false;

	int storyProgression = 0;

	public void init()
	{
		loadFromFile(conversations,"convo.txt");
		//loadFromFile(drives,"drive.txt");
		//loadFromFile(requests,"request.txt");
		loadFromFile(operatorConversations, "operator.txt");
		loadNames ("names.csv");
		finishedLoading = true;
		Debug.Log ("Finished loading");
	}

	 void loadNames(string filename){
		StreamReader sr = new StreamReader(Application.dataPath + "/ConversationFiles/" + filename);
		string line;
		string[] row;
		while ((line = sr.ReadLine()) != null)
		{
			row = line.Split(',');
			forenames.Add (row [0]);
			surnames.Add (row [1]);
		}

	}

	public string getRandomName(){
		int f = (int)(Random.value * 100) % forenames.Count;
		string forename = (string) forenames [f];
		forenames.RemoveAt (f);
		int s = (int)(Random.value * 100) % surnames.Count;
		string surname = (string) surnames [s];
		surnames.RemoveAt (s);
		return forename + " " + surname;
	}


	void loadFromFile(ArrayList list, string filename)
	{
		//Debug.Log ("Started loading "+filename);
		StringBuilder convstr;
		StreamReader rdr = new StreamReader (Application.dataPath + "/ConversationFiles/" + filename, Encoding.Default);
		using(rdr)
		{
			string line;
			do
			{
				line = rdr.ReadLine();
				if(line!=null)
				{
					if(line.Equals("***"))
						list.Add (new Conversation());
					else
					{
						((Conversation)list[list.Count-1]).addSentance(line);
						//Debug.Log("NEWSENTANCE: "+line);
						//conversations.Add (new Conversation());
					}

				}
			}
			while(line != null);
			rdr.Close();
		}
	}
	public Conversation getNextStoryConversation()
	{
		Conversation toreturn = (Conversation)story[storyProgression];
		toreturn.reset();
		storyProgression++;
		return toreturn;
	}
	public Conversation getRandomOperatorConversation()
	{
		int convo = Random.Range (0, (operatorConversations.Count - 1));
		((Conversation)operatorConversations[convo]).reset ();
		return (Conversation)operatorConversations[convo];
	}
    public Conversation getRandomTappedConversation()
    {
        int convo = Random.Range(0, (conversations.Count - 1));
        ((Conversation)conversations[convo]).reset();
        return (Conversation)conversations[convo];
    }
    public Conversation getDriveConversation(int day)
	{
		return (Conversation)drives[day];
	}
	public Conversation getRequest()
	{
		return (Conversation)requests [0];
	}
	public Conversation getStory(int day)
	{
		return (Conversation)story [day-1];
	}
	/*void accessData(JSONObject obj){
		switch(obj.type){
		case JSONObject.Type.OBJECT:
			for(int i = 0; i < obj.list.Count; i++){
				string key = (string)obj.keys[i];
				JSONObject j = (JSONObject)obj.list[i];
				Debug.Log(key);
				accessData(j);
			}
			break;
		case JSONObject.Type.ARRAY:
			foreach(JSONObject j in obj.list){
				accessData(j);
			}
			break;
		case JSONObject.Type.STRING:
			Debug.Log(obj.str);
			break;
		case JSONObject.Type.NUMBER:
			Debug.Log(obj.n);
			break;
		case JSONObject.Type.BOOL:
			//Debug.Log(obj.b);
			break;
		case JSONObject.Type.NULL:
		//	Debug.Log("NULL");
			break;
			
		}
	}*/
}

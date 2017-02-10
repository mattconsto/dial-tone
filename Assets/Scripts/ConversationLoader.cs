using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class ConversationLoader : MonoBehaviour {

	ArrayList conversations = new ArrayList ();
	void Awake()
	{
		loadFromFile();
	}
	
	void loadFromFile()
	{
		StringBuilder convstr;
		StreamReader rdr = new StreamReader(Application.dataPath+"/ConversationFiles/convo.txt",Encoding.Default);
		using(rdr)
		{
			string line;
			do
			{
				line = rdr.ReadLine();
				if(line!=null)
				{
					if(line.Equals("***"))
						conversations.Add (new Conversation());
					else
					{
						Debug.Log(conversations.Count);
						((Conversation)conversations[conversations.Count-1]).addSentance(line);
					}

				}
			}
			while(line != null);
			rdr.Close();
		}
		/*
		if (jsonfile.Length != 0) {
			JSONObject jsonobj = new JSONObject(jsonfile.ToString());
			//jsonobj.GetField("conversations");
			accessData(jsonobj);
		}*/
		
	}
	public Conversation choseRandomConversation()
	{
		int convo = Random.Range (0, (conversations.Count - 1));
		((Conversation)conversations [convo]).reset ();
		return (Conversation)conversations [convo];
	}

	void accessData(JSONObject obj){
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
			Debug.Log(obj.b);
			break;
		case JSONObject.Type.NULL:
			Debug.Log("NULL");
			break;
			
		}
	}
}

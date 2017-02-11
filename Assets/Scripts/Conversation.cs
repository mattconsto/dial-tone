using UnityEngine;
using System.Collections;

public class Conversation {

	ArrayList sentances = new ArrayList();
	int sentance = 0;
	public Color person1;
	public Color person2;
	bool isperson1 = true;

	public void addSentance(string str)
	{
		Debug.Log ("ConvCount:" + sentances.Count);
		sentances.Add (str);
		//sentances[sentances.Count] = str;
	}

	public bool hasNextSentance()
	{
		return sentance < sentances.Count;
	}
	public SentanceObject getNextSentance()
	{
		string str = (string)sentances [sentance];
		sentance++;
		SentanceObject obj = new SentanceObject ();
		obj.content = str;
		if (isperson1) {
			obj.textColor = person1;
			obj.Alignment = "left";
		} else {
			obj.textColor = person2;
			obj.Alignment = "right";
		}
		return obj;
	}
	public void reset()
	{
		sentance = 0;
	}

}

using UnityEngine;
using System.Collections;

public class Conversation {

	ArrayList sentances = new ArrayList();
	int sentance = 0;
	public Color person1;
	public Color person2;
	bool isperson1 = true;
	string formatter;
    public string toReplace = "TONAME";
    public string fromReplace = "FROMNAME";

    public void addSentance(string str)
	{
		sentances.Add (str);
		//Debug.Log ("ConvCount:" + sentances.Count);
		//Debug.Log("add()"+str);
		//sentances[sentances.Count] = str;
	}

	public void setFormatter(string formatter){
		this.formatter = formatter;
	}
	
	public bool hasNextSentance()
	{
		return sentance < sentances.Count;
	}
	public SentanceObject getNextSentance()
	{
        string str;
        if (!hasNextSentance())
            str = "...";
        else
		    str = (string)sentances [sentance];
		sentance++;
		SentanceObject obj = new SentanceObject ();
		obj.content = string.Format(str, formatter);
        obj.content = str.Replace("<TO>", toReplace).Replace("<FROM>", fromReplace);
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

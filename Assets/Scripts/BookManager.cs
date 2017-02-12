using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour {

    public Text txt1;
    public Text txt2;
    bool bookOpen = false;

    private void Awake()
    {
        Debug.Log("BookManager on " + gameObject.name);
    }
    public void populate(List<string> socketList, SocketController sockControl)
    {
        Debug.Log("POPULATING BOOK");
        int i = 0;
        txt1.text = "____________________________\n\n";
		txt2.text = "____________________________\n\n";
        Text outputTxt;
        foreach (string sckt in socketList)
        {
			if (i < (int)(socketList.Count / 2)) {
				outputTxt = txt1;
			}
			else {
				outputTxt = txt2;
			}
            outputTxt.text +=  sckt + ":  \n";
			foreach (string name in sockControl.getNames(sckt)) {
				outputTxt.text += "\t" + name + "\n";
			}
			outputTxt.text += "____________________________\n\n";
            i++;
        }
    }
}

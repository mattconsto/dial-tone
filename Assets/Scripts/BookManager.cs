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
    public void populate(List<Socket> socketList)
    {
        Debug.Log("POPULATING BOOK");
        int i = 0;
        txt1.text = "\n\n";
        txt2.text = "\n\n";
        Text outputTxt;
        foreach (Socket sckt in socketList)
        {
            if (i < (int) (socketList.Count / 2))
                outputTxt = txt1;
            else
                outputTxt = txt2;
            outputTxt.text +=  sckt.name + ":  \n";
            foreach (string name in sckt.getNames())
                outputTxt.text +="\t" + name + "\n";
            outputTxt.text += "\n\n";
            i++;
        }
    }
}

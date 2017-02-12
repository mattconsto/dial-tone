using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour {

    public Text outputTxt;
    public Animation anim;
    bool bookOpen = false;

    private void Awake()
    {
        Debug.Log("BookManager on " + gameObject.name);
    }
    public void populate(List<string> socketList)
    {
        Debug.Log("POPULATING BOOK");
        StringBuilder str = new StringBuilder();
        foreach (string sckt in socketList)
        {
            //foreach(string name in sckt.)
            //str.Append(sckt.name + " - " + "HUMANNAME" + "\n");
        }
        outputTxt.text = str.ToString();
        gameObject.SetActive(false);
    }
    public void openBook()
    {
        if (bookOpen)
            gameObject.SetActive(false);
        else
        {
            anim.Stop();
            anim.Play();
            gameObject.SetActive(true);
        }
        bookOpen = !bookOpen;
        Debug.Log("OPENING BOOK");
        Debug.Log("OPENED BOOK");
    }
}

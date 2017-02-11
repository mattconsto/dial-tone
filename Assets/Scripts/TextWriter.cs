using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour {

	private string toWrite;
	private int i;
	public Text text;
	public AudioSource source;

	// Use this for initialization
	void Start () {
		i = 0;
		Say ("testing, testing, testing", Color.red, "left");
	}

	public void Say (string toSay, Color colour, string allignment) {
		toWrite = toSay;
		i = 0;
		text.text = "";
		text.color = colour;
	}

	// Update is called once per frame
	void Update () {
		if (i < toWrite.Length) {
			text.text += toWrite[i].ToString();
			i++;
		}

	}
}

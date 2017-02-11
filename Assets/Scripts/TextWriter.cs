using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour {

	private string toWrite;
	public int cps;
	private float timeElapsed = 0;
	public bool speaking = false;
	public Text text;

	// Use this for initialization
	void Start () {
		Say ("testing, testing, testing", Color.red, "right");
	}

	public bool Say (string toSay, Color colour, string alignment) {
		if (!speaking) {
			speaking = true;
			toWrite = toSay;
			timeElapsed = 0f;
			text.text = "";
			text.color = colour;
			if (alignment == "left") {
				text.alignment = TextAnchor.UpperLeft;
			}
			else if (alignment == "right") {
				text.alignment = TextAnchor.UpperRight;
			}
			return true;
		}
		else {
			return false;
		}
	}

	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;
		text.text = toWrite.Substring (0,Mathf.Min(toWrite.Length, (int)(timeElapsed * cps)));
		if (text.text == toWrite) {
			speaking = false;
		}
	}
		
}

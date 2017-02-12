using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsScript : MonoBehaviour {

    public GameObject note;
    public Text instr;
    private void Awake()
    {
        note.gameObject.SetActive(false);
    }
	public void toggleDisplayed()
    {
        note.gameObject.SetActive(!note.gameObject.activeSelf);
    }
    public void display()
    {
        note.gameObject.SetActive(true);
    }
    public void setInstruction(string instruct)
    {
        instr.text = instruct;
    }
}

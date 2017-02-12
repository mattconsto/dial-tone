using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsScript : MonoBehaviour {

    public GameObject note;
    private void Awake()
    {
        note.gameObject.SetActive(false);
    }
	public void toggleDisplayed()
    {
        note.gameObject.SetActive(!note.gameObject.activeSelf);
    }
}

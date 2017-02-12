using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {

    public Button book;
    public Button sheet;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleBook()
    {
        book.interactable = !book.interactable;
    }

    public void ToggleSheet()
    {
        sheet.interactable = !sheet.interactable;
    }

}

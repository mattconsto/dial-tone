using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour {
	public Vector3 offset;
    // Use this for initialization
    void Start () {
        Cursor.visible = false;
		Update ();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 mousePos = Input.mousePosition;
		Vector3 worldOffset = transform.TransformPoint (offset);
		Vector3 posvec = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 0));
		transform.position = new Vector3 (posvec.x, posvec.y, -50) + worldOffset-transform.position;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plug : MonoBehaviour {
	public Vector3 offset;
	public Vector3 wireOffset;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
		Update ();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 mousePos = Input.mousePosition;
		Vector3 worldOffset = transform.TransformPoint (offset);
		Vector3 posvec = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 20));
		transform.position = new Vector3 (posvec.x, posvec.y, 20) + worldOffset-transform.position;
    }

	public Vector3 getWirePos () {
		Vector3 worldOffset = transform.TransformPoint (wireOffset);
		return worldOffset;
	}
 
}

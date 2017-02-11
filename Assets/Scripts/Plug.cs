using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour {
    public bool followMouse = true;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (followMouse) {
            Vector3 mousePos = Input.mousePosition;
			Vector3 posvec = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 14));
			transform.position = new Vector3 (posvec.x, posvec.y, -14);
        }
    }
    
    void OnDestroy () {
        Cursor.visible = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouse : MonoBehaviour {
	public GameObject toSpawn;
	bool mdown;
	// Use this for initialization
	void Start () {
		mdown = Input.GetMouseButtonDown (0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) != mdown) {
			mdown = !mdown;
			if (mdown == true) {
				Debug.Log ("YAY");
				GameObject.Instantiate (toSpawn, Input.mousePosition, new Quaternion (0, 0, 0, 0),GetComponentInParent<Transform>());
			}
		}
	}
}

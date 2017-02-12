using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouse : MonoBehaviour {
	public GameObject toSpawn;
	public GameObject toSpawn2;
	bool mdown;
	bool mdown2;
	// Use this for initialization
	void Start () {
		mdown = Input.GetMouseButtonDown (0);
		mdown2 = Input.GetMouseButtonDown (1);
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
		if (Input.GetMouseButtonDown(1) != mdown2) {
			mdown2 = !mdown2;
			if (mdown2 == true) {
				Debug.Log ("YAY");
				GameObject.Instantiate (toSpawn2, Input.mousePosition, new Quaternion (0, 0, 0, 0),GetComponentInParent<Transform>());
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bubble : MonoBehaviour {
	// Use this for initialization
	void Start () {
		GetComponent<Image> ().CrossFadeAlpha (0, 1, false);
		Invoke ("KillMe", 1);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mypos = transform.position;
		transform.position = new Vector3 (mypos.x, mypos.y + 1, mypos.z);
	}

	void KillMe()
	{
		GameObject.Destroy (this.gameObject);
	}

}

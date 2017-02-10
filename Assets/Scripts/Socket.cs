using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Socket : MonoBehaviour {
	public Button yourButton;
    public SocketController controller;
 

	// Use this for initialization
	void Start () {
		yourButton.onClick.AddListener(TaskOnClick);
	}

    void TaskOnClick() {
        if (enabled) {
            controller.SocketClick(this);
        }
    }

}

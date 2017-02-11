using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Socket : MonoBehaviour {
	public Button yourButton;
    public SocketController controller;
    public GameObject plug = null;
    private GameObject plugInstance = null;



    public bool IsPlugged()
    {
        return plug == null;
    }

    public void AddPlug()
    {
        // TODO: ID the plugs
        Debug.Log("Plug added");
        plugInstance = Instantiate(plug, transform.position, Quaternion.identity);
        plugInstance.transform.parent = transform;
        Plug plugScript = plugInstance.GetComponent<Plug>();
        plugScript.enabled = false;
    }

    public void RemovePlug()
    {
        GameObject.DestroyImmediate(plugInstance);
    }

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

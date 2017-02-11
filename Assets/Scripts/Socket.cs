using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Socket : MonoBehaviour {
	public Button yourButton;
    public SocketController controller;
    public GameObject plug = null;
    private GameObject plugInstance = null;
	public bool markedForUse = false;


    public bool IsPlugged()
    {
        return plugInstance != null;
    }

    public void AddPlug()
    {
        // TODO: ID the plugs
        Debug.Log("Plug added");
        plugInstance = (GameObject)Instantiate(plug, transform.position, Quaternion.identity);
        plugInstance.transform.SetParent(transform);
        plugInstance = (GameObject)Instantiate(plug, transform.position, Quaternion.identity);
        plugInstance.transform.parent = transform;
        Plug plugScript = plugInstance.GetComponent<Plug>();
        plugScript.enabled = false;
    }

    public void RemovePlug()
    {
        Debug.Log("Removeplug");
        GameObject.DestroyImmediate(plugInstance);
		markedForUse = false;
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

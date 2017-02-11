using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Socket : MonoBehaviour {
	public enum LEDColor {Red, Green, Off};
	public Button yourButton;
    public SocketController controller;
    public GameObject plug = null;
    private GameObject plugInstance = null;
	public bool markedForUse = false;
	public Sprite LED_OFF;
	public Sprite LED_GREEN;
	public Sprite LED_RED;


    public bool IsPlugged()
    {
        return plugInstance != null;
    }

    public void AddPlug()
    {
        // TODO: ID the plugs
      //  Debug.Log("Plug added to socket");
        plugInstance = (GameObject)Instantiate(plug, transform.position, Quaternion.identity);
        plugInstance.transform.SetParent(transform);
        Plug plugScript = plugInstance.GetComponent<Plug>();
        plugScript.enabled = false;
		setLED (LEDColor.Green);
    }

    public void RemovePlug()
    {
    //    Debug.Log("Removeplug");
        GameObject.DestroyImmediate(plugInstance);
		//markedForUse = false;
		setLED (LEDColor.Off);
    }

    // Use this for initialization
    void Start () {
		yourButton.onClick.AddListener(TaskOnClick);
        GetComponentInChildren<Text>().text = gameObject.name;
	}

    void TaskOnClick() {
        if (enabled) {
			
            controller.SocketClick(this);
        }
    }

	public void setLED(LEDColor color) {
		var LED = transform.Find("Indicator");
		Image image = LED.GetComponent<Image> ();
		if (color == LEDColor.Green) {
			image.sprite = LED_GREEN;
		} else if (color == LEDColor.Red) {
			image.sprite = LED_RED;
		} else if (color == LEDColor.Off) {
			image.sprite = LED_OFF;
		}
	}
		

	void Update() {
		
	}
}

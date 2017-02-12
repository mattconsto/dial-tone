using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;


public class Socket : MonoBehaviour {
	public enum LEDColor {Red, Green, Red_Flash, Green_Flash, Off};
	public Button yourButton;
    public SocketController controller;
	public GameObject plug = null;
	public GameObject insertedPlug = null;
    private GameObject plugInstance = null;
	public bool locked = false;
	private List<string> names = new List<string>();
	public Sprite LED_OFF;
	public Sprite LED_GREEN;
	public Sprite LED_RED;

	private Timer flashTimer;
	private double FLASH_DURATION = 500.0;
	private bool flashOn = false;
	private LEDColor currentColor = LEDColor.Off;


	public string getRandomName(){
		int i =  (int) (Random.value * 100) % names.Count;
		return (string) names [i];
	}

	public void addName(string name){
		names.Add (name);
	}

	public ArrayList getNames(){
		return  names;
	}

    public bool IsPlugged()
    {
        return plugInstance != null;
    }

    public void AddPlug()
    {
        // TODO: ID the plugs
        Debug.Log("Plug added to socket");
        plugInstance = (GameObject)Instantiate(insertedPlug, transform.position, Quaternion.identity);
        plugInstance.transform.SetParent(transform);
    }

    public void RemovePlug()
    {
        Debug.Log("Removeplug");
        GameObject.DestroyImmediate(plugInstance);
    }

    // Use this for initialization
    void Start () {
		flashTimer = new Timer(FLASH_DURATION);
		flashTimer.Elapsed += (sender, args) => {
			flashFunc();
		};
		yourButton.onClick.AddListener ( () => {if (enabled) controller.SocketClick(this);});
		GetComponentInChildren<Text> ().text = gameObject.name;
	}

	public void flashFunc(){
		flashOn = !flashOn;
	}

	public void setLED(LEDColor color) {
		var LED = transform.Find("Indicator");
		Image image = LED.GetComponent<Image> ();
		flashTimer.Stop ();
		if (color == LEDColor.Green_Flash || color == LEDColor.Red_Flash) {
			flashTimer.Start ();
			flashOn = true; // Start the flash on.
		}
		currentColor = color;
	}
		

	void Update() {
		var LED = transform.Find("Indicator");
		Image image = LED.GetComponent<Image> ();
		// Flashing
		if (currentColor == LEDColor.Red_Flash || currentColor == LEDColor.Green_Flash) {
			if (flashOn) {
				switch (currentColor) {
				case LEDColor.Green_Flash:
					image.sprite = LED_GREEN;
					break;
				case LEDColor.Red_Flash:
					image.sprite = LED_RED;
					break;
				}
			} else {
				image.sprite = LED_OFF;
			}
		// Static
		} else {
			switch (currentColor) {
			case LEDColor.Green:
				image.sprite = LED_GREEN;
				break;
			case LEDColor.Red:
				image.sprite = LED_RED;
				break;
			case LEDColor.Off:
				image.sprite = LED_OFF;
				break;
			}
		}
	}
}

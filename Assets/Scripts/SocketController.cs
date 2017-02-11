using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tuple<T1, T2> {
    public T1 First { get; private set; }
    public T2 Second { get; private set; }
    internal Tuple(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }
}


public class SocketController : MonoBehaviour {
    private Socket from = null;

	public GameObject mousePlug;
	public GameObject cursor;

	private GameObject mousePlugInstance;

	private List<Tuple<Socket, Socket>> connections = new List<Tuple<Socket, Socket>>();
	private List<Socket> sockets = new List<Socket>();


	//Code to be place in a MonoBehaviour with a GraphicRaycaster component
	private GraphicRaycaster gr;


	// Use this for initialization
	void Start () {
		sockets = GetComponentsInChildren<Socket> ().OfType<Socket> ().ToList ();
		gr = this.GetComponent<GraphicRaycaster> ();	
	}

    // -- Interface --

	public bool areSocketsConnected(string a, string b) {
		return getConnectedTo (getSocket(a)) == getSocket(b);
    }

    public Socket getConnectedTo(Socket socket)
    {
        foreach (Tuple<Socket,Socket> connection in connections)
        {
            if (connection.First == socket)
            {
                return connection.Second;
            } else if (connection.Second == socket)
            {
                return connection.First;
            }
        }
        return null;
    }

	public Socket getConnectedTo(string socketName) {
		return getConnectedTo (getSocket (socketName));
	}


	public Socket getSocket(string socketName) {
		foreach (Socket socket in sockets) {
			if (socket.name == socketName) {
				return socket;
			}
		}
		return null;
	}

    public List<Socket> getAllSockets() {
		return sockets;
    }

	public List<Tuple<string,string>> GetConnectedSockets() {
		return (from c in connections select new Tuple<string,string>(c.First.name,c.Second.name)).ToList();
	}

	public void UnlockSocket(string socket) {
		getSocket(socket).locked = false;
	}

	public void LockSocket(string socket) {
		getSocket(socket).locked = true;
	}

	public void RemoveConnection(Socket s1, Socket s2) {
		List<Tuple<Socket,Socket>> toRemove = new List<Tuple<Socket, Socket>> ();
		foreach (Tuple<Socket,Socket> connection in connections) {
			if ((s1 == connection.First && s2 == connection.Second) || (s1 == connection.Second && s2 == connection.First)) {
				toRemove.Add (connection);
			}
		}
		foreach (var connection in toRemove) {
			connections.Remove(connection);
		}
	}

    // -- Private methods --


    private void PickUp(Socket socket) {
		cursor.GetComponent<Image> ().enabled = false;
		mousePlugInstance = (GameObject)Instantiate(mousePlug, transform.position, Quaternion.identity);
		mousePlugInstance.transform.SetParent(transform);
        from = socket;
        from.AddPlug();
        Debug.Log("Picked Up");
    }

    private void InsertInto(Socket socket) {
		GameObject.DestroyImmediate (mousePlugInstance);
		cursor.GetComponent<Image> ().enabled = true;
        connections.Add(new Tuple<Socket,Socket>(from, socket));
        socket.AddPlug();
		from = null;
		Debug.Log("Inserted");
    }

	public void setLED(string socket, string color) {
		Socket s = getSocket (socket);
		if (color == "RED") { 
			s.setLED (Socket.LEDColor.Red);
		} else if (color == "GREEN") { 
			s.setLED(Socket.LEDColor.Green);
		} else if (color == "OFF") { 
			s.setLED(Socket.LEDColor.Off);
		}
	}

    public void SocketClick(Socket socket) {
		if (socket.locked) {
			return;
		}
		if (isHoldingAPlug()) {
            if (!socket.IsPlugged())
            {
                InsertInto(socket);
            }
        } else {
            if (socket.IsPlugged())
            {
				Debug.Log ("Disconnected");
				var connectedTo = getConnectedTo(socket);
                socket.RemovePlug();
				connectedTo.RemovePlug();
				RemoveConnection (socket, connectedTo);
				PickUp(connectedTo);
            } else {
				PickUp(socket);
            }
        }
    }

	void LineTo(Vector3 p1, Vector3 p2, LineRenderer r) {
		r.enabled = true;
		p1.z = -15;
		p2.z = -15;
		Vector3[] positions = { p1, p2 };
		r.SetPositions(positions);
	}

	void ClearLine(GameObject o) {
		var r = o.GetComponent<LineRenderer> ();
		r.enabled = false;
	}

	public bool isHoldingAPlug() {
		return from != null;
	}

    // Update is called once per frame
    void Update () {
		foreach (Socket socket in sockets) {
			ClearLine (socket.transform.gameObject);
		}

		if (isHoldingAPlug()) {
			// Draw line between mouse and plugged socket
			Vector3 from_o = from.transform.position;
			Vector3 to_o = mousePlugInstance.GetComponent<Plug> ().getWirePos ();
			LineTo (from_o, to_o, from.GetComponent<LineRenderer> ());

			if (Input.GetMouseButtonDown (0)) {
				var mousePos = Input.mousePosition;
				//Create the PointerEventData with null for the EventSystem
				PointerEventData ped = new PointerEventData (null);
				//Set required parameters, in this case, mouse position
				ped.position = Input.mousePosition;
				//Create list to receive all results
				List<RaycastResult> results = new List<RaycastResult> ();
				//Raycast it
				gr.Raycast (ped, results);
				if (results.Count == 0) {
					Debug.Log ("YOU MISSED");
					from.RemovePlug ();
					ClearLine (from.transform.gameObject);
					from = null;
					GameObject.DestroyImmediate (mousePlugInstance);
					cursor.GetComponent<Image> ().enabled = true;
				} else {
					Debug.Log ("You didn't miss");
				}
			}
		}
		foreach (Tuple<Socket,Socket> connection in connections) {
			var first = connection.First.transform.position;
			var second = connection.Second.transform.position;
			LineTo (first,second,connection.First.GetComponent<LineRenderer> ());
		}
	}
}


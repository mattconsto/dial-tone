using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public GameObject plug;
    private GameObject mousePlugInstance = null;

	private List<Tuple<Socket, Socket>> connections = new List<Tuple<Socket, Socket>>();
	private List<Socket> sockets = new List<Socket>();

    // -- Interface --

    public bool addListenerForSocketsConnected(string a, string b) {
		// ROWPOW USE THIS
		return false;
    }

	public bool addListenerForSocketsDisconnected(string a, string b) {
        // ROWPOW USE THIS
		return false;
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
        return new List<Socket>();
        // TODO
    }

	public List<Tuple<string,string>> GetConnectedSockets() {
		return (from c in connections select new Tuple<string,string>(c.First.name,c.Second.name)).ToList();
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


	// Use this for initialization
	void Start () {
		sockets = GetComponentsInChildren<Socket> ().OfType<Socket> ().ToList ();
	}

    private void StartWiring(Socket socket) {
        mousePlugInstance = Instantiate(plug, new Vector3(), Quaternion.identity);
        mousePlugInstance.transform.SetParent(transform);
        from = socket;
        from.AddPlug();
        Debug.Log("Picked Up");
    }

    private void InsertInto(Socket socket) {
        GameObject.DestroyImmediate(mousePlugInstance);
        connections.Add(new Tuple<Socket,Socket>(from, socket));
        socket.AddPlug();
        from = null;
        Debug.Log("Inserted");
    }

    public void SocketClick(Socket socket) {
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
                StartWiring(connectedTo);
            } else {
                StartWiring(socket);
            }
        }
    }


	void LineTo(GameObject o1, GameObject o2) {
		var r = o1.GetComponent<LineRenderer> ();
		r.enabled = true;
		Vector3[] positions = { o1.transform.position, o2.transform.position };
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
			GameObject from_o = from.transform.gameObject;
			GameObject to_o = mousePlugInstance.transform.gameObject;
			LineTo (from_o, to_o);
		}
		foreach (Tuple<Socket,Socket> connection in connections) {
			LineTo (connection.First.transform.gameObject,connection.Second.transform.gameObject);
		}
	}
}

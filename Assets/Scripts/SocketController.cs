using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tuple<T1, T2>
{
    public T1 First { get; private set; }
    public T2 Second { get; private set; }
    internal Tuple(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }
}

public class TupleList<T1, T2> : List<Tuple<T1, T2>>
{
    public void Add(T1 item, T2 item2)
    {
        Add(new Tuple<T1, T2>(item, item2));
    }
}


public class SocketController : MonoBehaviour {
    private Socket from = null;

    public GameObject plug;
    private GameObject fromPlugInstance = null;
    private GameObject mousePlugInstance = null;


    // -- Interface --

    public void addListenerForSocketsConnected(string a, string b) {
        // ROWPOW USE THIS
    }

    public void addListenerForSocketsDisconnected(string a, string b) {
        // ROWPOW USE THIS
    }

    public List<Socket> getAllSockets() {
        return new List<Socket>();
        // TODO
    }

    public TupleList<string,string> GetConnectedSockets() {
        return new TupleList<string,string>(); //TODO
        // init code
        var groceryList = new TupleList<int, string>
        {
            { 1, "kiwi" },
            { 5, "apples" },
            { 3, "potatoes" },
            { 1, "tomato" }
        };
    }

    // -- Private methods --

    private void PickUp(Socket socket) {
        mousePlugInstance = Instantiate(plug, new Vector3(), Quaternion.identity);
        mousePlugInstance.transform.parent = transform;
        from = socket;
        Debug.Log("Picked Up");
    }

    private void Drop(Socket socket) {
        GameObject.DestroyImmediate(mousePlugInstance);
        from = null;
        Debug.Log("Dropped");
    }

    public void SocketClick(Socket socket) {
        if (from) {
            Drop(socket);
        } else {
            PickUp(socket);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}

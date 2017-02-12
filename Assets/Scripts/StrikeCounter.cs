using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrikeCounter : MonoBehaviour {

    public int strikeCount;
    public int maxStrikeCount;

    private Image[] citations;

	// Use this for initialization
	void Start () {
        citations = gameObject.GetComponentsInChildren<Image>();
        newDay(4);
	}
	
    void newDay(int maxStrikeCount)
    {
        this.maxStrikeCount = maxStrikeCount;
        for (int i=0; i < citations.Length; i++)
        {
            if (i< maxStrikeCount)
            {
                citations[i].enabled = true;
                setAlpha(citations[i], 0.2f);
            }
            else
            {
                citations[i].enabled = false;
                setAlpha(citations[i], 0f);
            }
        }

        
    }

    void setAlpha(Image i, float alpha)
    {
        Color c = i.color;
        c.a = alpha;
        i.color = c;
    }

    public bool addCitation()
    {
        strikeCount++;
        setAlpha(citations[strikeCount - 1], 1.0f);
        if (strikeCount >= maxStrikeCount)
        {
            Debug.Log("MAX CITE COUNT REACHED.");
            return false;
        }
        else
        {
            return true;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}

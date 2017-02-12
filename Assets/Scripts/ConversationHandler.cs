using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationHandler : MonoBehaviour
{

    public TextWriter txtwrite;
    Conversation curconv = null;
    bool hasnext = false;


    public void setConversation(Conversation cov)
    {
        Debug.Log("Trying to set cnv");
        if(curconv!=cov)
        {
            Debug.Log("Set cnv");
            curconv = cov;
            StopCoroutine(sendConversation());
            StartCoroutine(sendConversation());
        }
    }
    IEnumerator sendConversation()
    {
        Debug.Log("Started new sendConversation()");
        bool hasnext = curconv.hasNextSentance();
        //Debug.Log("HasNext: " + hasnext);
        while (hasnext)
        {
            if (!txtwrite.speaking && hasnext)
            {
                //Debug.Log("sendConversation():"+curconv);
                SentanceObject sent = curconv.getNextSentance();
                hasnext = curconv.hasNextSentance();
                Debug.Log("[conversation]" + sent.content);
                //curconv.getNextSentance().content = string.Format(curconv.getNextSentance().content,curconv.t);
                //curconv.reset();
                txtwrite.Say(sent.content, sent.textColor, sent.Alignment);
            }
            yield return new WaitForFixedUpdate();
        }
        //Debug.Log("NO LONGER IN CONVERSATION");
        //curconv = null;
    }
}

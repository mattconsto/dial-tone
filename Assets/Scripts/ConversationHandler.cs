using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationHandler : MonoBehaviour
{

    public TextWriter txtwrite;
    public TextWriter txtwritetapped;
    Conversation curconvNormal = null;
    Conversation curconvTapped = null;
    bool hasnext = false;


    public void setConversation(Conversation cov, bool isNormal)
    {
        Debug.Log("Trying to set cnv");
        if(isNormal)
        {
            if (curconvNormal != cov)
            {
                Debug.Log("Set cnv");
                curconvNormal = cov;
                StopCoroutine(sendConversation());
                StartCoroutine(sendConversation());
            }
        }
        else
        {
            if (curconvTapped != cov)
            {
                Debug.Log("Set cnv");
                curconvTapped = cov;
                StopCoroutine(sendConversationTapped());
                StartCoroutine(sendConversationTapped());
            }
        }
    }
    IEnumerator sendConversation()
    {
        Debug.Log("Started new sendConversation()");
        bool hasnext = curconvNormal.hasNextSentance();
        //Debug.Log("HasNext: " + hasnext);
        while (hasnext)
        {
            if (!txtwrite.speaking && hasnext)
            {
                //Debug.Log("sendConversation():"+curconv);
                SentanceObject sent = curconvNormal.getNextSentance();
                hasnext = curconvNormal.hasNextSentance();
                Debug.Log("[conversation-normal]" + sent.content);
                //curconv.getNextSentance().content = string.Format(curconv.getNextSentance().content,curconv.t);
                //curconv.reset();
                txtwrite.Say(sent.content, sent.textColor, sent.Alignment);
            }
            yield return new WaitForFixedUpdate();
        }
        //Debug.Log("NO LONGER IN CONVERSATION");
        //curconv = null;
    }
    IEnumerator sendConversationTapped()
    {
        Debug.Log("Started new sendConversation()");
        bool hasnext = curconvTapped.hasNextSentance();
        //Debug.Log("HasNext: " + hasnext);
        while (hasnext)
        {
            if (!txtwritetapped.speaking && hasnext)
            {
                SentanceObject sent = curconvTapped.getNextSentance();
                hasnext = curconvTapped.hasNextSentance();
                Debug.Log("[conversation-tapped]" + sent.content);
                txtwritetapped.Say(sent.content, sent.textColor, sent.Alignment);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}

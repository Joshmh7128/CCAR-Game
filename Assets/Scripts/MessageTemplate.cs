using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageTemplate : MonoBehaviour
{
    public string messageText; // what text are we sending
    public Text DisplayText; // what text are we displaying
    public int messageTypeID; // which set are we being used in?
    GameManager gameManager;

    private void Start()
    {
        // find and set our game manager needs
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        switch (messageTypeID)
        {
            case 1:
                gameManager.realMessagesA.Add(gameObject); // add ourselves to the list of messages in the scene
                gameManager.AdvanceMessages(messageTypeID);
                break;

            case 2:
                gameManager.realMessagesB.Add(gameObject); // add ourselves to the list of messages in the scene
                gameManager.AdvanceMessages(messageTypeID);
                break;

            case 3:
                gameManager.realMessagesC.Add(gameObject); // add ourselves to the list of messages in the scene
                gameManager.AdvanceMessages(messageTypeID);
                break;
        }

        // setup the text of our message
        DisplayText.text = messageText;
    }

    public void RaiseMessage() // use to raise the message up
    {
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + 95f, transform.position.z); // setup our target position
        transform.position = Vector3.Lerp(transform.position, targetPos, 2f); // lerp to that target pos
    }
}

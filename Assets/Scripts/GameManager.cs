using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // track the stage the story is in so we know what conversations to use
    public int Stage;

    // track which day we're on so far of the story. Stories consist of multiple days and will have to be scripted manually.
    public int currentDay;

    // track all the scripts that we CAN use
    public enum conversations
    {
        // it is important to note that only some conversations will work in some slots. I have
        // arranged them by the column below. From left to right, 1 2 3. This is done for optimization.
        // set 1 of our conversations
        Jessica, Karen, Robert,
        Jeff,    Mike,  Devin
    };

    //list all the conversations that we are using, then store them per-slot
    public conversations convoSlotA;
    public string[] convoSlotA_MessageData;
    // which message are we on?
    public int convoSlotA_MessageIndex;
    public conversations convoSlotB;
    public string[] convoSlotB_MessageData;
    // which message are we on?
    public int convoSlotB_MessageIndex;
    public conversations convoSlotC;
    public string[] convoSlotC_MessageData;
    // which message are we on?
    public int convoSlotC_MessageIndex;

    // get all the messages in the conversation
    public string[] JessicaMessages;
    public string[] KarenMessages;  
    public string[] RobertMessages;
    public string[] JeffMessages;
    public string[] MikeMessages;
    public string[] DevinMessages;

    // Start is called before the first frame update
    void Start()
    {
        // set our day to 1 since we're starting
        currentDay = 1;

        #region First Message Determination
        // setup our environment pieces based on which stage we're on
        /*
        if (Stage == 1)
        {
            convoSlotA = conversations.Jessica;
            convoSlotB = conversations.Karen;
            convoSlotC = conversations.Robert;
        }*/

        // run this to determine which conversation we're using
        ConvoDataAssign();
        #endregion First Message Determination
    }

    private void ConvoDataAssign()
    {
        // helper function to properly setup data assignments
        // this is done via switch case so that in the event we want to manually engage something
        // we have the ability to manually edit things more-so than we would if we were using a dictionary

        switch (convoSlotA)
        {
            // set the conversation message data based on the conversations we're using
            case (conversations)0:
                convoSlotA_MessageData = JessicaMessages;
                break;
            case (conversations)1:
                convoSlotA_MessageData = KarenMessages;
                break;
            case (conversations)2:
                convoSlotA_MessageData = RobertMessages;
                break;
            case (conversations)3:
                convoSlotA_MessageData = JeffMessages;
                break;
            case (conversations)4:
                convoSlotA_MessageData = MikeMessages;
                break;
            case (conversations)5:
                convoSlotA_MessageData = DevinMessages;
                break;

        }

        switch (convoSlotB)
        {
            // set the conversation message data based on the conversations we're using
            case (conversations)0:
                convoSlotB_MessageData = JessicaMessages;
                break;
            case (conversations)1:
                convoSlotB_MessageData = KarenMessages;
                break;
            case (conversations)2:
                convoSlotB_MessageData = RobertMessages;
                break;
            case (conversations)3:
                convoSlotB_MessageData = JeffMessages;
                break;
            case (conversations)4:
                convoSlotB_MessageData = MikeMessages;
                break;
            case (conversations)5:
                convoSlotB_MessageData = DevinMessages;
                break;

        }

        switch (convoSlotC)
        {
            // set the conversation message data based on the conversations we're using
            case (conversations)0:
                convoSlotC_MessageData = JessicaMessages;
                break;
            case (conversations)1:
                convoSlotC_MessageData = KarenMessages;
                break;
            case (conversations)2:
                convoSlotC_MessageData = RobertMessages;
                break;
            case (conversations)3:
                convoSlotC_MessageData = JeffMessages;
                break;
            case (conversations)4:
                convoSlotC_MessageData = MikeMessages;
                break;
            case (conversations)5:
                convoSlotC_MessageData = DevinMessages;
                break;

        }
    }

    private void FixedUpdate()
    {
        // run our message controller to deliver messages to the player
        MessageController();
    }

    // can the scripts continue to send messages to the player?
    private bool continueSend = true;

    // we need to manage all of our messages in order to make the system function.
    void MessageController()
    {
        // if we can continue to send messages, send them
        if (continueSend == true)
        {
            SendMessageToPlayer(convoSlotA_MessageData, convoSlotA_MessageIndex);
            SendMessageToPlayer(convoSlotB_MessageData, convoSlotB_MessageIndex);
            SendMessageToPlayer(convoSlotC_MessageData, convoSlotC_MessageIndex);
            // add one to each message index
            if (convoSlotA_MessageIndex < convoSlotA_MessageData.Length)
            {
                convoSlotA_MessageIndex += 1;
            }

            if (convoSlotB_MessageIndex < convoSlotB_MessageData.Length)
            {
                convoSlotB_MessageIndex += 1;
            }

            if (convoSlotC_MessageIndex < convoSlotC_MessageData.Length)
            {
                convoSlotC_MessageIndex += 1;
            }
        }
    }

    void SendMessageToPlayer(string[] MessageData, int MessageID)
    {
        // check to see if the message in question exists so we don't get a reference exception, and the program can continue
        if (MessageID < MessageData.Length)
        // check to see if we require a response
        if (MessageData[MessageID] == "RESPONSE")
        {
            continueSend = false; // since we need a response, stop sending messages
            PromptResponse(); // ask for a response
        }
        else
        {
            if (continueSend == true) // if we can continue sending messages
                                      // send the message based on the provided information
                if (MessageID > MessageData.Length)
                {
                    Debug.Log("REQUESTED MESSAGE ID IS OUT OF BOUNDS OF ARRAY. NO MESSAGE WILL BE SENT");
                }
                else
                {
                    Debug.Log(MessageData[MessageID]);
                }
        }
    }

    void PromptResponse()
    {
        Debug.Log("RESPONSE REQUIRED");
        // after the response is give, add 1 to the message ID. This is done in the SendMessageToPlayer Function
        continueSend = true;
    }

    void RunConvo()
    {

    }

}

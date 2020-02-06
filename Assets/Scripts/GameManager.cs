using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // here's a place to track the actual messages spawned in the game space
    public List<GameObject> realMessagesA;
    public List<GameObject> realMessagesB;
    public List<GameObject> realMessagesC;
    // here is our prefab object of player messages
    public GameObject userMessagePrefab;
    public GameObject npcMessagePrefab;
    // canvas and parent are the same object
    public Canvas messageCanvasA;
    public Canvas messageCanvasB;
    public Canvas messageCanvasC;
    // list all our buttons
    public Button canvasButtonA1;
    public Button canvasButtonA2;
    public Button canvasButtonA3;

    public Button canvasButtonB1;
    public Button canvasButtonB2;
    public Button canvasButtonB3;

    public Button canvasButtonC1;
    public Button canvasButtonC2;
    public Button canvasButtonC3;

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
        // start our messages
        StartCoroutine(MessageControlTime());
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
        //MessageController();
    }

    
    IEnumerator MessageControlTime()
    {
        yield return new WaitForSeconds(2f);
        MessageController();
        StartCoroutine(MessageControlTime());
    }

    // can the scripts continue to send messages to the player?
    public bool continueSend = true;
    public bool messageSend = true;

    // we need to manage all of our messages in order to make the system function.
    void MessageController()
    {
        // if we can continue to send messages, send them
        if (continueSend == true)
        {
            SendMessageToPlayer(convoSlotA_MessageData, convoSlotA_MessageIndex, 1);
            SendMessageToPlayer(convoSlotB_MessageData, convoSlotB_MessageIndex, 2);
            SendMessageToPlayer(convoSlotC_MessageData, convoSlotC_MessageIndex, 3);
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

    // get data from strings
    public static string getBetween(string strSource, string strStart, string strEnd)
    {
        int Start, End;
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            End = strSource.IndexOf(strEnd, Start);
            return strSource.Substring(Start, End - Start);
        }
        else
        {
            return "";
        }
    }
    /// example on how to use for reference:
    /// string text = "This is an example string and my data is here";
    /// string data = getBetween(text, "my", "is");

    void SendMessageToPlayer(string[] MessageData, int MessageID, int canvasID)
    {
        // check to see if the message in question exists so we don't get a reference exception, and the program can continue
        if (MessageID < MessageData.Length)
        // check to see if we require a response
        if (MessageData[MessageID].Contains("RESPONSE"))
        {
            continueSend = false; // since we need a response, stop sending messages
            PromptResponse(canvasID, MessageData[MessageID]); // ask for a response
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
                        switch (canvasID)
                        {
                            case 1:
                                    GameObject ourMessageA = Instantiate(npcMessagePrefab, messageCanvasA.transform); // get our message prefab
                                    ourMessageA.GetComponent<MessageTemplate>().messageTypeID = canvasID;
                                    ourMessageA.GetComponent<MessageTemplate>().messageText = MessageData[MessageID]; // set up our text
                                break;

                            case 2:
                                    GameObject ourMessageB = Instantiate(npcMessagePrefab, messageCanvasB.transform); // get our message prefab
                                    ourMessageB.GetComponent<MessageTemplate>().messageTypeID = canvasID;
                                    ourMessageB.GetComponent<MessageTemplate>().messageText = MessageData[MessageID]; // set up our text
                                break;

                            case 3:
                                    GameObject ourMessageC = Instantiate(npcMessagePrefab, messageCanvasC.transform); // get our message prefab
                                    ourMessageC.GetComponent<MessageTemplate>().messageTypeID = canvasID;
                                    ourMessageC.GetComponent<MessageTemplate>().messageText = MessageData[MessageID]; // set up our text
                                break;
                        }
                }
        }
    }

    IEnumerator WaitSecondsMessageSend(int seconds)
    {
        messageSend = false;
        yield return new WaitForSeconds(seconds);
        messageSend = true;
    }

    public void PromptResponse(int CanvasID, string responses)
    {
        Debug.Log("RESPONSE REQUIRED ON " + CanvasID);
        switch (CanvasID)
        {
            case 1: // get the message text, set the text to the button, make a new message
                canvasButtonA1.onClick.RemoveAllListeners();
                string messageTextA1 = getBetween(responses, "ONESTART", "ONEFIN");
                canvasButtonA1.transform.GetChild(0).GetComponent<Text>().text = messageTextA1;
                canvasButtonA1.onClick.AddListener(() => InstantiateUserMessage(messageTextA1, CanvasID));
                canvasButtonA1.onClick.AddListener(() => continueSend = true);

                canvasButtonA2.onClick.RemoveAllListeners();
                string messageTextA2 = getBetween(responses, "TWOSTART", "TWOFIN");
                canvasButtonA2.transform.GetChild(0).GetComponent<Text>().text = messageTextA2;
                canvasButtonA2.onClick.AddListener(() => InstantiateUserMessage(messageTextA2, CanvasID));
                canvasButtonA2.onClick.AddListener(() => continueSend = true);

                canvasButtonA3.onClick.RemoveAllListeners();
                string messageTextA3 = getBetween(responses, "THREESTART", "THREEFIN");
                canvasButtonA3.transform.GetChild(0).GetComponent<Text>().text = messageTextA3;
                canvasButtonA3.onClick.AddListener(() => InstantiateUserMessage(messageTextA3, CanvasID));
                canvasButtonA3.onClick.AddListener(() => continueSend = true);
                break;

            case 2:
                canvasButtonB1.onClick.RemoveAllListeners();
                string messageTextB1 = getBetween(responses, "ONESTBRT", "ONEFIN");
                canvasButtonB1.transform.GetChild(0).GetComponent<Text>().text = messageTextB1;
                canvasButtonB1.onClick.AddListener(() => InstantiateUserMessage(messageTextB1, CanvasID));
                canvasButtonB1.onClick.AddListener(() => continueSend = true);

                canvasButtonB2.onClick.RemoveAllListeners();
                string messageTextB2 = getBetween(responses, "TWOSTBRT", "TWOFIN");
                canvasButtonB2.transform.GetChild(0).GetComponent<Text>().text = messageTextB2;
                canvasButtonB2.onClick.AddListener(() => InstantiateUserMessage(messageTextB2, CanvasID));
                canvasButtonB2.onClick.AddListener(() => continueSend = true);

                canvasButtonB3.onClick.RemoveAllListeners();
                string messageTextB3 = getBetween(responses, "THREESTBRT", "THREEFIN");
                canvasButtonB3.transform.GetChild(0).GetComponent<Text>().text = messageTextB3;
                canvasButtonB3.onClick.AddListener(() => InstantiateUserMessage(messageTextB3, CanvasID));
                canvasButtonB3.onClick.AddListener(() => continueSend = true);
                break;

            case 3:
                canvasButtonC1.onClick.RemoveAllListeners();
                string messageTextC1 = getBetween(responses, "ONESTBRT", "ONEFIN");
                canvasButtonC1.transform.GetChild(0).GetComponent<Text>().text = messageTextC1;
                canvasButtonC1.onClick.AddListener(() => InstantiateUserMessage(messageTextC1, CanvasID));
                canvasButtonC1.onClick.AddListener(() => continueSend = true);

                canvasButtonC2.onClick.RemoveAllListeners();
                string messageTextC2 = getBetween(responses, "TWOSTBRT", "TWOFIN");
                canvasButtonC2.transform.GetChild(0).GetComponent<Text>().text = messageTextC2;
                canvasButtonC2.onClick.AddListener(() => InstantiateUserMessage(messageTextC2, CanvasID));
                canvasButtonC2.onClick.AddListener(() => continueSend = true);

                canvasButtonC3.onClick.RemoveAllListeners();
                string messageTextC3 = getBetween(responses, "THREESTBRT", "THREEFIN");
                canvasButtonC3.transform.GetChild(0).GetComponent<Text>().text = messageTextC3;
                canvasButtonC3.onClick.AddListener(() => InstantiateUserMessage(messageTextC3, CanvasID));
                canvasButtonC3.onClick.AddListener(() => continueSend = true);
                break;
        }

        // after the response is give, add 1 to the message ID. This is done in the SendMessageToPlayer Function
        //continueSend = true;
    }

    public void InstantiateUserMessage(string messageText, int canvasID)
    {
        if (continueSend == false)
        switch (canvasID)
        {
            case 1:
                GameObject ourMessageA = Instantiate(userMessagePrefab, messageCanvasA.transform);
                ourMessageA.GetComponent<MessageTemplate>().messageTypeID = canvasID;
                ourMessageA.GetComponent<MessageTemplate>().messageText = messageText;
                    canvasButtonA1.transform.GetChild(0).GetComponent<Text>().text = "";
                    canvasButtonA2.transform.GetChild(0).GetComponent<Text>().text = "";
                    canvasButtonA3.transform.GetChild(0).GetComponent<Text>().text = "";
                break;

            case 2:
                GameObject ourMessageB = Instantiate(userMessagePrefab, messageCanvasB.transform);
                ourMessageB.GetComponent<MessageTemplate>().messageTypeID = canvasID;
                ourMessageB.GetComponent<MessageTemplate>().messageText = messageText;
                    canvasButtonB1.transform.GetChild(0).GetComponent<Text>().text = "";
                    canvasButtonB2.transform.GetChild(0).GetComponent<Text>().text = "";
                    canvasButtonB3.transform.GetChild(0).GetComponent<Text>().text = "";
                    break;

            case 3:
                GameObject ourMessageC = Instantiate(userMessagePrefab, messageCanvasC.transform);
                ourMessageC.GetComponent<MessageTemplate>().messageTypeID = canvasID;
                ourMessageC.GetComponent<MessageTemplate>().messageText = messageText;
                    canvasButtonC1.transform.GetChild(0).GetComponent<Text>().text = "";
                    canvasButtonC2.transform.GetChild(0).GetComponent<Text>().text = "";
                    canvasButtonC3.transform.GetChild(0).GetComponent<Text>().text = "";
                    break;
        }
    }

    public void AdvanceMessages(int messageTypeID) // move all messages up one message height
    {   // switch which message we're moving up
        switch (messageTypeID)
        { 
            case 1:
                foreach (GameObject message in realMessagesA)
                {
                    message.GetComponent<MessageTemplate>().RaiseMessage();
                }
                break;

            case 2:
                foreach (GameObject message in realMessagesB)
                {
                    message.GetComponent<MessageTemplate>().RaiseMessage();
                }
                break;

            case 3:
                foreach (GameObject message in realMessagesC)
                {
                    message.GetComponent<MessageTemplate>().RaiseMessage();
                }
                break;
        }
    }

}

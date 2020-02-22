using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// This script contains all the data to run 3 different chats within a single scene. These chats are organized as chats
    /// 1, 2, and 3. Chat 1 correlates to the letter A, Chat 2 correlates to the letter B, and Chat 3 correlates to the letter C.
    /// All three chats are handled by the same script to increase optimization. This project has to be built to be future-proofed
    /// for WebGL exporting. Because of this, all variables are managed here. Currently all are public for ease of access via
    /// the game itself. Upon completion, optimization will take place, and I will assign pointers / privacy.
    /// </summary>

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
        JessicaOne, JessicaTwo, JessicaThree,
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
    public string[] JessicaMessagesDay1;
    public string[] JessicaMessagesDay2;
    public string[] JessicaMessagesDay3;

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

    // main menu buttons
    public Button mainMenuButton1;
    public Button mainMenuButton2;
    public Button mainMenuButton3;
    public Button mainMenuXButton1;
    public Button mainMenuXButton2;
    public Button mainMenuXButton3;

    // other buttons
    public Button sendButtonA;
    public Button sendButtonB;
    public Button sendButtonC;

    // Start is called before the first frame update
    void Start()
    {
        // manage our main menu
        mainMenuButton1.onClick.AddListener( () => AnimatePanel(1)    );
        mainMenuButton1.onClick.AddListener( () => Panel1State = true );
        mainMenuButton2.onClick.AddListener( () => AnimatePanel(2)    );
        mainMenuButton2.onClick.AddListener( () => Panel2State = true );
        mainMenuButton3.onClick.AddListener( () => AnimatePanel(3)    );
        mainMenuButton3.onClick.AddListener( () => Panel3State = true );
        // X buttons
        mainMenuXButton1.onClick.AddListener( () => AnimatePanel(1)     );
        mainMenuXButton1.onClick.AddListener( () => Panel1State = false );
        mainMenuXButton2.onClick.AddListener( () => AnimatePanel(2)     );
        mainMenuXButton2.onClick.AddListener( () => Panel2State = false );
        mainMenuXButton3.onClick.AddListener( () => AnimatePanel(3)     );
        mainMenuXButton3.onClick.AddListener( () => Panel3State = false );

        // Send Buttons, and their disabling of the objects when a message is sent
        // A section
        sendButtonA.onClick.AddListener( () => InstantiateUserMessage( customResponseInputA.text, 1) );
        sendButtonA.onClick.AddListener( () => continueSendA = true                                  );
        sendButtonA.onClick.AddListener( () => customResponseInputA.gameObject.SetActive(false)      );
        sendButtonA.onClick.AddListener( () => sendButtonA.gameObject.SetActive(false)               );
        // B section
        sendButtonB.onClick.AddListener( () => InstantiateUserMessage( customResponseInputB.text, 2) );
        sendButtonB.onClick.AddListener( () => continueSendB = true                                   );
        sendButtonB.onClick.AddListener( () => customResponseInputB.gameObject.SetActive(false)       );
        sendButtonB.onClick.AddListener( () => sendButtonB.gameObject.SetActive(false)                );
        // C section
        sendButtonC.onClick.AddListener( () => InstantiateUserMessage(customResponseInputC.text, 3)   );
        sendButtonC.onClick.AddListener( () => continueSendC = true                                   );
        sendButtonC.onClick.AddListener( () => customResponseInputC.gameObject.SetActive(false)       );
        sendButtonC.onClick.AddListener( () => sendButtonC.gameObject.SetActive(false)                );


        // set our day to 1 since we're starting
        currentDay = 1;

        // run this to determine which conversation we're using
        ConvoDataAssign();

        // start our messages
        StartCoroutine(MessageControlTime());
    }

    // set our states for our panels. true = on screen, false = off screen.
    bool Panel1State = false;
    bool Panel2State = false;
    bool Panel3State = false;

    // check to see if our conversations are complete or not
    public bool ConvoA_Complete = false;
    public bool ConvoB_Complete = false;
    public bool ConvoC_Complete = false;

    void AnimatePanel(int Convo)
    {
        if (Convo == 1)
        {
            if (Panel1State == false)
            {   // make our message canvas and our X button move in
                messageCanvasA.GetComponent<Animator>().Play("ENTERANIM"); mainMenuXButton1.GetComponent<Animator>().Play("ENTERANIM");
            }

            if (Panel1State == true)
            {
                messageCanvasA.GetComponent<Animator>().Play("EXITANIM"); mainMenuXButton1.GetComponent<Animator>().Play("EXITANIM");
            }
        }

        if (Convo == 2)
        {
            if (Panel2State == false)
            {   // make our message canvas and our X button move in
                messageCanvasB.GetComponent<Animator>().Play("ENTERANIM"); mainMenuXButton2.GetComponent<Animator>().Play("ENTERANIM");
            }

            if (Panel2State == true)
            {
                messageCanvasB.GetComponent<Animator>().Play("EXITANIM"); mainMenuXButton2.GetComponent<Animator>().Play("EXITANIM");
            }
        }

        if (Convo == 3)
        {
            if (Panel3State == false)
            {   // make our message canvas and our X button move in
                messageCanvasC.GetComponent<Animator>().Play("ENTERANIM"); mainMenuXButton3.GetComponent<Animator>().Play("ENTERANIM");
            }

            if (Panel3State == true)
            {
                messageCanvasC.GetComponent<Animator>().Play("EXITANIM"); mainMenuXButton3.GetComponent<Animator>().Play("EXITANIM");
            }
        }

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
                convoSlotA_MessageData = JessicaMessagesDay1;
                break;
            case (conversations)1:
                convoSlotA_MessageData = JessicaMessagesDay1;
                break;
            case (conversations)2:
                convoSlotA_MessageData = JessicaMessagesDay1;
                break;
            case (conversations)3:
                convoSlotA_MessageData = JessicaMessagesDay1;
                break;
            case (conversations)4:
                convoSlotA_MessageData = JessicaMessagesDay1;
                break;
            case (conversations)5:
                convoSlotA_MessageData = JessicaMessagesDay1;
                break;

        }

        switch (convoSlotB)
        {
            // set the conversation message data based on the conversations we're using
            case (conversations)0:
                convoSlotB_MessageData = JessicaMessagesDay1;
                break;
            case (conversations)1:
                convoSlotB_MessageData = JessicaMessagesDay1;
                break;
            case (conversations)2:
                convoSlotB_MessageData = JessicaMessagesDay1;
                break;
            case (conversations)3:
                convoSlotB_MessageData = JessicaMessagesDay1;
                break;
            case (conversations)4:
                convoSlotB_MessageData = JessicaMessagesDay1;
                break;
            case (conversations)5:
                convoSlotB_MessageData = JessicaMessagesDay1;
                break;

        }

        switch (convoSlotC)
        {
            // set the conversation message data based on the conversations we're using
            case (conversations)0:
                convoSlotC_MessageData = JessicaMessagesDay1;
                break;
            case (conversations)1:
                convoSlotC_MessageData = JessicaMessagesDay1;
                break;
            case (conversations)2:
                convoSlotC_MessageData = JessicaMessagesDay1;
                break;
            case (conversations)3:
                convoSlotC_MessageData = JessicaMessagesDay1;
                break;
            case (conversations)4:
                convoSlotC_MessageData = JessicaMessagesDay1;
                break;
            case (conversations)5:
                convoSlotC_MessageData = JessicaMessagesDay1;
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
    public bool continueSendA = true;
    public bool continueSendB = true;
    public bool continueSendC = true;
    public bool messageSend = true;

    // we need to manage all of our messages in order to make the system function.
    void MessageController()
    {
        Debug.Log("Message Control Fire");
        // if we can continue to send messages, send them
        if (continueSendA == true)
        {
            // send the message
            SendMessageToPlayer(convoSlotA_MessageData, convoSlotA_MessageIndex, 1);
            // add one to the index
            if (convoSlotA_MessageIndex < convoSlotA_MessageData.Length)
            {
                convoSlotA_MessageIndex += 1;
            }

            // if we have reached the end of the entire message log
            if (convoSlotA_MessageIndex >= convoSlotA_MessageData.Length)
            {
                //Debug.Log("Conversation Complete.");
                ConvoA_Complete = true;
            }
        }

        // if we can continue to send messages, send them
        if (continueSendB == true)
        {
            // send the message
            SendMessageToPlayer(convoSlotB_MessageData, convoSlotB_MessageIndex, 2);
            // add one to the index
            if (convoSlotB_MessageIndex < convoSlotB_MessageData.Length)
            {
                convoSlotB_MessageIndex += 1;
            }

            // if we have reached the end of the entire message log
            if (convoSlotB_MessageIndex >= convoSlotB_MessageData.Length)
            {
                //Debug.Log("Conversation Complete.");
                ConvoB_Complete = true;
            }
        }

        // if we can continue to send messages, send them
        if (continueSendC == true)
        {
            // send the message
            SendMessageToPlayer(convoSlotC_MessageData, convoSlotC_MessageIndex, 3);
            // add one to the index
            if (convoSlotC_MessageIndex < convoSlotC_MessageData.Length)
            {
                convoSlotC_MessageIndex += 1;   
            }

            // if we have reached the end of the entire message log
            if (convoSlotC_MessageIndex >= convoSlotC_MessageData.Length)
            {
                //Debug.Log("Conversation Complete.");
                ConvoC_Complete = true;
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

        // check to see if we require a response

        if (MessageID < MessageData.Length)
        if (MessageData[MessageID].Contains("RESPONSE"))
        {
                if (canvasID == 1)
                {
                    continueSendA = false; // since we need a response, stop sending messages
                    PromptResponse(canvasID, MessageData[MessageID]); // ask for a response
                }

                if (canvasID == 2)
                {
                    continueSendB = false; // since we need a response, stop sending messages
                    PromptResponse(canvasID, MessageData[MessageID]); // ask for a response
                }

                if (canvasID == 3)
                {
                    continueSendC = false; // since we need a response, stop sending messages
                    PromptResponse(canvasID, MessageData[MessageID]); // ask for a response
                }
        }
        else if (MessageData[MessageID].Contains("CUSTOM"))
        {
            if (canvasID == 1)
            {
                continueSendA = false;
                CustomResponse(canvasID);
            }

            if (canvasID == 2)
            {
                continueSendB = false;
                CustomResponse(canvasID);
            }

            if (canvasID == 3)
            {
                continueSendC = false;
                CustomResponse(canvasID);
            }
        }
        else switch (canvasID)
        {
            case 1:
                    if (continueSendA == true)
                    {
                        GameObject ourMessageA = Instantiate(npcMessagePrefab, messageCanvasA.transform); // get our message prefab
                        ourMessageA.GetComponent<MessageTemplate>().messageTypeID = canvasID;
                        ourMessageA.GetComponent<MessageTemplate>().messageText = MessageData[MessageID]; // set up our text
                    }
                break;

            case 2:
                    if (continueSendB == true)
                    {
                        GameObject ourMessageB = Instantiate(npcMessagePrefab, messageCanvasB.transform); // get our message prefab
                        ourMessageB.GetComponent<MessageTemplate>().messageTypeID = canvasID;
                        ourMessageB.GetComponent<MessageTemplate>().messageText = MessageData[MessageID]; // set up our text
                    }
                break;

            case 3:
                    if (continueSendC == true)
                    {
                        GameObject ourMessageC = Instantiate(npcMessagePrefab, messageCanvasC.transform); // get our message prefab
                        ourMessageC.GetComponent<MessageTemplate>().messageTypeID = canvasID;
                        ourMessageC.GetComponent<MessageTemplate>().messageText = MessageData[MessageID]; // set up our text
                    }
                break;
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
        //Debug.Log("RESPONSE REQUIRED ON " + CanvasID);
        switch (CanvasID)
        {
            case 1: // get the message text, set the text to the button, make a new message
                canvasButtonA1.onClick.RemoveAllListeners();
                string messageTextA1 = getBetween(responses, "ONESTART", "ONEFIN");
                canvasButtonA1.transform.GetChild(0).GetComponent<Text>().text = messageTextA1;
                canvasButtonA1.onClick.AddListener(() => InstantiateUserMessage(messageTextA1, CanvasID));
                canvasButtonA1.onClick.AddListener(() => continueSendA = true);

                canvasButtonA2.onClick.RemoveAllListeners();
                string messageTextA2 = getBetween(responses, "TWOSTART", "TWOFIN");
                canvasButtonA2.transform.GetChild(0).GetComponent<Text>().text = messageTextA2;
                canvasButtonA2.onClick.AddListener(() => InstantiateUserMessage(messageTextA2, CanvasID));
                canvasButtonA2.onClick.AddListener(() => continueSendA = true);

                canvasButtonA3.onClick.RemoveAllListeners();
                string messageTextA3 = getBetween(responses, "THREESTART", "THREEFIN");
                canvasButtonA3.transform.GetChild(0).GetComponent<Text>().text = messageTextA3;
                canvasButtonA3.onClick.AddListener(() => InstantiateUserMessage(messageTextA3, CanvasID));
                canvasButtonA3.onClick.AddListener(() => continueSendA = true);
                break;

            case 2:
                canvasButtonB1.onClick.RemoveAllListeners();
                string messageTextB1 = getBetween(responses, "ONESTART", "ONEFIN");
                canvasButtonB1.transform.GetChild(0).GetComponent<Text>().text = messageTextB1;
                canvasButtonB1.onClick.AddListener(() => InstantiateUserMessage(messageTextB1, CanvasID));
                canvasButtonB1.onClick.AddListener(() => continueSendB = true);

                canvasButtonB2.onClick.RemoveAllListeners();
                string messageTextB2 = getBetween(responses, "TWOSTART", "TWOFIN");
                canvasButtonB2.transform.GetChild(0).GetComponent<Text>().text = messageTextB2;
                canvasButtonB2.onClick.AddListener(() => InstantiateUserMessage(messageTextB2, CanvasID));
                canvasButtonB2.onClick.AddListener(() => continueSendB = true);

                canvasButtonB3.onClick.RemoveAllListeners();
                string messageTextB3 = getBetween(responses, "THREESTART", "THREEFIN");
                canvasButtonB3.transform.GetChild(0).GetComponent<Text>().text = messageTextB3;
                canvasButtonB3.onClick.AddListener(() => InstantiateUserMessage(messageTextB3, CanvasID));
                canvasButtonB3.onClick.AddListener(() => continueSendB = true);
                break;

            case 3:
                canvasButtonC1.onClick.RemoveAllListeners();
                string messageTextC1 = getBetween(responses, "ONESTART", "ONEFIN");
                canvasButtonC1.transform.GetChild(0).GetComponent<Text>().text = messageTextC1;
                canvasButtonC1.onClick.AddListener(() => InstantiateUserMessage(messageTextC1, CanvasID));
                canvasButtonC1.onClick.AddListener(() => continueSendC = true);

                canvasButtonC2.onClick.RemoveAllListeners();
                string messageTextC2 = getBetween(responses, "TWOSTART", "TWOFIN");
                canvasButtonC2.transform.GetChild(0).GetComponent<Text>().text = messageTextC2;
                canvasButtonC2.onClick.AddListener(() => InstantiateUserMessage(messageTextC2, CanvasID));
                canvasButtonC2.onClick.AddListener(() => continueSendC = true);

                canvasButtonC3.onClick.RemoveAllListeners();
                string messageTextC3 = getBetween(responses, "THREESTART", "THREEFIN");
                canvasButtonC3.transform.GetChild(0).GetComponent<Text>().text = messageTextC3;
                canvasButtonC3.onClick.AddListener(() => InstantiateUserMessage(messageTextC3, CanvasID));
                canvasButtonC3.onClick.AddListener(() => continueSendC = true);
                break;
        }

        // after the response is give, add 1 to the message ID. This is done in the SendMessageToPlayer Function
        //continueSend = true;
    }

    // custom response section
    public InputField customResponseInputA;
    public InputField customResponseInputB;
    public InputField customResponseInputC;

    public void CustomResponse(int CanvasID)
    {   // check which cavas we're working with
        switch (CanvasID)
        { // activate the correct response sections accordingly
            case 1:
                customResponseInputA.gameObject.SetActive(true);
                sendButtonA.gameObject.SetActive(true);
                break;

            case 2:
                customResponseInputB.gameObject.SetActive(true);
                sendButtonB.gameObject.SetActive(true);
                break;

            case 3:
                customResponseInputC.gameObject.SetActive(true);
                sendButtonC.gameObject.SetActive(true);
                break;
        }

    }

    public void InstantiateUserMessage(string messageText, int canvasID)
    {
        //if (continueSend == false)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageTemplate : MonoBehaviour
{
    public string messageText; // what text are we sending
    private Text DisplayText; // what text are we displaying

    private void OnEnable()
    {
        //DisplayText.text = messageText;
    }
}

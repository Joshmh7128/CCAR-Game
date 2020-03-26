using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDayButton : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject nextButton;

    private void Update()
    {
        // can we show our button?
        if (gameManager.ConvoA_Complete && gameManager.ConvoB_Complete && gameManager.ConvoC_Complete)
        {   // set it's activity to true
            nextButton.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoToSceneScript : MonoBehaviour
{
    [Header("Type in the name of the scene we want the button to react to.")]
    public string SceneName;
    public Button ourButton;

    void Start()
    {
        // add a listener to our button
        ourButton.onClick.AddListener(GoToNextScene);
    }

    void GoToNextScene()
    {
        // load our next scene
        SceneManager.LoadScene(SceneName);
    }
}

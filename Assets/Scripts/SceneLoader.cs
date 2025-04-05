using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.onClick.AddListener(() => StartGame());
        quitButton.onClick.AddListener(() => QuitGame());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(){
       // SceneManager.LoadScene("StartScene");   // Change to reflect name of actual scene
       Debug.Log("Start button clicked");
    }

    public void QuitGame(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
        Debug.Log("Quit Game");
    }
}

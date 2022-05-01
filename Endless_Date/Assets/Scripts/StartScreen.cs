using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void quitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;        //quit game in application or if in unity editor, quit game mode
    }

    public static class StartGameWith
    {
        static public bool load;    // this is reachable from everywhere
    }

    public void startGame()
    {
        StartGameWith.load = false;
        SceneManager.LoadScene("GameScene");
    }

    public void loadGame()
    {
        StartGameWith.load = true;
        SceneManager.LoadScene("GameScene");
    }

}

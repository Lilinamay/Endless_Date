using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//start screen button function
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

    public void startGame()         //start game from new, pass static bool load to game scene
    {
        StartGameWith.load = false;
        SceneManager.LoadScene("GameScene");
    }

    public void loadGame()         //start game from save , pass static bool load to game scene
    {
        StartGameWith.load = true;
        SceneManager.LoadScene("GameScene");
    }

}

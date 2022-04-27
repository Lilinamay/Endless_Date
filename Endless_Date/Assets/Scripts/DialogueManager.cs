using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextAsset inkFile;
    public GameObject textBox;
    public GameObject customButton;
    public GameObject optionPanel;
    public bool isTalking = false;

    static Story story;
    TMP_Text nametag;
    TMP_Text message;
    List<string> tags;
    static Choice choiceSelected;

    string savedJson;

    //Vector2 choiceSpace = Vector2;

    // Start is called before the first frame update
    void Start()
    {
        nametag = textBox.transform.GetChild(0).GetComponent<TMP_Text>();
        message = textBox.transform.GetChild(1).GetComponent<TMP_Text>();
        tags = new List<string>();
        choiceSelected = null;
        if (PlayerPrefs.HasKey("inkSaveState"))
        {
            story = new Story(inkFile.text);
            savedJson = PlayerPrefs.GetString("inkSaveState");
            story.state.LoadJson(savedJson);
            Debug.Log(savedJson);
            //RefreshView(); // or   refresh();
            if (story.currentChoices.Count != 0)
            {
                StartCoroutine(ShowChoices());
            }
            if (story.canContinue)
            {
                nametag.text = "Phoenix";
                AdvanceDialogue();

                //Are there any choices?
                if (story.currentChoices.Count != 0)
                {
                    StartCoroutine(ShowChoices());
                }
            }
            else
            {
                FinishDialogue();
            }
        }
        else
        {
            story = new Story(inkFile.text);
            AdvanceDialogue();
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            savedJson = story.state.ToJson();
            PlayerPrefs.SetString("inkSaveState", savedJson);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerPrefs.DeleteKey("inkSaveState");
            story.ResetState();
        }
        if (Input.GetKeyDown(KeyCode.Space) /*|| Input.GetMouseButtonDown(0)/*/)
        {
            //Is there more to the story?
            if (story.canContinue)
            {
                nametag.text = "Phoenix";
                AdvanceDialogue();

                //Are there any choices?
                if (story.currentChoices.Count != 0)
                {
                    StartCoroutine(ShowChoices());
                }
            }
            else
            {
                FinishDialogue();
            }
        }
    }

    // Finished the Story (Dialogue)
    private void FinishDialogue()
    {
        Debug.Log("End of Dialogue!");
    }

    // Advance through the story 
    void AdvanceDialogue()
    {
        string currentSentence = story.Continue();
        //story
        ParseTags();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    // Type out the sentence letter by letter and make character idle if they were talking
    IEnumerator TypeSentence(string sentence)
    {
        message.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            message.text += letter;
            yield return null;
        }
        //CharacterScript tempSpeaker = GameObject.FindObjectOfType<CharacterScript>();
        //if (tempSpeaker.isTalking)
        //{
        //    SetAnimation("idle");
        //}
        //yield return null;
    }

    // Create then show the choices on the screen until one got selected
    IEnumerator ShowChoices()
    {
        Debug.Log("There are choices need to be made here!");
        List<Choice> _choices = story.currentChoices;
        float bound = 210/ _choices.Count;
        float space = (optionPanel.GetComponent<RectTransform>().sizeDelta.y+bound)/ _choices.Count;
        Debug.Log(_choices.Count);
        Debug.Log("hight: " + (optionPanel.GetComponent<RectTransform>().sizeDelta.y + bound));
        Debug.Log("space: " +space);
        for (int i = 0; i < _choices.Count; i++)
        {
            
            GameObject temp = Instantiate(customButton, optionPanel.transform);
            temp.transform.position += Vector3.down * i * -space -Vector3.up * space;
            Debug.Log(temp.transform.position);
            //Debug.Log(temp);
            //Debug.Log(i);
            temp.transform.GetChild(0).GetComponent<TMP_Text>().text = _choices[i].text;
            temp.AddComponent<Selectable>();
            temp.GetComponent<Selectable>().element = _choices[i];
            temp.GetComponent<Button>().onClick.AddListener(() => { temp.GetComponent<Selectable>().Decide(); });
        }

        optionPanel.SetActive(true);

        yield return new WaitUntil(() => { return choiceSelected != null; });

        AdvanceFromDecision();
    }

    // Tells the story which branch to go to
    public static void SetDecision(object element)
    {
        choiceSelected = (Choice)element;
        story.ChooseChoiceIndex(choiceSelected.index);
    }

    // After a choice was made, turn off the panel and advance from that choice
    void AdvanceFromDecision()
    {
        optionPanel.SetActive(false);
        for (int i = 0; i < optionPanel.transform.childCount; i++)
        {
            Destroy(optionPanel.transform.GetChild(i).gameObject);
        }
        choiceSelected = null; // Forgot to reset the choiceSelected. Otherwise, it would select an option without player intervention.
        AdvanceDialogue();
    }

    /*** Tag Parser ***/
    /// In Inky, you can use tags which can be used to cue stuff in a game.
    /// This is just one way of doing it. Not the only method on how to trigger events. 
    void ParseTags()
    {
        tags = story.currentTags;
        foreach (string t in tags)
        {
            string prefix = t.Split(' ')[0];
            string param = t.Split(' ')[1];

            switch (prefix.ToLower())
            {
                case "emotion":
                    //SetAnimation(param);
                    Debug.Log(param);
                    break;
                case "color":
                    SetTextColor(param);
                    break;
            }
        }
    }
    void SetAnimation(string _name)
    {
        CharacterScript cs = GameObject.FindObjectOfType<CharacterScript>();
        cs.PlayAnimation(_name);
    }
    void SetTextColor(string _color)
    {
        switch (_color)
        {
            case "red":
                message.color = Color.red;
                break;
            case "blue":
                message.color = Color.cyan;
                break;
            case "green":
                message.color = Color.green;
                break;
            case "white":
                message.color = Color.white;
                break;
            default:
                Debug.Log($"{_color} is not available as a text color");
                break;
        }
    }

}

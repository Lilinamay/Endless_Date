using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextAsset inkFile;
    public TextAsset inkFileMom;

    public GameObject textBox;
    public GameObject customButton;
    public GameObject optionPanel;
    public bool isTalking = false;


    static Story story;
    TMP_Text nametag;
    TMP_Text message;
    public TMP_Text nametag2;
    public TMP_Text message2;
    
    List<string> tags;
    static Choice choiceSelected;

    string mainsavedJson;
    string tempsavedJson;
    string phonesavedJson;

    public bool main = true;
    [SerializeField] GameObject phoneEnterButton;
    [SerializeField] GameObject phoneExitButton;
    public Image phoneBack;
    [SerializeField] GameObject secondCanvas;

    //Vector2 choiceSpace = Vector2;

    // Start is called before the first frame update
    void Start()
    {
        phoneBack.enabled = false;
        phoneExitButton.SetActive(false);
        secondCanvas.SetActive(false);
        nametag = textBox.transform.GetChild(0).GetComponent<TMP_Text>();
        message = textBox.transform.GetChild(1).GetComponent<TMP_Text>();
        tags = new List<string>();
        choiceSelected = null;
        if (PlayerPrefs.HasKey("inkSaveStatePhone"))
        {
            story = new Story(inkFileMom.text);
            phonesavedJson = PlayerPrefs.GetString("inkSaveStatePhone");
            story.state.LoadJson(phonesavedJson);
            Debug.Log(phonesavedJson);
            phone();
        }
        else if (PlayerPrefs.HasKey("inkSaveStateMain"))
        {
            story = new Story(inkFile.text);
            mainsavedJson = PlayerPrefs.GetString("inkSaveStateMain");
            story.state.LoadJson(mainsavedJson);
            Debug.Log(mainsavedJson);
            //nametag.text = "Phoenix";
            message.text = story.currentText;
            if (story.currentChoices.Count != 0)
            {
                
                StartCoroutine(ShowChoices());
            }
            if (story.canContinue)
            {
                
                //AdvanceDialogue();

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
            if (main)
            {
                if (PlayerPrefs.HasKey("inkSaveStatePhone"))
                {
                    PlayerPrefs.DeleteKey("inkSaveStatePhone");
                }
                mainsavedJson = story.state.ToJson();
                PlayerPrefs.SetString("inkSaveStateMain", mainsavedJson);
            }
            else
            {
                phonesavedJson = story.state.ToJson();
                PlayerPrefs.SetString("inkSaveStatePhone", phonesavedJson);
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerPrefs.DeleteKey("inkSaveStateMain");
            if (PlayerPrefs.HasKey("inkSaveStateTemp"))
            {
                PlayerPrefs.DeleteKey("inkSaveStateTemp");
            }
            if (PlayerPrefs.HasKey("inkSaveStatePhone"))
            {
                PlayerPrefs.DeleteKey("inkSaveStatePhone");
            }
            story.ResetState();
            main = true;
            story = new Story(inkFile.text);
            AdvanceDialogue();

        }
        if (Input.GetKeyDown(KeyCode.Space) /*|| Input.GetMouseButtonDown(0)/*/)
        {
            //Is there more to the story?
            if (story.canContinue)
            {
                //nametag.text = "Phoenix";
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

        if (main)
        {
            phoneExitButton.SetActive(false);
            nametag2.text = "";
            message2.text = "";
            phoneBack.enabled = false;
            secondCanvas.SetActive(false);
        }
    }

    // Finished the Story (Dialogue)
    private void FinishDialogue()
    {
        if (main)
        {
            Debug.Log("End of Dialogue!");
        }
        else
        {
            
            Debug.Log("End of Side Dialogue!");


        }
    }

    // Advance through the story 
    void AdvanceDialogue()
    {
        string currentSentence = story.Continue();

        ParseTags();
        StopAllCoroutines();
        if (main)
        {
            StartCoroutine(TypeSentence(currentSentence));
        }
        else
        {
            message2.text = currentSentence;
        }
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
                case "name":
                    if (main)
                    {
                        nametag.text = param;
                    }
                    else
                    {
                        nametag2.text = param;
                    }
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

    public void phone()
    {
        secondCanvas.SetActive(true);
        phoneEnterButton.SetActive(false);
        phoneExitButton.SetActive(true);
        Debug.Log("triggered phone");
        main = false;
        //nametag2.text = "Phoenix";
        if (PlayerPrefs.HasKey("inkSaveStatePhone"))
        {
            message2.text = story.currentText;
        }
        else
        {
            tempsavedJson = story.state.ToJson();
            PlayerPrefs.SetString("inkSaveStateTemp", tempsavedJson);       //save  story progess in a temperary to go back to
            story = new Story(inkFileMom.text);
            tags = new List<string>();
            choiceSelected = null;
            AdvanceDialogue();
        }
        
        
        phoneBack.enabled = true;
        
    }

    public void exitPhone()
    {
        // go back to main
        
        if (PlayerPrefs.HasKey("inkSaveStateTemp"))
        {
            main = true;
            phoneExitButton.SetActive(false);
            nametag2.text = "";
            message2.text = "";
            phoneBack.enabled = false;
            //PlayerPrefs.DeleteKey("inkSaveStatePhone");
            story = new Story(inkFile.text);
            //mainsavedJson = PlayerPrefs.GetString("inkSaveStateTemp");
            tempsavedJson = PlayerPrefs.GetString("inkSaveStateTemp");
            story.state.LoadJson(tempsavedJson);
            Debug.Log(tempsavedJson);
            nametag.text = "Phoenix";
            message.text = story.currentText;
            if (story.currentChoices.Count != 0)
            {
                StartCoroutine(ShowChoices());
            }
            
        }
        else
        {
            Debug.Log("you shouldn't be here, error");
            main = true;
        }
    }


}

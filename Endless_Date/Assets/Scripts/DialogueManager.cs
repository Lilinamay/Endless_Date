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
    [SerializeField] List<TextAsset> phoneFiles;
    public int phoneindex = -1;


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
    string phonetempsavedJson;

    public bool main = true;
    [SerializeField] GameObject phoneEnterButton;
    [SerializeField] GameObject phoneExitButton;
    public Image phoneBack;
    [SerializeField] GameObject secondCanvas;
    [SerializeField] GameObject phoneBox;
    [SerializeField] GameObject phoneBoxSam;
    string myname;

    [SerializeField] float yPhoneboxOffset = 0;
    [SerializeField] float maxyPhonebox = -150;
    [SerializeField] List<GameObject> phoneboxes;
    public bool hasPhoneMessages= false;
    bool finishedPhone = false;
    public float randomPhoneTime = 0;
    [SerializeField] int timerMin = 0;
    [SerializeField] int timerMax = -150;
    void Start()
    {
        phoneBack.enabled = false;
        phoneEnterButton.SetActive(false);
        phoneExitButton.SetActive(false);
        secondCanvas.SetActive(false);
        nametag = textBox.transform.GetChild(0).GetComponent<TMP_Text>();
        message = textBox.transform.GetChild(1).GetComponent<TMP_Text>();
        tags = new List<string>();
        choiceSelected = null;
        if (PlayerPrefs.HasKey("inkSaveStatePhoneindex"))
        {
            phoneindex = PlayerPrefs.GetInt("inkSaveStatePhoneindex");      //retrive current side storyfile
        }
        if (PlayerPrefs.HasKey("inkSaveStatePhone"))                //if was in phone messages
        {
            main = false;
            hasPhoneMessages = true;
            story = new Story(phoneFiles[phoneindex].text);
            phonesavedJson = PlayerPrefs.GetString("inkSaveStatePhone");    //retrive current content
            story.state.LoadJson(phonesavedJson);
            Debug.Log("start game load from saved phone: " +phonesavedJson);
            phone();
            if (PlayerPrefs.HasKey("inkSaveStateTempPhone"))
            {
                phoneEnterButton.SetActive(true);
            }
        }
        else if (PlayerPrefs.HasKey("inkSaveStateMain"))
        {
            main = true;
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
            generatePhonetime();
            story = new Story(inkFile.text);
            AdvanceDialogue(story.Continue());
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
            PlayerPrefs.SetInt("inkSaveStatePhoneindex", phoneindex);
        }
        if (Input.GetKeyDown(KeyCode.O))                    //restart game
        {
            optionPanel.SetActive(false);
            phoneEnterButton.SetActive(false);
            phoneindex = -1;
            generatePhonetime();
            PlayerPrefs.DeleteKey("inkSaveStateMain");
            if (PlayerPrefs.HasKey("inkSaveStateTemp"))
            {
                PlayerPrefs.DeleteKey("inkSaveStateTemp");
            }
            if (PlayerPrefs.HasKey("inkSaveStatePhone"))
            {
                PlayerPrefs.DeleteKey("inkSaveStatePhone");
            }
            if (PlayerPrefs.HasKey("inkSaveStateTempPhone"))
            {
                PlayerPrefs.DeleteKey("inkSaveStateTempPhone");
            }
            if (PlayerPrefs.HasKey("inkSaveStatePhoneindex"))
            {
                PlayerPrefs.DeleteKey("inkSaveStatePhoneindex");
            }
            clearPhoneBox();
            story.ResetState();
            main = true;
            story = new Story(inkFile.text);
            AdvanceDialogue(story.Continue());

        }
        if (Input.GetKeyDown(KeyCode.Space) /*|| Input.GetMouseButtonDown(0)/*/)
        {
            //Is there more to the story?
            if (story.canContinue)
            {
                //nametag.text = "Phoenix";
                AdvanceDialogue(story.Continue());

                //Are there any choices?
                if (story.currentChoices.Count != 0)
                {
                    if (main)
                    {
                        StartCoroutine(ShowChoices());
                    }
                    else
                    {
                        StartCoroutine(ShowChoicesPhone());
                    }
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

        if (randomPhoneTime <= 0 && !hasPhoneMessages)                  //setactve new story button phonestartbutton
        {
            if (phoneindex < phoneFiles.Count)
            {
                hasPhoneMessages = true;
                phoneindex++;
                phoneEnterButton.SetActive(true);
                yPhoneboxOffset = 0;
            }
        }
        else if (hasPhoneMessages && randomPhoneTime <= 0)
        {

        }
        else
        {
            randomPhoneTime -= Time.deltaTime;
        }

        //if (!hasPhoneMessages)
        //{
        //    phoneEnterButton.SetActive(false);
        //}
        //else
        //{
        //    phoneEnterButton.SetActive(true);
        //}
    }

    void clearPhoneBox()
    {
        foreach (GameObject p in phoneboxes)
        {
            Destroy(p);                 //clear phoneboxes
        }
        phoneboxes.Clear();
    }

    void generatePhonetime()
    {
        randomPhoneTime = Random.Range(timerMin, timerMax);
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
            finishedPhone = true;
        }
    }

    // Advance through the story 
    void AdvanceDialogue(string currentSentence)
    {
        //string currentSentence = story.Continue();

        ParseTags();
        StopAllCoroutines();
        if (main)                                       //if in main, type sentence
        {
            StartCoroutine(TypeSentence(currentSentence));
        }
        else                                            //if in phone create phonetextbox and show text
        {
            //message2.text = currentSentence;
            float chatspace = 20;
            float bound = 50;
            float spaceX = (phoneBack.GetComponent<RectTransform>().sizeDelta.x - bound * 2) / 12;
            float spaceY = (phoneBack.GetComponent<RectTransform>().sizeDelta.y - bound * 1.5f) / 2;
            if (phoneboxes.Count > 0)
            {
                yPhoneboxOffset -= phoneboxes[phoneboxes.Count - 1].GetComponentInChildren<test>().yvalue + chatspace; // + bound;
                Debug.Log("value: " + phoneboxes[phoneboxes.Count - 1].GetComponentInChildren<test>().yvalue);
            }
            while (yPhoneboxOffset <= maxyPhonebox && phoneboxes.Count > 0)
            {
                print("check");
                GameObject temp = phoneboxes[0];
                phoneboxes.Remove(phoneboxes[0]);
                Destroy(temp);
                for (int i = 0; i < phoneboxes.Count; i++)
                {
                    if(i == 0)
                    {
                        yPhoneboxOffset = 0;
                    }
                    else
                    {
                        yPhoneboxOffset -= phoneboxes[i - 1].GetComponentInChildren<test>().yvalue + chatspace;
                    }
                    phoneboxes[i].transform.position = phoneBack.transform.position;
                    phoneboxes[i].transform.localPosition = new Vector3(-spaceX, spaceY + yPhoneboxOffset, 0);
                    Debug.Log("index: " + i+ " offset: " + yPhoneboxOffset + " previous y: " + phoneboxes[phoneboxes.Count - 1].GetComponentInChildren<test>().yvalue);
                }
                yPhoneboxOffset -= phoneboxes[phoneboxes.Count - 1].GetComponentInChildren<test>().yvalue + chatspace;

            }
            //
            GameObject phonebox = new GameObject();
            if (myname == "Sam")
            {
                phonebox = Instantiate(phoneBoxSam, phoneBack.transform);
            }
            else
            {
                phonebox = Instantiate(phoneBox, phoneBack.transform);
            }
            phoneboxes.Add(phonebox);
            phonebox.GetComponent<TMP_Text>().text = myname;
            phonebox.transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text = currentSentence;

            phonebox.transform.localPosition = new Vector3(-spaceX, spaceY + yPhoneboxOffset, 0);

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
    public void destoryChoices(GameObject choice)
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Destroy(choice);
        }
    }

    // Create then show the choices on the screen until one got selected
    IEnumerator ShowChoices()
    {
        Debug.Log("There are choices need to be made here!");
        List<Choice> _choices = story.currentChoices;
        float bound = 210/ _choices.Count;
        float space = (optionPanel.GetComponent<RectTransform>().sizeDelta.y+bound)/ _choices.Count;
        for (int i = 0; i < _choices.Count; i++)
        {
            
            GameObject temp = Instantiate(customButton, optionPanel.transform);
            temp.GetComponent<deleteChoiceSelf>().dialogueManager = gameObject;
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

    IEnumerator ShowChoicesPhone()
    {
        Debug.Log("There are Phone choices need to be made here!");
        List<Choice> _choices = story.currentChoices;
        float bound = 10/ _choices.Count;
        float space = (phoneBack.GetComponent<RectTransform>().sizeDelta.y + bound) / _choices.Count/4;
        for (int i = 0; i < _choices.Count; i++)
        {

            GameObject temp = Instantiate(customButton, phoneBack.transform);
            temp.GetComponent<deleteChoiceSelf>().dialogueManager = gameObject;
            temp.transform.position += Vector3.down * i * -space - Vector3.up * space- Vector3.up *100;
            Debug.Log(temp.transform.position);
            temp.transform.GetChild(0).GetComponent<TMP_Text>().text = _choices[i].text;
            temp.AddComponent<Selectable>();
            temp.GetComponent<Selectable>().element = _choices[i];
            temp.GetComponent<Button>().onClick.AddListener(() => { temp.GetComponent<Selectable>().Decide(); });
        }

        phoneBack.enabled = true;

        yield return new WaitUntil(() => { return choiceSelected != null; });
        //phoneBack.enabled = false;
        for (int i = 0; i < phoneBack.transform.childCount; i++)
        {
            if (phoneBack.transform.GetChild(i).gameObject.CompareTag("choice"))
            {
                Destroy(phoneBack.transform.GetChild(i).gameObject);
            }
        }
        choiceSelected = null; // Forgot to reset the choiceSelected. Otherwise, it would select an option without player intervention.
        AdvanceDialogue(story.Continue());
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
        AdvanceDialogue(story.Continue());
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
                case "name":            //set name
                    if (main)
                    {
                        nametag.text = param;
                    }
                    else
                    {
                        myname = param;
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
        //Debug.Log(phoneEnterButton.active);
        finishedPhone = false;
        Debug.Log("triggered phone");
        main = false;
        if (PlayerPrefs.HasKey("inkSaveStatePhone"))        ///if start game from saved phone
        {
            Debug.Log("load from saved phone story");
            AdvanceDialogue(story.currentText);
        }
        else
        {
            tempsavedJson = story.state.ToJson();
            PlayerPrefs.SetString("inkSaveStateTemp", tempsavedJson);       //save  story progess in a temperary to go back to
            if (PlayerPrefs.HasKey("inkSaveStateTempPhone"))                //if was in phone messages
            {
                Debug.Log("load from temp saved phone story");
                //phoneindex = PlayerPrefs.GetInt("inkSaveStatePhoneindex");      //retrive current storyfile
                story = new Story(phoneFiles[phoneindex].text);
                phonesavedJson = PlayerPrefs.GetString("inkSaveStateTempPhone");    //retrive current content
                story.state.LoadJson(phonesavedJson);
                Debug.Log(phonesavedJson);
                //AdvanceDialogue(story.currentText);
            }
            else
            {
                Debug.Log("load from new story");
                story = new Story(phoneFiles[phoneindex].text);
                tags = new List<string>();
                choiceSelected = null;
                AdvanceDialogue(story.Continue());
            }
        }
        if (story.currentChoices.Count != 0)
        {
            StartCoroutine(ShowChoicesPhone());
        }


        phoneBack.enabled = true;
        
    }

    public void exitPhone()
    {
        if (finishedPhone)                  //if all of the content are finished and exit phone, finished this phone context phone 
        {
            hasPhoneMessages = false;
            finishedPhone = false;
            clearPhoneBox();
            generatePhonetime();

            PlayerPrefs.DeleteKey("inkSaveStateTempPhone");
        }
        else
        {
            //PlayerPrefs.SetInt("inkSaveStatePhoneindex", phoneindex);
            phonetempsavedJson = story.state.ToJson();                      //save phone temp file 
            PlayerPrefs.SetString("inkSaveStateTempPhone", phonetempsavedJson);
            phoneEnterButton.SetActive(true);       //else allow player to click phone button to go back to the content
        }
        //hasPhoneMessages = false;
        if (PlayerPrefs.HasKey("inkSaveStateTemp"))         // go back to main
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
            //nametag.text = "Phoenix";
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

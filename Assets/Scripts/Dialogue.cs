using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using Defective.JSON;
using UnityEngine.SceneManagement;


     





public class Dialogue : MonoBehaviour
{
    public Color32 textDefaultColor;
    public Color32 textHoverColor;
    public float typingSpeed;
    public bool inDialogue = false;
    private JSONObject dialoguesJson;
    private string playerName = "B33-P";
    private int dialogueId;
    private string choicesType;
    private int choicesCount;
    private string currentLine;
    private List<int> choices = new List<int>();
    private bool nextReplica;
    private bool waitForChoice = false;
    private bool oneChoice;
    private bool exitDialogue = false;
    private bool actionDone = false;
    private bool actionDone2 = false;
    private bool requirementsMet = true;
    private List<int> choicesAvailable = new List<int>();
    private List<List<int>> ListOfChoicesAvailable;
    private bool minigameInterrupt = false;
    // private int avaialableChoice
    // Start is called before the first frame update




    private GameObject dialogueCanvas;
    private GameObject dialogueBox;
    private TextMeshProUGUI textComponent;
    private TextMeshProUGUI textComponent2;
    private TextMeshProUGUI textComponent3;
    private TextMeshProUGUI textDialogueName;
    private DialogueCanvasReference dialogueRef;
    private GameObject solderMinigame;
    private GameObject musicMinigame;
    private LevelLoader levelLoader;
    private ClickHandler clickHandler;
    // private bool assignVariables = true;

    void assignVariablesF()
    {
        // if (assignVariables) {
        dialogueRef = GameObject.Find("DialogueRef").GetComponent<DialogueCanvasReference>();
        dialogueCanvas = dialogueRef.dialogueCanvas;

        // dialogueBox = GameObject.Find("Dialogue box");
        // textComponent = GameObject.Find("Dialogue text option 1").GetComponent<TextMeshProUGUI>();
        // textComponent2 = GameObject.Find("Dialogue text option 2").GetComponent<TextMeshProUGUI>();
        // textComponent3 = GameObject.Find("Dialogue text option 3").GetComponent<TextMeshProUGUI>();
        // textDialogueName = GameObject.Find("Dialogue player name").GetComponent<TextMeshProUGUI>();
        // solderMinigame = GameObject.Find("solderMinigame");
        // musicMinigame = GameObject.Find("musicMinigame");
        inDialogue = false;
        dialogueBox = dialogueRef.dialogueBox;
        textDialogueName = dialogueRef.textDialogueName.GetComponent<TextMeshProUGUI>();
        textComponent = dialogueRef.textComponent.GetComponent<TextMeshProUGUI>();
        textComponent2 = dialogueRef.textComponent2.GetComponent<TextMeshProUGUI>();
        textComponent3 = dialogueRef.textComponent3.GetComponent<TextMeshProUGUI>();
        solderMinigame = dialogueRef.solderMinigame;
        musicMinigame = dialogueRef.musicMinigame;

        textComponent.text = string.Empty;
        textComponent2.text = string.Empty;
        textComponent3.text = string.Empty;
        textDialogueName.text = string.Empty;
        textComponent.color = textDefaultColor;
        textComponent2.color = textDefaultColor;
        textComponent3.color = textDefaultColor;
            

        dialogueCanvas.SetActive(false);
        if (solderMinigame != null)
        {
            solderMinigame.SetActive(false);
        }
        if (musicMinigame != null)
        {   
            musicMinigame.SetActive(false);
        }
        
        // assignVariables = false;
    }

    void checkIfValid()
    {
        if (levelLoader == null)
        {
            levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        }
        if (clickHandler == null)
        {
            clickHandler = FindObjectOfType<ClickHandler>();
        }
    }


    private LevelLoader getLevelLoader()
    {
        checkIfValid();
        return levelLoader;
    }

    private ClickHandler GetClickHandler()
    {
        checkIfValid();
        return clickHandler;
    }

    private void Awake()
    {
        if (PlayerData.firstScript)
        {
            // DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
            PlayerData.firstScript = false;
        }

        readJson();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Debug.Log(scene.name);
        // if (scene.name == "Skyscraper2") {
            // if(PlayerData.mayorStatus == "discovered") {
                // StartDialogue("michaldo");
            // }
        // }
        if ((scene.name != "MainMenu") && (scene.name != "Endings"))
        {
            assignVariablesF();
            
        }
        // else
        // {
            // assignVariables = true;
        // }
    }


    // void Start()
    // {

        // textComponent.text = string.Empty;
        // textComponent2.text = string.Empty;
        // textComponent3.text = string.Empty;
        // textDialogueName.text = string.Empty;
        // textComponent.color = textDefaultColor;
        // textComponent2.color = textDefaultColor;
        // textComponent3.color = textDefaultColor;
        // // gameObject.SetActive(false);
        // gameObject.transform.localScale = new Vector3(0, 0, 0);
        // readJson();

    // }

    // Update is called once per frame
    void Update()
    {

        if (inDialogue) {
            PlayerData.canMove = false;
        }

        // mousePosition = Input.mousePosition;
        // Ray castPoint = Camera.main.ScreenPointToRay(mousePosition);
        // RaycastHit hit;
        // 
        // if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        // {
            // Debug.Log(hit);
        // }

        // Debug.Log(mousePosition);
        // if (Input.GetKeyDown(KeyCode.Space) && !inDialogue)
        // {
            // StartDialogue("npc-test2");
        // }
        if (PlayerData.inMinigame)
        {
            return;
        }
        if (!PlayerData.inMinigame && minigameInterrupt)
        {
            dialogueCanvas.SetActive(true);
            // gameObject.transform.localScale = new Vector3(1, 1, 1);
            ContinueDialogue();
        }



        for (var i = 0; i < 2; i++) {     
            if (Input.GetMouseButtonUp(i)) {
                if (i == 0) {
                    if (actionDone)
                    {
                        actionDone = false;
                        return;
                    }
                }
                else {
                    if (actionDone2)
                    {
                        actionDone2 = false;
                        return;
                    }
                
                }
            }
            if (Input.GetMouseButtonDown(i))
            {
                if (oneChoice)
                {
                    if ((textComponent.text != currentLine))
                    {
                        // Debug.Log("finish");
                        if (waitForChoice) {
                            StopAllCoroutines();
                            textComponent.text = currentLine;
                            if (i == 0) {
                                actionDone = true;
                            }
                            else {
                                actionDone2 = true;
                            }
                            
                            return;
                        }

                    }
                    choices.Add(0);
                    waitForChoice = false;
                    ContinueDialogue();
                    if (i == 0) {
                        actionDone = true;
                    }
                    else {
                        actionDone2 = true;
                    }
                }
        }
        //     // switch to next line
        //     if (textComponent.text == currentLine && !waitForChoice)
        //     {
        //         Debug.Log("next line");

        //         // NextLine();
        //         ignoreNextClick = true;
        //         ContinueDialogue();
        //     }
        //     // finish typing
        //     if (textComponent.text != currentLine && !waitForChoice)
        //     {
        //         // Debug.Log("finish");
        //         StopAllCoroutines();
        //         textComponent.text = currentLine;
        //     }
        }
    }

    void readJson()
    {
        // string path = "Assets/Json/dialogues.json";
        // string jsonString = File.ReadAllText(path);
        // dialoguesJson = JsonUtility.FromJson<NpcsData>(jsonString);
        dialoguesJson = new JSONObject(JsonData.dialogue);
        // Debug.Log(dialoguesJson["npcs"][0]["name"]);
    }

    public void TextHover(int n)
    {
        if (oneChoice) {
            return;
        }
        if (n == 1) {
            textComponent.color = textHoverColor;
        }
        else if (n == 2) {
            textComponent2.color = textHoverColor;
        }
        else if (n == 3) {
            textComponent3.color = textHoverColor;
        }
    }

    public void TextHoverExit(int n)
    {
        if (n == 1) {
            textComponent.color = textDefaultColor;
        }
        else if (n == 2) {
            textComponent2.color = textDefaultColor;
        }
        else if (n == 3) {
            textComponent3.color = textDefaultColor;
        }
    }

    public void TextClick(int n)
    {
        if (PlayerData.inMinigame)
        {
            return;
        }

        if (actionDone)
        {
            actionDone = false;
            return;
        }

        if (actionDone2)
        {
            actionDone2 = false;
            return;
        }

        if (waitForChoice) {
            if (n == 1) {
                if ((textComponent.text != currentLine) && oneChoice)
                {
                    // Debug.Log("finish");
                    StopAllCoroutines();
                    textComponent.text = currentLine;
                    return;
                }
                choices.Add(0);
                waitForChoice = false;
                ContinueDialogue();
            }
            else if ((n == 2) && (choicesCount != 1)) {
                choices.Add(1);
                waitForChoice = false;
                ContinueDialogue();
            }
            else if ((n == 3) && (choicesCount == 3)) {
                choices.Add(2);
                waitForChoice = false;
                ContinueDialogue();
            }
        }

    }
        
    public void StartDialogueWithString(JSONObject dialogueContent)
    {
        // Example
        // {
        //     "dialogueName": "Spare part #01"
        //     "dialogueLine": "Spare part ...."
        // }
        

        if (inDialogue)
        {
            return;
        }
        

        inDialogue = true;
        choices.Clear();
        currentLine = string.Empty;
        dialogueCanvas.SetActive(true);
        // gameObject.transform.localScale = new Vector3(1, 1, 1);
        ListOfChoicesAvailable = new List<List<int>>();
        textDialogueName.text = dialogueContent["dialogueName"].stringValue + ": ";
        currentLine = dialogueContent["dialogueLine"].stringValue;
        textComponent.alignment = TextAlignmentOptions.TopLeft;
        StartCoroutine(TypeLine(currentLine));
        waitForChoice = true;
        oneChoice = true;
        exitDialogue = true;
        return;
    }

    public void StartDialogue(string npcId)
    {
        if (inDialogue)
        {
            return;
        }


        inDialogue = true;
        exitDialogue = false;
        choices.Clear();
        currentLine = string.Empty;

        dialogueCanvas.SetActive(true);
        // gameObject.transform.localScale = new Vector3(1, 1, 1);
        ListOfChoicesAvailable = new List<List<int>>();
        // gameObject.SetActive(true);

        for (int i = 0; i < dialoguesJson["npcs"].list.Count; i++)
        {
            if (dialoguesJson["npcs"][i]["id"].stringValue == npcId)
            {
                dialogueId = i;
                break;
            }
        }
        requirementsMet = true;
        foreach (JSONObject requirement in dialoguesJson["npcs"][dialogueId]["requirements"]) {
            if (!checkRequirements(requirement))
            {
                requirementsMet = false;
                break;
            }
        }
        if (!requirementsMet)
        {
            currentLine = dialoguesJson["npcs"][dialogueId]["notFullfilingRequirements"].stringValue;
            textComponent.alignment = TextAlignmentOptions.TopLeft;
            StartCoroutine(TypeLine(currentLine));
            waitForChoice = true;
            oneChoice = true;
            exitDialogue = true;
            // ExitDialogue();
            return;
        }
        // Debug.Log(dialoguesJson["npcs"][dialogueId]["requirements"]);
        // for (int i = 0; i < dialoguesJson["npcs"]["requirements"].list.Count; i++) {
            // Debug.Log(dialoguesJson["npcs"]["requirements"][i].ToString());
        // }
        choicesType = dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["type"].stringValue;
        choicesCount = dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"].list.Count;
        requirementsMet = true;
        choicesAvailable = new List<int>();
        if (choicesType == "npc") {
            if (dialoguesJson["npcs"][dialogueId]["name"].stringValue == "") {
                textDialogueName.text = "";
            }
            else {
                textDialogueName.text = dialoguesJson["npcs"][dialogueId]["name"].stringValue + ": ";
            }
            // conditional choice selection
            // for (choice in dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"].list)
            for (int i = 0; i < dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"].list.Count; i++)
            {
                if (dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"][i]["requirements"].list != null)
                {
                    requirementsMet = true;
                    foreach (JSONObject requirement in dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"][i]["requirements"].list)
                    {
                        if (!checkRequirements(requirement))
                        {
                            requirementsMet = false;
                            break;
                        }
                    }
                    if (requirementsMet)
                    {
                        choicesAvailable.Add(i);
                    }  
                }
                else
                {
                    choicesAvailable.Add(i);  
                }
            }
            // choices.Add(choicesAvailable[0]);
            ListOfChoicesAvailable.Add(choicesAvailable);


            currentLine = dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"][choicesAvailable[0]]["line"].stringValue;
            textComponent.alignment = TextAlignmentOptions.TopLeft;
            StartCoroutine(TypeLine(currentLine));
            waitForChoice = true;
            oneChoice = true;

            
            return;
        }
        requirementsMet = true;
        choicesAvailable = new List<int>();
        if (choicesType == "player") {
            if (dialoguesJson["npcs"][dialogueId].GetField("customDialogue")) {
                textDialogueName.text = dialoguesJson["npcs"][dialogueId]["name"].stringValue;
            }
            else {
                textDialogueName.text = playerName + ": ";
            }
            // conditional choice selection
            // for (choice in dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"].list)
            for (int i = 0; i < dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"].list.Count; i++)
            {
                if (dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"][i]["requirements"].list != null)
                {
                    requirementsMet = true;
                    foreach (JSONObject requirement in dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"][i]["requirements"].list)
                    {
                        if (!checkRequirements(requirement))
                        {
                            requirementsMet = false;
                            break;
                        }
                    }
                    if (requirementsMet)
                    {
                        choicesAvailable.Add(i);
                    }
                }
                else
                {
                    choicesAvailable.Add(i);
                }
            }
            choicesCount = choicesAvailable.Count;
        }
        // Debug.Log("Added list");
        ListOfChoicesAvailable.Add(choicesAvailable);
        if (choicesCount == 1) {
            // choices.Add(0);
            textComponent.alignment = TextAlignmentOptions.TopLeft;
            currentLine = dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"][choicesAvailable[0]]["line"].stringValue;
            waitForChoice = true;
            oneChoice = true;
            StartCoroutine(TypeLine(currentLine));
        }
        else if (choicesCount == 2) {
            textComponent.alignment = TextAlignmentOptions.Left;
            textComponent.text = dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"][choicesAvailable[0]]["line"].stringValue;
            textComponent2.text = dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"][choicesAvailable[1]]["line"].stringValue;
            waitForChoice = true;
            oneChoice = false;
        }
        else if (choicesCount == 3) {
            textComponent.alignment = TextAlignmentOptions.Left;
            textComponent.text = dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"][choicesAvailable[0]]["line"].stringValue;
            textComponent2.text = dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"][choicesAvailable[1]]["line"].stringValue;
            textComponent3.text = dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"][choicesAvailable[2]]["line"].stringValue;
            waitForChoice = true;
            oneChoice = false;
        }
        
        
    }

    private void ExitDialogue()
    {
        currentLine = string.Empty;
        dialogueCanvas.SetActive(false);
        StartCoroutine(ExitDialogueWithDelay());   

    }

    private IEnumerator ExitDialogueWithDelay()
    {
        yield return new WaitForSeconds(0.05f);
        inDialogue = false;
        PlayerData.canMove = true;
        PlayerData.CheckAIEnding();
    }

    public void AddItemToInv(string item)
    {
        Inventory.AddItem(item);
    }
    
    public void RemoveItemFromInv(string item)
    {
        Inventory.RemoveItem(item);
    }

    void StartSolderMinigame()
    {
        if (PlayerData.minigameSolderStatus != null)
        {
            return;
        }
        solderMinigame.SetActive(true);
        PlayerData.inMinigame = true;
        minigameInterrupt = true;
        
    }

    void startMusicMinigame()
    {
        if (PlayerData.minigameMusicStatus != null)
        {
            return;
        }
        musicMinigame.SetActive(true);
        PlayerData.inMinigame = true;
        minigameInterrupt = true;
    }

    bool HandleOutcomes(JSONObject outcomes)
    {
        if (outcomes.list == null)
        {
            return false;
        }

        bool state = false;

        foreach (JSONObject outcome in outcomes.list)
        {
            switch (outcome.GetField("id").stringValue)
            {
                case "item-add":
                    // Debug.Log("item-add: " + outcome.GetField("value").stringValue);
                    Inventory.AddItem(outcome.GetField("value").stringValue);
                    // PlayerData.coins += outcome.GetField("value").floatValue;
                    break;
                case "item-remove":
                    // Debug.Log("item-remove: " + outcome.GetField("value").stringValue);
                    Inventory.RemoveItem(outcome.GetField("value").stringValue);
                    // PlayerData.coins += outcome.GetField("value").floatValue;
                    break;
                case "mayor-status":
                    if(SceneManager.GetActiveScene().buildIndex == 5) {
                        if (outcome.GetField("value").stringValue == "dead") {
                            // leave dialogue switch to scene skyscraper 1
                            getLevelLoader().LoadNextLevel(4);
                        }
                    }

                    PlayerData.mayorStatus = outcome.GetField("value").stringValue;
                    break;
                case "hamper-state":
                    PlayerData.hamperState = outcome.GetField("value").stringValue;
                    break;
                case "minigame":
                    switch (outcome.GetField("value").stringValue)
                    {
                        case "solder-minigame":
                            StartSolderMinigame();
                            break;
                        case "music-minigame":
                            startMusicMinigame();
                            break;
                        default:
                            break;
                    }
                    break;
                case "music-minigame-status":
                    if (outcome.GetField("value").type == JSONObject.Type.Null)
                    {
                        PlayerData.minigameMusicStatus = null;
                        break;
                    }
                    PlayerData.minigameMusicStatus = outcome.GetField("value").boolValue;
                    break;
                case "abbz-first-time":
                    PlayerData.abbzFirstTime = outcome.GetField("value").boolValue;
                    break;
                case "abbz-state":
                    PlayerData.abbzState = outcome.GetField("value").stringValue;
                    break;
                case "if-talked-to-abbz-set-talked-to-oliver":
                    PlayerData.talkedWithOliver = true;
                    if (!PlayerData.abbzFirstTime)
                    {
                        PlayerData.talkedWithOliverAfterAbbz = true;
                    }
                    break;
                case "generator-fixed":
                    PlayerData.generatorFixed = outcome.GetField("value").boolValue;
                    break;
                case "repaired":
                    PlayerData.repaired = outcome.GetField("value").boolValue;
                    break;
                case "atmosphereDestroyed":
                    PlayerData.atmosphereDestroyed = outcome.GetField("value").boolValue;
                    break;
                case "pick-up-item":
                    if (outcome.GetField("value").boolValue)
                    {
                        PlayerData.IncrementBatteryLevel(-1);
                        // PlayerData.batteriesLevel -= 1;
                        GetClickHandler().PickUp(PlayerData.itemToPickUp);
                        PlayerData.itemToPickUp = "";
                    }
                    else {
                        PlayerData.itemToPickUp = "";
                    }
                    break;
                case "battery-cell":
                    PlayerData.IncrementBatteryLevel(outcome.GetField("value").intValue);
                    // PlayerData.batteriesLevel += outcome.GetField("value").intValue;
                    break;
                case "main-scene":
                    getLevelLoader().LoadNextLevel(1);
                    break;
                case "ending":
                    GetClickHandler().LoadEnding(outcome.GetField("value").intValue);
                    Debug.Log("Ending: " + outcome.GetField("value").intValue);
                    break;
                case "parent-up":
                    choices.RemoveRange(choices.Count - outcome.GetField("value").intValue, outcome.GetField("value").intValue);
                    ListOfChoicesAvailable.RemoveRange(ListOfChoicesAvailable.Count - outcome.GetField("value").intValue, outcome.GetField("value").intValue);
                    return true;
                case "feed-cat":
                    PlayerData.FeedCat();
                    break;
                case "water-flower":
                    PlayerData.IncrementFlowersWatered();
                    break;
                // case "karma":
                //     PlayerData.karma += outcome.GetField("value").floatValue;
                //     break;
                // case "gold":
                //     PlayerData.coins += outcome.GetField("value").floatValue;
                //     break;

                default:
                    break;
                // satet
                    // return false;
            }
        }
        return state;
    }

    void ContinueDialogue()
    {
        if (!inDialogue || waitForChoice || PlayerData.inMinigame)
        {
            return;
        }
        textComponent.text = string.Empty;
        textComponent2.text = string.Empty;
        textComponent3.text = string.Empty;
        textDialogueName.text = string.Empty;
        textComponent.color = textDefaultColor;
        textComponent2.color = textDefaultColor;
        textComponent3.color = textDefaultColor;
        if (exitDialogue)
        {
            ExitDialogue();
            return;
        }

        
        JSONObject parent = dialoguesJson["npcs"][dialogueId]["lines"]["replica"];
        JSONObject parentTop = parent;
        for (int i = 0; i < choices.Count; i++)
        {
            // Debug.Log("choices len: " + choices.Count);
            // Debug.Log("choices[i]: " + choices[i]);
            // Debug.Log("len of listofchoicesavailable: " + ListOfChoicesAvailable.Count);
            // Debug.Log("i: " + i);
            // Debug.Log(" choices[i]: " + choices[i] + "join ListOfChoicesAvailable[i]: " + string.Join("; ", ListOfChoicesAvailable[i]));
            int choiceIndex = ListOfChoicesAvailable[i][choices[i]];
            // Debug.Log("choices index: " + choiceIndex);
            if (parent["choices"][choiceIndex].GetField("next") == null)
            {
                // inDialogue = false;
                // currentLine = string.Empty;
                HandleOutcomes(parent["choices"][choiceIndex]["outcome"]);
                // Debug.Log(parent["choices"][choice]["line"]);
                ExitDialogue();

                return;
            }
            // Debug.Log("parentsTop: " + parent["choices"][choice].ToString());
            parentTop = parent["choices"][choiceIndex];
            parent = parent["choices"][choiceIndex]["next"]["replica"];
            
            

        }
        // parent up statement
        if (!minigameInterrupt) {
            if(HandleOutcomes(parentTop["outcome"]))
            {
                ContinueDialogue();
                return;
            }
        }
        else {
            minigameInterrupt = false;
        }
        if (PlayerData.inMinigame)
        {   
            dialogueCanvas.SetActive(false);
            // gameObject.transform.localScale = new Vector3(0, 0, 0);
            return;
        }
        // Debug.Log(parentTop["line"]);

        choicesType = parent["type"].stringValue;
        choicesCount = parent["choices"].list.Count;
        requirementsMet = true;
        choicesAvailable = new List<int>();
        if (choicesType == "npc") {
            if (dialoguesJson["npcs"][dialogueId].GetField("customDialogue"))
            {
                textDialogueName.text = dialoguesJson["npcs"][dialogueId]["npc-name"].stringValue + ": ";
            }
            else {
                if (dialoguesJson["npcs"][dialogueId]["name"].stringValue == "") {
                    textDialogueName.text = "";
                }
                else {
                    textDialogueName.text = dialoguesJson["npcs"][dialogueId]["name"].stringValue + ": ";
                }
                
            }
            // conditional choice selection
            // for (choice in dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"].list)
            for (int i = 0; i < parent["choices"].list.Count; i++)
            {
                // Debug.Log(parent["choices"][i]["requirements"].list);
                // int count = parent["choices"][i]["requirements"].list.Count;
                // Debug.Log(count);
                if (parent["choices"][i]["requirements"].list != null)
                {
                    requirementsMet = true;
                    // Debug.Log(parent.ToString());
                    foreach (JSONObject requirement in parent["choices"][i]["requirements"].list)
                    {
                        if (!checkRequirements(requirement))
                        {
                            requirementsMet = false;
                            break;
                        }
                    }
                    if (requirementsMet)
                    {
                        choicesAvailable.Add(i);
                    }
                }
                else
                {
                    choicesAvailable.Add(i);
                }
            }
            currentLine = parent["choices"][choicesAvailable[0]]["line"].stringValue;
            ListOfChoicesAvailable.Add(choicesAvailable);
            textComponent.alignment = TextAlignmentOptions.TopLeft;
            StartCoroutine(TypeLine(currentLine));
            waitForChoice = true;
            oneChoice = true;
            return;
        }
        requirementsMet = true;
        choicesAvailable = new List<int>();
        if (choicesType == "player") {
            // if (dialoguesJson["npcs"][dialogueId].GetField("customDialogue")) {
                // textDialogueName.text = dialoguesJson["npcs"][dialogueId]["name"].stringValue;
            // }
            // else {
            textDialogueName.text = playerName + ": ";
            // }
            
            // conditional choice selection
            // for (choice in dialoguesJson["npcs"][dialogueId]["lines"]["replica"]["choices"].list)
            for (int i = 0; i < parent["choices"].list.Count; i++)
            {
                // int count = parent["choices"][i]["requirements"].list.Count;
                if (parent["choices"][i]["requirements"].list != null)
                {
                    requirementsMet = true;
                    foreach (JSONObject requirement in parent["choices"][i]["requirements"].list)
                    {
                        if (!checkRequirements(requirement))
                        {
                            requirementsMet = false;
                            break;
                        }
                    }
                    if (requirementsMet)
                    {
                        choicesAvailable.Add(i);
                    }
                }
                else
                {
                    choicesAvailable.Add(i);
                }
            }
            choicesCount = choicesAvailable.Count;
            // Debug.Log("a " + choicesAvailable);
            // foreach (int c in choicesAvailable)
            // {
                // Debug.Log("c " + c);
            // }
            // Debug.Log("cc " + choicesCount);
        }
        ListOfChoicesAvailable.Add(choicesAvailable);
        // Debug.Log(c)
        // Debug.Log("choicesType: " + choicesType);
        // Debug.Log("choicesCount: " + choicesCount);
        // foreach (JSONObject choice in availableChoices["choices"].list)
        // {
            // Debug.Log(choice.line);
        // }
        // Debug.Log(parent);
        if (choicesCount == 1) {
            // choices.Add(0);
            textComponent.alignment = TextAlignmentOptions.TopLeft;
            currentLine = parent["choices"][choicesAvailable[0]]["line"].stringValue;
            waitForChoice = true;
            oneChoice = true;
            StartCoroutine(TypeLine(currentLine));
        }
        else if (choicesCount == 2) {
            textComponent.alignment = TextAlignmentOptions.Left;
            textComponent.text = parent["choices"][choicesAvailable[0]]["line"].stringValue;
            textComponent2.text = parent["choices"][choicesAvailable[1]]["line"].stringValue;
            waitForChoice = true;
            oneChoice = false;
        }
        else if (choicesCount == 3) {
            textComponent.alignment = TextAlignmentOptions.Left;
            textComponent.text = parent["choices"][choicesAvailable[0]]["line"].stringValue;
            textComponent2.text = parent["choices"][choicesAvailable[1]]["line"].stringValue;
            textComponent3.text = parent["choices"][choicesAvailable[2]]["line"].stringValue;
            waitForChoice = true;
            oneChoice = false;
        }
        
    }
    bool checkRequirements(JSONObject requirement)
    {
        switch (requirement.GetField("id").stringValue)
        {
            // case "level-min":
            //     return (PlayerData.level >= requirement.GetField("value").floatValue);

            // case "level-max":
            //     return (PlayerData.level <= requirement.GetField("value").floatValue);

            // case "karma-min":
            //     return (PlayerData.karma >= requirement.GetField("value").floatValue);

            // case "karma-max":
            //     return (PlayerData.karma <= requirement.GetField("value").floatValue);

            // case "gold-min":
            //     return (PlayerData.coins >= requirement.GetField("value").floatValue);

            // case "gold-max":
            //     return (PlayerData.coins <= requirement.GetField("value").floatValue);
            case "solder-minigame":
                if (requirement.GetField("value").type == JSONObject.Type.Null)
                {
                    return PlayerData.minigameSolderStatus == null;
                }
                return PlayerData.minigameSolderStatus == requirement.GetField("value").boolValue;
            case "music-minigame":
                if (requirement.GetField("value").type == JSONObject.Type.Null)
                {
                    return PlayerData.minigameMusicStatus == null;
                }
                return PlayerData.minigameMusicStatus == requirement.GetField("value").boolValue;
            case "mayor-visible":
                return (PlayerData.mayorStatus == "discovered" || PlayerData.mayorStatus == "saved") == requirement.GetField("value").boolValue;
            case "mayor-status":
                return PlayerData.mayorStatus == requirement.GetField("value").stringValue;
            case "hamper-state":
                return PlayerData.hamperState == requirement.GetField("value").stringValue;
            case "abbz-first-time":
                return PlayerData.abbzFirstTime == requirement.GetField("value").boolValue;
            case "abbz-state":
                return PlayerData.abbzState == requirement.GetField("value").stringValue;
            case "talked-with-oliver":
                return PlayerData.talkedWithOliver == requirement.GetField("value").boolValue;
            case "talked-with-oliver-after-abbz":
                return PlayerData.talkedWithOliverAfterAbbz == requirement.GetField("value").boolValue;
            case "generator-fixed":
                return PlayerData.generatorFixed == requirement.GetField("value").boolValue;
            case "repaired":
                return PlayerData.repaired == requirement.GetField("value").boolValue;
            case "item":
                return Inventory.IsInInventory(requirement.GetField("value").stringValue);
            case "atmosphereDestroyed":
                return PlayerData.atmosphereDestroyed == requirement.GetField("value").boolValue;
            case "battery-level":
                return PlayerData.batteriesLevel >= requirement.GetField("value").intValue;
            case "all-spare-parts":
                return PlayerData.AllSparePartsInInv();
            default:
                return false;
        }
    }

    IEnumerator TypeLine(string line)
    {
        
        textComponent.text = "_";
        foreach (char letter in line.ToCharArray())
        {
            // Debug.Log(textComponent.text);
            // Debug.Log("letter: " + letter);
            // Debug.Log("f: " + textComponent.text.Substring(0, textComponent.text.Length - 1) + letter + "_" );
            textComponent.text = textComponent.text.Substring(0, textComponent.text.Length - 1) + letter + "_";
            yield return new WaitForSeconds(typingSpeed);
        }
        textComponent.text = textComponent.text.Substring(0, textComponent.text.Length - 1);
       
    }

}
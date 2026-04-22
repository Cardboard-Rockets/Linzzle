// ===============================
// DialogueScriptTwo.cs
// ===============================
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DialogueLine
{
    [TextArea(3, 10)]
    public string text;

    public string sprite;
}

[System.Serializable]
public class DialogueData
{
    public DialogueLine[] start;
    public DialogueLine[] win;
    public DialogueLine[] lose;
}

public class DialogueScriptTwo : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] PhysicsSystem_Script ps;
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject DialogueBox;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image LensImage;

    [Header("Sprites")]
    [SerializeField] Sprite idleSprite;
    [SerializeField] Sprite hiSprite;
    [SerializeField] Sprite plsSprite;
    [SerializeField] Sprite idkSprite;
    [SerializeField] Sprite thinkSprite;
    [SerializeField] Sprite likeSprite;
    [SerializeField] Sprite coffeeSprite;
    [SerializeField] Sprite blyaSprite;

    [Header("Vars")]
    [SerializeField] bool isStart = true;
    [SerializeField] float startDelay = 2f;
    [SerializeField] bool debug = false;
    [SerializeField] string level_dialogue_file;

    private DialogueData data;

    private DialogueLine[] currentDialogue;
    private int currentIndex = 0;

    private bool isDialogueActive = false;
    private bool hasWinPlayed = false;
    private bool hasLosePlayed = false;

    private float timer = 0;

    void Start()
    {
        LoadDialogue();
        DialogueBox.SetActive(false);
    }

    void Update()
    {
        if (debug) return;

        if (isStart && !isDialogueActive)
        {
            timer += Time.deltaTime;

            if (timer >= startDelay)
            {
                isStart = false;
                StartDialogue(data.start);
            }
        }

        if (ps.isWin && !hasWinPlayed)
        {
            hasWinPlayed = true;
            StartDialogue(data.win);
        }

        if (ps.isLose && !hasLosePlayed)
        {
            hasLosePlayed = true;
            StartDialogue(data.lose);
        }

        if (isDialogueActive && UniversalInput.Down())
        {
            NextLine();
        }
    }

    void LoadDialogue()
    {
        TextAsset json = Resources.Load<TextAsset>(level_dialogue_file);

        if (json == null)
        {
            Debug.LogError("Dialogue JSON not found!");
            return;
        }

        data = JsonUtility.FromJson<DialogueData>(json.text);
    }

    void StartDialogue(DialogueLine[] dialogue)
    {
        if (dialogue == null || dialogue.Length == 0)
            return;

        currentDialogue = dialogue;
        currentIndex = 0;
        isDialogueActive = true;

        DialogueBox.SetActive(true);
        ShowCurrentLine();
    }

    void ShowCurrentLine()
    {
        DialogueLine line = currentDialogue[currentIndex];

        text.text = line.text;
        LensImage.sprite = GetSprite(line.sprite);
    }

    void NextLine()
    {
        currentIndex++;

        if (currentIndex >= currentDialogue.Length)
        {
            EndDialogue();
            return;
        }

        ShowCurrentLine();
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        DialogueBox.SetActive(false);

        if (hasWinPlayed)
            levelManager.nextLevel();
        else if (hasLosePlayed)
            levelManager.restartLevel();
    }

    Sprite GetSprite(string emotion)
    {
        switch (emotion)
        {
            case "Hi": return hiSprite;
            case "Pls": return plsSprite;
            case "Idk": return idkSprite;
            case "Think": return thinkSprite;
            case "Like": return likeSprite;
            case "Coffee": return coffeeSprite;
            case "Blya": return blyaSprite;
            default: return idleSprite;
        }
    }

    public void ResetDialogues()
    {
        isDialogueActive = false;
        hasWinPlayed = false;
        hasLosePlayed = false;
        isStart = true;
        timer = 0;
        DialogueBox.SetActive(false);
    }
}
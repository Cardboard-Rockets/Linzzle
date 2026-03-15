using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    [Header("Connecters")]
    [SerializeField] PhysicsSystem_Script ps;
    [SerializeField] GameObject DialogueBox;
    [SerializeField] Image LensImage; // Добавил отдельное поле для Image компонента

    [Header("DialogueList")]
    [SerializeField] string[] startDi;
    [SerializeField] string[] lose;
    [SerializeField] string[] win;
    [SerializeField] int[] StartDialogueLensSprite;
    [SerializeField] int[] WinDialogueLensSprite;
    [SerializeField] int[] LoseDialogueLensSprite;

    [Header("Vars")]
    [SerializeField] Sprite[] LensSprites;
    [SerializeField] TMPro.TextMeshProUGUI text;
    [SerializeField] GameObject Lens;
    [SerializeField] bool isActive;
    [SerializeField] float timer = 0;
    [SerializeField] bool isStart = true;
    [SerializeField] bool debug = false; // Добавлена debug переменная
    
    // Индексы для каждого типа диалога
    private int currentStartIndex = 0;
    private int currentWinIndex = 0;
    private int currentLoseIndex = 0;
    
    // Флаги для отслеживания состояния диалогов
    private bool isStartDialogueActive = false;
    private bool isWinDialogueActive = false;
    private bool isLoseDialogueActive = false;
    
    // Флаг для блокировки повторного запуска диалогов
    private bool hasWinDialoguePlayed = false;
    private bool hasLoseDialoguePlayed = false;

    void Start()
    {
        DialogueBox.SetActive(false);
        
        // Проверяем наличие необходимых компонентов
        if (LensImage == null)
        {
            LensImage = GetComponent<Image>();
            if (LensImage == null && Lens != null)
            {
                LensImage = Lens.GetComponent<Image>();
            }
        }
        
        // Если включен debug режим, отключаем стартовый диалог
        if (debug)
        {
            isStart = false;
            Debug.Log("DialogueScript: Debug mode enabled - dialogues are disabled");
        }
    }

    void Update()
    {
        // Если включен debug режим, не обрабатываем диалоги
        if (debug)
        {
            return;
        }

        // Таймер для стартового диалога
        if (isStart && !isStartDialogueActive && !isWinDialogueActive && !isLoseDialogueActive)
        {
            timer += Time.deltaTime;
            
            if (timer >= 2f)
            {
                isStart = false;
                StartStartDialogue();
                timer = 0;
            }
        }

        // Проверка победы
        if (ps.isWin && !hasWinDialoguePlayed && !isWinDialogueActive)
        {
            hasWinDialoguePlayed = true;
            StartWinDialogue();
        }

        // Проверка поражения
        if (ps.isLose && !hasLoseDialoguePlayed && !isLoseDialogueActive)
        {
            hasLoseDialoguePlayed = true;
            StartLoseDialogue();
        }

        // Проверка нажатия мыши для продолжения диалога
        if (Input.GetMouseButtonDown(0))
        {
            if (isStartDialogueActive)
            {
                ContinueStartDialogue();
            }
            else if (isWinDialogueActive)
            {
                ContinueWinDialogue();
            }
            else if (isLoseDialogueActive)
            {
                ContinueLoseDialogue();
            }
        }
    }

    void StartStartDialogue()
    {
        // Проверка debug режима перед запуском диалога
        if (debug)
        {
            return;
        }
        
        currentStartIndex = 0;
        isStartDialogueActive = true;
        DialogueBox.SetActive(true);
        UpdateDialogueText(startDi, StartDialogueLensSprite, currentStartIndex);
    }

    void StartWinDialogue()
    {
        // Проверка debug режима перед запуском диалога
        if (debug)
        {
            return;
        }
        
        currentWinIndex = 0;
        isWinDialogueActive = true;
        DialogueBox.SetActive(true);
        UpdateDialogueText(win, WinDialogueLensSprite, currentWinIndex);
    }

    void StartLoseDialogue()
    {
        // Проверка debug режима перед запуском диалога
        if (debug)
        {
            return;
        }
        
        currentLoseIndex = 0;
        isLoseDialogueActive = true;
        DialogueBox.SetActive(true);
        UpdateDialogueText(lose, LoseDialogueLensSprite, currentLoseIndex);
    }

    void ContinueStartDialogue()
    {
        currentStartIndex++;
        
        if (currentStartIndex >= startDi.Length)
        {
            // Завершаем стартовый диалог
            isStartDialogueActive = false;
            DialogueBox.SetActive(false);
            currentStartIndex = 0;
        }
        else
        {
            // Показываем следующее сообщение
            UpdateDialogueText(startDi, StartDialogueLensSprite, currentStartIndex);
        }
    }

    void ContinueWinDialogue()
    {
        currentWinIndex++;
        
        if (currentWinIndex >= win.Length)
        {
            // Завершаем диалог победы
            isWinDialogueActive = false;
            DialogueBox.SetActive(false);
            currentWinIndex = 0;
        }
        else
        {
            // Показываем следующее сообщение
            UpdateDialogueText(win, WinDialogueLensSprite, currentWinIndex);
        }
    }

    void ContinueLoseDialogue()
    {
        currentLoseIndex++;
        
        if (currentLoseIndex >= lose.Length)
        {
            // Завершаем диалог поражения
            isLoseDialogueActive = false;
            DialogueBox.SetActive(false);
            currentLoseIndex = 0;
        }
        else
        {
            // Показываем следующее сообщение
            UpdateDialogueText(lose, LoseDialogueLensSprite, currentLoseIndex);
        }
    }

    void UpdateDialogueText(string[] dialogueArray, int[] spriteIndices, int index)
    {
        if (dialogueArray != null && index < dialogueArray.Length)
        {
            text.text = dialogueArray[index];
            
            // Обновляем спрайт если есть соответствующий индекс
            if (spriteIndices != null && index < spriteIndices.Length)
            {
                int spriteIndex = spriteIndices[index];
                if (spriteIndex >= 0 && spriteIndex < LensSprites.Length && LensImage != null)
                {
                    LensImage.sprite = LensSprites[spriteIndex];
                }
            }
        }
    }

    public void ResetDialogues()
    {
        isStartDialogueActive = false;
        isWinDialogueActive = false;
        isLoseDialogueActive = false;
        hasWinDialoguePlayed = false;
        hasLoseDialoguePlayed = false;
        isStart = true;
        timer = 0;
        currentStartIndex = 0;
        currentWinIndex = 0;
        currentLoseIndex = 0;
        DialogueBox.SetActive(false);
    }
    
    public void SetDebugMode(bool enabled)
    {
        debug = enabled;
        
        if (debug)
        {
            isStartDialogueActive = false;
            isWinDialogueActive = false;
            isLoseDialogueActive = false;
            DialogueBox.SetActive(false);
        }
    }
}
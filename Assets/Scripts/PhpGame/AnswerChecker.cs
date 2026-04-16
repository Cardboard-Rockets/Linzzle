using UnityEngine;
using TMPro;

public class AnswerChecker : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private PhysicsSystem_Script pscr;

    [Header("Answer")]
    [SerializeField] private string correctAnswer;
    [SerializeField] private string PostVar;
    [SerializeField] private string PostAns;

    [Header("StaticAnswer")]
    [SerializeField] bool isStaticAnswer;
    [SerializeField] TextAsset staticCode;


    private PHPTranslatorScript translator;

    void Start()
    {
        translator = new PHPTranslatorScript();
    }

  void Update()
{
    if (inputField != null && inputField.isFocused)
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int pos = inputField.caretPosition;

            inputField.text = inputField.text.Insert(pos, "    "); // 4 пробела
            inputField.caretPosition = pos + 4;
        }
    }
}

    public void CheckAnswer()
{
        if (isStaticAnswer)
        {
            if (staticCode == null)
            {
                Debug.Log("Файл статического кода не подключен!");
            }
            else
            {
                string correctCode = staticCode.text.ToLower().Trim();
                string userCode = inputField.text.ToLower().Trim();

                if (userCode.Equals(correctCode))
                {
                    pscr.isBackGood = true;
                    Debug.Log("Правильно (статическая проверка кода)");
                }
                else
                {
                    pscr.isBackGood = false;
                    Debug.Log("Неправильно (Статическая проверка кода)");
                }
            }
        }
    else{


    translator = new PHPTranslatorScript();

    translator.SetPost(PostVar, PostAns);

    string userCode = inputField.text;

    translator.Execute(userCode);

    string fullOutput = string.Join("\n", translator.output);

    if (fullOutput == correctAnswer)
    {
        pscr.isBackGood = true;
        Debug.Log("Правильно (Динамиеская проверка кода)");
    }
    else
    {
        pscr.isBackGood = false;
        Debug.Log("Неправильно (Динамиеская проверка кода)");
    }
}
}
}
using UnityEngine;
using TMPro;

public class AnswerChecker : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private PhysicsSystem_Script pscr;
    [SerializeField] private string correctAnswer;
    [SerializeField] private string PostVar;
    [SerializeField] private string PostAns;

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
    translator = new PHPTranslatorScript();

    translator.SetPost(PostVar, PostAns);

    string userCode = inputField.text;

    translator.Execute(userCode);

    string fullOutput = string.Join("\n", translator.output);

    if (fullOutput == correctAnswer)
    {
        pscr.isBackGood = true;
        Debug.Log("Правильно");
    }
    else
    {
        pscr.isBackGood = false;
        Debug.Log("Неправильно");

        Debug.Log("Ожидалось: " + correctAnswer);
        Debug.Log("Получено: " + fullOutput);
    }
}
}
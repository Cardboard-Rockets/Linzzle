using UnityEngine;
using UnityEngine.EventSystems;

public class NodeConnector : MonoBehaviour
{
    public static NodeConnector Instance;

    public RectTransform linePrefab;
    public Transform linesParent;

    private NodeScript startNode;
    private Port startPort;
    private ConnectionLine currentLine;

    private void Awake() => Instance = this;

    private void Update()
    {
        if (currentLine == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                CancelConnection();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            CancelConnection();
    }

    public void StartConnection(NodeScript node, Port port)
    {
        if (port == null || port.button == null)
        {
            Debug.LogError("Port или port.button равен null! Проверь инициализацию портов в NodeScript.");
            return;
        }

        RectTransform buttonRect = port.button.GetComponent<RectTransform>();
        if (buttonRect == null)
        {
            Debug.LogError("У кнопки порта нет RectTransform!");
            return;
        }

        if (linePrefab == null)
        {
            Debug.LogError("linePrefab не назначен в NodeConnector!");
            return;
        }

        if (currentLine != null)
            Destroy(currentLine.gameObject);

        startNode = node;
        startPort = port;

        RectTransform lineObj = Instantiate(linePrefab, linesParent);
        currentLine = lineObj.GetComponent<ConnectionLine>();
        currentLine.a = buttonRect;
        currentLine.followMouse = true;
        currentLine.b = null;
    }

    public void EndConnection(NodeScript target)
    {
        if (startNode == null || startNode == target || currentLine == null)
        {
            CancelConnection();
            return;
        }

        currentLine.followMouse = false;
        currentLine.b = target.GetComponent<RectTransform>();

        startNode.ConnectTo(target);

        startNode = null;
        startPort = null;
        currentLine = null;
    }

    public void CancelConnection()
    {
        if (currentLine != null)
            Destroy(currentLine.gameObject);

        startNode = null;
        startPort = null;
        currentLine = null;
    }
}
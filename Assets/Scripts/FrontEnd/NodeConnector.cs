using UnityEngine;

public class NodeConnector : MonoBehaviour
{
    public static NodeConnector Instance;

    public RectTransform linePrefab;
    public Transform linesParent;

    private NodeScript startNode;
    private Port startPort;
    private ConnectionLine currentLine;

    private void Awake() => Instance = this;

    public void StartConnection(NodeScript node, Port port)
    {
        if (currentLine != null)
            Destroy(currentLine.gameObject);

        startNode = node;
        startPort = port;

        RectTransform lineObj = Instantiate(linePrefab, linesParent);
        currentLine = lineObj.GetComponent<ConnectionLine>();

        currentLine.a = port.button.GetComponent<RectTransform>();
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
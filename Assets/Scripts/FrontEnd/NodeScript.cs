using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum NodeType { Input, Checker, If, Error, Answer, Length, db }
public enum PortType { Default, True, False }

[System.Serializable]
public class Port
{
    public Button button;
    public PortType type = PortType.Default;
}

public class NodeScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private bool isDragging;

    public int id;
    public NodeType type;
    public List<NodeScript> exits = new List<NodeScript>();
    public List<Port> outputPorts = new List<Port>();

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        for (int i = 0; i < outputPorts.Count; i++)
        {
            int index = i;
            if (outputPorts[i].button != null)
                outputPorts[i].button.onClick.AddListener(() => StartConnection(index));
        }
    }

    private void Start()
    {
        if (GraphScript.Instance != null)
            GraphScript.Instance.RegisterNode(this);
        else
            Debug.LogError($"GraphScript не найден! Нода: {name}");
    }

    public void Setup(NodeType nodeType)
    {
        type = nodeType;
        if (GraphScript.Instance != null)
            id = GraphScript.Instance.GetNextID();
    }

    public void StartConnection(int portIndex)
    {
        if (portIndex < 0 || portIndex >= outputPorts.Count)
        {
            Debug.LogWarning($"Node {name}: порт с индексом {portIndex} не найден!");
            return;
        }

        Port port = outputPorts[portIndex];
        NodeConnector.Instance.StartConnection(this, port);
    }

    public void ConnectTo(NodeScript target)
    {
        if (!exits.Contains(target))
            exits.Add(target);
    }

    public void OnPointerDown(PointerEventData eventData)
{
    NodeConnector.Instance.EndConnection(this);
}

    public void OnBeginDrag(PointerEventData eventData) => isDragging = true;

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) => isDragging = false;
}
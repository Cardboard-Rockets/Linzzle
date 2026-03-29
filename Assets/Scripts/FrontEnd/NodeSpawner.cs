using UnityEngine;

public class NodeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] nodes;
    [SerializeField] private Transform parent;

    public void SpawnNode(int index)
    {
        if (index < 0 || index >= nodes.Length) return;

        GameObject nodeObject = Instantiate(nodes[index], parent);
        RectTransform rect = nodeObject.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(480, 240);

        NodeScript node = nodeObject.GetComponent<NodeScript>();
        SetupNode(node, index);
    }

    private void SetupNode(NodeScript node, int index)
    {
        switch (index)
        {
            case 0: node.Setup(NodeType.Input); break;
            case 1: node.Setup(NodeType.Checker); break;
            case 2: node.Setup(NodeType.If); break;
            case 3: node.Setup(NodeType.Error); break;
            case 4: node.Setup(NodeType.Answer); break;
        }
    }
}
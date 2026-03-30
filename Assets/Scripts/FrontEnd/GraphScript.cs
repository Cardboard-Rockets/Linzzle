using System.Collections.Generic;
using UnityEngine;

public class GraphScript : MonoBehaviour
{
    public static GraphScript Instance;
    private int idCounter;
    public List<NodeScript> nodes = new List<NodeScript>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public int GetNextID() => ++idCounter;

    public void RegisterNode(NodeScript node)
    {
        nodes.Add(node);
    }
}
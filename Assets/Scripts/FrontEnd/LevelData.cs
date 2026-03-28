using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeConnection
{
    public NodeType fromType; 
    public PortType fromPort;
    public NodeType toType;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Graph/LevelData")]
public class LevelData : ScriptableObject
{
    public List<NodeConnection> correctConnections = new List<NodeConnection>();
}
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeConnection
{
    public NodeType fromType;
    public PortType fromPort;
    public NodeType toType;
}

[System.Serializable]
public class LevelData
{
    public List<NodeConnection> correctConnections;
}
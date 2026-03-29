using System.Linq;
using UnityEngine;

public static class GraphChecker
{
    public static bool CheckGraph(LevelData level)
    {
        foreach (var conn in level.correctConnections)
        {
            var fromNodes = GraphScript.Instance.nodes.Where(n => n.type == conn.fromType).ToList();
            var toNodes = GraphScript.Instance.nodes.Where(n => n.type == conn.toType).ToList();

            bool connectionFound = false;

            foreach (var from in fromNodes)
            {
                foreach (var to in toNodes)
                {
                    if (from.exits.Contains(to))
                    {
                        connectionFound = true;
                        break;
                    }
                }
                if (connectionFound) break;
            }

            if (!connectionFound)
                return false;
        }

        return true;
    }
}
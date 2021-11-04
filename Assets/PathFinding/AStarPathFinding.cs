using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class AStarPathFinding : MonoBehaviour
{
    Coroutine StartFindP;
    PathRequests RequestManager;

    AStarGrid grid;
    private void Awake()
    {
        RequestManager = GetComponent<PathRequests>();
        grid = GetComponent<AStarGrid>();
    }

    public void StartFindPath(Vector3 StartPos,Vector3 EndPos)
    {
        StartFindP = StartCoroutine(FindPath(StartPos,EndPos));
    }

    public void StopFindPath()
    {
        StopCoroutine(StartFindP);
    }

    IEnumerator FindPath(Vector3 StartPos,Vector3 TargetPos)
    {
        Stopwatch SW = new Stopwatch();
        SW.Start();

        Vector3[] Waypoints = new Vector3[0];
        bool PathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(StartPos);
        Node TargetNode = grid.NodeFromWorldPoint(TargetPos);

        if (startNode.Walkable && TargetNode.Walkable)
        {
            AStarHeap<Node> OpenSet = new AStarHeap<Node>(grid.MaxSize);
            HashSet<Node> ClosedSet = new HashSet<Node>();

            OpenSet.Add(startNode);

            while (OpenSet.Count > 0)
            {

                Node CurrentNode = OpenSet.RemoveFirst();
                ClosedSet.Add(CurrentNode);

                if (CurrentNode == TargetNode)
                {
                    SW.Stop();
                    PathSuccess = true;
                    break;
                }

                foreach (Node neighbor in grid.GetNeighbors(CurrentNode))
                {
                    if (!neighbor.Walkable || ClosedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    int newMovementToNeighbor = CurrentNode.GCost + GetDistance(CurrentNode, neighbor);
                    if (newMovementToNeighbor < neighbor.GCost || !OpenSet.Contains(neighbor))
                    {
                        neighbor.GCost = newMovementToNeighbor;
                        neighbor.HCost = GetDistance(neighbor, TargetNode);
                        neighbor.parent = CurrentNode;

                        if (!OpenSet.Contains(neighbor))
                        {
                            OpenSet.Add(neighbor);
                        }
                        else
                        {
                            OpenSet.UpdateItem(neighbor);
                        }
                    }
                }
            }
        }
        yield return null;
        if (PathSuccess)
        {
            Waypoints = RetracePath(startNode, TargetNode);
        }
        RequestManager.FinishedPath(Waypoints, PathSuccess);
    }

    Vector3[] RetracePath(Node Start, Node End)
    {
        List<Node> path = new List<Node>();
        Node currentNode = End;

        while (currentNode != Start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] Waypoints = SimplifyPath(path);
        Array.Reverse(Waypoints);
        return Waypoints;
        

    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> Waypoints = new List<Vector3>();
        Vector2 DirectionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 DirectionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (DirectionNew != DirectionOld)
            {
                Waypoints.Add(path[i].WorldPosition);
            }
            DirectionOld = DirectionNew;
        }
        return Waypoints.ToArray();
    }

    int GetDistance(Node NodeA, Node NodeB)
    {
        int xDist = Mathf.Abs(NodeA.gridX - NodeB.gridX);
        int yDist  = Mathf.Abs(NodeA.gridY - NodeB.gridY);

        if (xDist > yDist)
            return 14 * yDist + 10 * (xDist - yDist);
        return 14 * xDist + 10 * (yDist - xDist);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool Walkable;
    public Vector3 WorldPosition;
    public int gridX;
    public int gridY;

    public int GCost;
    public int HCost;
    public Node parent;
    int _HeapIndex;

    public Node(bool _Walkable, Vector3 _WorldPosition,int _gridX,int _gridY)
    {
        Walkable = _Walkable;
        WorldPosition = _WorldPosition;
        gridX = _gridX;
        gridY = _gridY;
    }
    public int FCost
    {
        get
        {
            return GCost + HCost;
        }
    }
    public int HeapIndex
    {
        get
        {
            return _HeapIndex;
        }
        set
        {
            _HeapIndex = value;
        }
    }

    public int CompareTo(Node NodeToCompare)
    {
        int Compare = FCost.CompareTo(NodeToCompare.FCost);
        if (Compare == 0)
        {
            Compare = HCost.CompareTo(NodeToCompare.HCost);
        }
        return -Compare;
    }
}

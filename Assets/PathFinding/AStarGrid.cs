using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    public bool DrawGridGizmos;
    public Transform player;
    public LayerMask Obsticles;
    public Vector2 GridSize;
    public float NodeRadius;
    Node[,] Nodes;
    Vector3 GridBottomLeft;
    Vector3 WorldPoint;
    float NodeDiameter;
    int GridX, GridY;

    private void Awake()
    {
        NodeDiameter = NodeRadius * 2;
        GridX = Mathf.RoundToInt(GridSize.x / NodeDiameter);
        GridY = Mathf.RoundToInt(GridSize.y / NodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return GridX * GridY;
        }
    }

    void CreateGrid()
    {
        Nodes = new Node[GridX, GridY];

        GridBottomLeft = transform.position - Vector3.right * GridSize.x / 2 - Vector3.up * GridSize.y / 2;

        for (int x=0; x < GridX; x++)
        {
            for (int y=0;y < GridY;y++)
            {
                WorldPoint = GridBottomLeft + Vector3.right * (x*NodeDiameter+NodeRadius) + Vector3.up * (y*NodeDiameter+NodeRadius);
                bool Walkable = !(Physics2D.OverlapBox(WorldPoint,Vector2.one*NodeRadius,0f,Obsticles));
                Nodes[x, y] = new Node(Walkable, WorldPoint,x,y);
            }
        }
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < GridX && checkY >= 0 && checkY < GridY)
                {
                    neighbors.Add(Nodes[checkX, checkY]);
                }
            }
        }
        return neighbors;
    }

    public Node NodeFromWorldPoint(Vector3 WorldPos)
    {
        float PercentX = (WorldPos.x + GridSize.x / 2) / GridSize.x;
        float PercentY = (WorldPos.y + GridSize.y / 2) / GridSize.y;
        PercentX = Mathf.Clamp01(PercentX);
        PercentY = Mathf.Clamp01(PercentY);

        int x = Mathf.RoundToInt((GridX - 1) * PercentX);
        int y = Mathf.RoundToInt((GridY - 1) * PercentY);
        return Nodes[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridSize.x, GridSize.y, 2));
        if(Nodes != null && DrawGridGizmos)
        {
            foreach (Node n in Nodes)
            {
                Gizmos.color = (n.Walkable) ? Color.clear : Color.red;
                Gizmos.DrawCube(n.WorldPosition, Vector3.one * (NodeDiameter-.1f));
            }
        }
    }
}

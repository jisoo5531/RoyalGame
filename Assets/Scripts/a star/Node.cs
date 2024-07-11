using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region ¹ÚÈ¯ÀÇ
public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;

    public int gCost;
    public int hCost;
    public Node parent;

    public int gridX;
    public int gridY;
    public int movementPenalty;
    int heapIndex;

    public Node(bool walkable, Vector3 worldPosition, int _gridX, int _gridY, int _penalty)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = _gridX;
        this.gridY = _gridY;
        this.movementPenalty = _penalty;
    }

    public int fCost => gCost + hCost;

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }

        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }
}
#endregion
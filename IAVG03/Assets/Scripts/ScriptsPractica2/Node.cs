using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int gridX;
    public int gridY;

    public bool walkable;
    public Vector3 position;

    public Node parent;

    public int gCost;
    public int hCost;
    public int FCost { get { return gCost + hCost; } }

    public Node(bool isAWall, Vector3 aPos, int aGridX, int aGridY)
    {
        gridX = aGridX; // posicion X del nodo en el array de nodos
        gridY = aGridY; // posicion Y del nodo en el array de nodos
        walkable = isAWall; // comprueba si el nodo es una pared
        position = aPos; // posicion global del nodo 
    }

}

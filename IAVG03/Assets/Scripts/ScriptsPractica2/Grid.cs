using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform playerPosition;
    public LayerMask wallMask;
    public Vector2 gridWorldSize; // tamaño del grid en la escena
    public float nodeRadius;

    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        // se calcula el tamaño que debe tener cada celda
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter); 
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    // crea una nueva matriz que representa el mapa de nodos
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        // calcula la esquina izquierda del grid
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                // comprueba si la casilla contiene una pared o no
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, wallMask));

                // crea un nuevo nodo y lo inserta en el grid
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    // calcula y devuelve una lista con los nodos vecinos de un nodo del grid en concreto
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue; // es el propio nodo asi que no hace nada

                int checkX = node.gridX + x; // la x del nodo vecino
                int checkY = node.gridY + y; // la y del nodo vecino

                // hace las comprobaciones necesarias para saber si el nodo esta dentro del grid
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    // si es asi lo añade a la lista de nodos vecinos
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    // calcula el nodo al que pertenece una posicion concreta de la escena
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float pX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float pY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        pX = Mathf.Clamp01(pX);
        pY = Mathf.Clamp01(pY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * pX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * pY);

        return grid[x, y];
    }

    public List<Node> finalPath;
    public float distance;

    // para dibujar los nodos
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            Node playerNode = NodeFromWorldPoint(playerPosition.position);
            foreach(Node node in grid)
            {
                if (node.walkable) // si es suelo
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.green;
                }

                if (playerNode == node) // el nodo donde esta el jugador
                {
                    Gizmos.color = Color.cyan;
                }

                if (finalPath != null)
                {
                    if (finalPath.Contains(node)) // los nodos que trazan el hilo
                    {
                        Gizmos.color = Color.red;
                    }
                }

                Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter * distance));
            }
        }
    }
}

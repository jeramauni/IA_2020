using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    Grid grid;
    public Transform player, target;


    // Start is called before the first frame update
    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        FindPath(player.position, target.position);
    }


    // funcion que implementa el A*
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos); // nodo del jugador
        Node targetNode = grid.NodeFromWorldPoint(targetPos); // nodo de la salida

        // listas de nodos para ir formando el camino mas optimo
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        // mierntras existan nodos
        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost)
                {
                    if (openSet[i].hCost < currentNode.hCost)
                        // recorre la lista de nodos y si un nodo tiene un coste menor que el nodo de menor coste anterior
                        // o tiene el mismo coste, entonces es el siguiente nodo al que acceder
                        currentNode = openSet[i];
                }
            }
            // lo quita de la lista abierta y lo añade a la definitiva
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                // calcula el path que debe seguir desde el player hasta su target
                GetFinalPath(startNode, targetNode);
                return;
            }

            foreach(Node neighbour in grid.GetNeighbors(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                // calcula el coste de movimiento de cada uno de los vecinos para elegir la ruta mas eficiente
                int newMoveCostNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMoveCostNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMoveCostNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);

                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        // una vez ha encontrado el nodo lo añade a la lista abierta
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    // calcula la ruta final mas optima
    void GetFinalPath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.finalPath = path;
    }

    // calcula la distancia del nodo A al nodo B
    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }

        return 14 * distX + 10 * (distY - distX);
    }
}

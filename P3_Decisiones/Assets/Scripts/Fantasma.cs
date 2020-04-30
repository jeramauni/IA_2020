using UnityEngine;
using UnityEngine.AI;

public class Fantasma : MonoBehaviour
{
    // Malla de navegacion interna del fantasma
    private NavMeshAgent navMeshAgent;
    // Coste actual 
    private float cost;
    private void Start() {
        // Coger la malla de navegacion del fantasma
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetNavMeshCost(NavMesh.GetAreaFromName("Escenario"), 1000);
    }
    // Getters y Setters
    public void SetNavMeshCost(int areaIdex, float areaCost) {
        navMeshAgent.SetAreaCost(areaIdex, areaCost);
    }
}
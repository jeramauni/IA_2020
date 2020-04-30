using UnityEngine;
using UnityEngine.AI;

public class Fantasma : MonoBehaviour
{
    // Malla de navegacion interna del fantasma
    private NavMeshAgent navMeshAgent;
    private void Start() {
        // Coger la malla de navegacion del fantasma
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    // Getters y Setters
    public void SetNavMeshCost(int areaIdex, float areaCost) {
        navMeshAgent.SetAreaCost(areaIdex, areaCost);
    }
}
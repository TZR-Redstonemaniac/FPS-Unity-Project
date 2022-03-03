using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NavMeshUpdater : MonoBehaviour
{

    [SerializeField] private NavMeshSurface surface;
    
    // Start is called before the first frame update
    void Start()
    {
        surface.BuildNavMesh();
    }
}

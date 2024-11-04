using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NavMeshBuild : MonoBehaviour
{
    public NavMeshSurface navSurface;
    // Start is called before the first frame update
    void Start()
    {
        navSurface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

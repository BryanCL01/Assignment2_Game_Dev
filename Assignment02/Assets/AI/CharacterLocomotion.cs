using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Based on code from TheKiwiCoder: https://www.youtube.com/watch?v=_I8HsTfKep8&t=1s
//Standard assets for animation
//Mixamo for Pistol idle animation
//POLYGON starter pack https://assetstore.unity.com/packages/3d/props/polygon-starter-pack-low-poly-3d-art-by-synty-156819?aid=1011ljjCh&utm_campaign=unity_affiliate&utm_medium=affiliate&utm_source=partnerize-linkmaker
public class CharacterLocomotion : MonoBehaviour
{
    Animator animator;
    public LayerMask whatIsGround, whatIsPlayer;
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    Vector2 input;
    public Transform playerTransform;
    UnityEngine.AI.NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Patrolling();
    }
    public void Patrolling() {
        if (!walkPointSet) {SearchWalkPoint();}
        if (walkPointSet) {
            agent.SetDestination(walkPoint);
            animator.SetFloat("speed", agent.velocity.magnitude);
        }
        Vector3 distanceToWalkPoint =transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 2f)
        {
            walkPointSet = false;
        }
        if (agent.velocity.magnitude  == 0) {
            SearchWalkPoint();
        }
    }
    private void SearchWalkPoint() {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        walkPointSet = true;
        //if (Physics.Raycast(walkPoint, -playerTransform.up, 2f, whatIsGround)) {
        //    
        //
        }
}

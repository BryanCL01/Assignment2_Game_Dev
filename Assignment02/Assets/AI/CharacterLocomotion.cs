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
        StartCoroutine(Patrolling());
    }
    public IEnumerator Patrolling() {
        SearchWalkPoint();
        agent.SetDestination(walkPoint);
        animator.SetFloat("speed", agent.velocity.magnitude);
            
        yield return new WaitForSeconds(2.0f); // Update every second
    }
    private void SearchWalkPoint() {
        float randomZ = Random.Range(-walkPointRange, walkPointRange) + 10;
        float randomX = Random.Range(-walkPointRange, walkPointRange) + 10;
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        walkPointSet = true;
        //if (Physics.Raycast(walkPoint, -playerTransform.up, 2f, whatIsGround)) {
        //    
        //
        }
}

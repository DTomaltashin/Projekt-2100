using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Z_Walk : StateMachineBehaviour
{
    float timer;
    List<Transform> enemyWaypoint = new List<Transform>();
    NavMeshAgent agent;

    Transform player;
    float ChaseDistance = 4f;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //audioSource = GameObject.FindGameObjectWithTag("Enemy").GetComponent<AudioSource>();
        //audioSource.clip = zombiewalk;
        //audioSource.Play();

        AudioManager.instance.Play("Walking");
        timer = 0f;
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = 1f;

        GameObject EnemyWaypoint = GameObject.FindGameObjectWithTag("EnemyWaypoint");
        foreach(Transform t in EnemyWaypoint.transform)
        {
            enemyWaypoint.Add(t);
        }

        agent.SetDestination(enemyWaypoint[Random.Range(0, enemyWaypoint.Count)].position);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(agent.remainingDistance <= agent.stoppingDistance)
            agent.SetDestination(enemyWaypoint[Random.Range(0, enemyWaypoint.Count)].position);


        timer += Time.deltaTime;
        if (timer > 10f)
        {
            animator.SetBool("isPatroling", false);
        }

        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < ChaseDistance)
        {
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
        AudioManager.instance.Stop("Walking");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
    }
}

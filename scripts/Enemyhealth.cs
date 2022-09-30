using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemyhealth : MonoBehaviour
{
    float hp = 100;
    public Animator animator;
    [SerializeField] NavMeshAgent navAgent;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }
    public void TakeDamage(int damageAmount)
    {
        Debug.Log(damageAmount);
        hp -= damageAmount;
        if(hp==0)
        {
            AudioManager.instance.Play("zombieDeath");
            GetComponent<Collider>().enabled = false;
            animator.SetTrigger("isDead");
            navAgent.enabled = false;
            Destroy(gameObject, 2f);
        }
        else
        {
            AudioManager.instance.Play("zombieDamaged");
            animator.SetTrigger("ishit");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Equals("hitbox"))
        {
            Debug.Log("hit");
            TakeDamage(20);
        }
    }
}

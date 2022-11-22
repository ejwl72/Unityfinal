using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class moveable : MonoBehaviour
{
    public Animator anim;

    NavMeshAgent agent;
    [SerializeField]
    Transform target;

    public int zomhp;
    WALL wall;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        wall = GetComponent<WALL>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        agent.SetDestination(target.position);
        if (zomhp <=0)
        {
            Destroy(gameObject);
        }
        anim.SetBool("walk", true);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Lwall")
        {
            InvokeRepeating("Lattackmove", 2.0f, 1.0f);
            anim.SetBool("attack", true);
        }

        if (collision.gameObject.tag == "Rwall")
        {
            InvokeRepeating("Rattackmove", 2.0f, 1.0f);
            anim.SetBool("attack", true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="bullet")
        {
            zomhp -= 1;
        }
    }
    void Rattackmove()
    {
        GameObject.Find("Rtarget2").GetComponent<WALL>().wallHP -= 5;
    }
    void Lattackmove()
    {
        GameObject.Find("Ltarget2").GetComponent<WALL>().wallHP -= 5;
    }
}
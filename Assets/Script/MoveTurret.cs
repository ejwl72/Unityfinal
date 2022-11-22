using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTurret : MonoBehaviour
{
    public GameObject[] gameObjects;
    private int pivot = 0;
    public GameObject bulletFrefeb;
    float delay=1f;
    float timer;
    private int level = 3;

    //public GameObject bulletEffect; //이펙트 추가하기
    //ParticleSystem ps;


    public Transform target;
    //터렛이 쳐다보게될 타겟
    public float range = 15f;
    //터렛이 적을 인식할 수 있는 범위
    public string enemyTag = "Enemy";
    //터렛이 인식할 적의 태그
    public Transform partToRotate;
    //터렛이 중심으로 회전할 회전축
    public float turnSpeed = 10f;
    //터렛의 회전할때 속도
    public Animator t_anim;
    NavMeshAgent agent;
    [SerializeField]
    Transform movetarget;
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        Destroy(gameObject, 10f);

        agent = GetComponent<NavMeshAgent>();
        t_anim = GetComponent<Animator>();
    }

    void UpdateTarget()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag); //Enemy라는 태그를 갖은 것들을 enemies[]배열에 저장
        float shortestDistance = Mathf.Infinity; //가장 짧은 거리를 무한으로 둔다.
        GameObject nearestEnemy = null; //가장 가까운 적을 null 로 둔다

        foreach (GameObject enemy in enemies) //enemy가 enemies의 수만큼 반복
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position); //터렛과 enemy의 거리
            if (distanceToEnemy < shortestDistance) //터렛과 enemy의 거리가 가장 짧은 거리보다 작으면 
            {
                shortestDistance = distanceToEnemy; //가장 짧은 거리는 터렛과enemy 거리가 되고
                nearestEnemy = enemy; //가장 가까운 적은 enemy가됨
            }

        }
        if (nearestEnemy != null && shortestDistance <= range) //만약 가까운 적이 없고, 가장 짧은 거리가 터렛의 범위보다 짧으면
        {
            target = nearestEnemy.transform; //타겟은 다시한번 가장 가까운 놈으로바뀜
        }
        else
        {
            target = null; //아니면 타겟은 없는겨
        }
    }

    void Update()
    {
        agent.SetDestination(movetarget.position);
        timer += Time.deltaTime;
        if (timer >= delay)
        {
            var bullet = Instantiate(bulletFrefeb, transform.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.Fire(transform.forward);
            timer = 0;
        }

        if (target == null)
            return; //타겟없으면 카마있고

        Vector3 dir = target.position - transform.position; //타겟과 터렛의 위치를 뺀 값을 dir로 갖고
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);  //회전 하는 건데 따로설명
        
        //agent.SetDestination(movetarget.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; //범위 만큼 선그어주기. 빨간색으로다가
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
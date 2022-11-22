using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseTower : MonoBehaviour
{
    [SerializeField] private string towerName; // 방워타워의 이름
    [SerializeField] private float range; // 방어타워의 사정거리
    [SerializeField] private int damage; // 방어 타워의 공격력
    [SerializeField] private float rateOfAccuracy; // 정확도(0에 가까울 수록 정확도 높음)
    [SerializeField] private float rateOfFire; // 연사속도(rateOfFire초마다 발사)
    private float currentRateOfFire; // 연사속도 계산(갱신됨)
    [SerializeField] private float viewAngle; // 시야각
    [SerializeField] private float spinSpeed; // 포신 회전 속도
    [SerializeField] private LayerMask layerMask; // 움직이는 대상만 타겟으로 지정(플레이어 혹은 동물)
    [SerializeField] private Transform tf_TopGun; // 포신
    [SerializeField] private ParticleSystem particle_MuzzleFlash; // 총구 섬광
    [SerializeField] private GameObject go_HitEffect_Prefab; // 적중 효과 이펙트

    private RaycastHit hitInfo;
    private Animator anim;
    private AudioSource theAudio;

    private bool isFindTarget = false; // 적 타겟 발견시 True
    private bool isAttack = false; // 정확히 타겟을 향해 포신 회전 완료시 True (총구 방향과 적 방향이 일치할 때)

    private Transform tf_Target; // 현재 설정된 타겟의 트랜스폼

    [SerializeField] private AudioClip sound_Fire;


    void Start()
    {
        theAudio = GetComponent<AudioSource>();
        theAudio.clip = sound_Fire;
        anim = GetComponent<Animator>();
    }

    void FixedUpdate() // 정해진 초마다 (!= 프레임마다. update보다 좀 느리고 정확)
    {
        Spin();
        SearchEnemy();
        LookTarget();
        Attack();
    }
    private void Spin()
    {
        if (!isFindTarget && !isAttack)
        {
            Quaternion _spin = Quaternion.Euler(0f, tf_TopGun.eulerAngles.y + (1f * spinSpeed * Time.deltaTime), 0f);
            tf_TopGun.rotation = _spin;
        }
    }

    private void SearchEnemy()
    {
        Collider[] _target = Physics.OverlapSphere(tf_TopGun.position, range, layerMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform; // 이게 더 빠르다

            if (_targetTf.tag == "Zom")
            {
                Vector3 _direction = (_targetTf.position - tf_TopGun.position).normalized;
                float _angle = Vector3.Angle(_direction, tf_TopGun.forward);

                if (_angle < viewAngle * 0.5f)
                {
                    tf_Target = _targetTf;
                    isFindTarget = true;

                    if (_angle < 5f) // 거의 차이 안나면
                        isAttack = true;
                    else
                        isAttack = false;

                    return;
                }
            }
        }
        // 플레이어 못 찾음
        tf_Target = null;
        isAttack = false;
        isFindTarget = false;
    }

    private void LookTarget()
    {
        if (isFindTarget)
        {
            Vector3 _direction = (tf_Target.position - tf_TopGun.position).normalized;
            Quaternion _lookRotation = Quaternion.LookRotation(_direction);
            Quaternion _rotation = Quaternion.Lerp(tf_TopGun.rotation, _lookRotation, 0.2f);
            tf_TopGun.rotation = _rotation;
        }
    }

    private void Attack()
    {
        if (isAttack)
        {
            currentRateOfFire += Time.deltaTime;
            if (currentRateOfFire >= rateOfFire)
            {
                currentRateOfFire = 0;
                anim.SetTrigger("Fire");
                theAudio.Play();
                particle_MuzzleFlash.Play();

                if (Physics.Raycast(tf_TopGun.position,
                                    tf_TopGun.forward + new Vector3(Random.Range(-1, 1f) * rateOfAccuracy, Random.Range(-1, 1f) * rateOfAccuracy, 0f),
                                    out hitInfo,
                                    range,
                                    layerMask))
                {
                    GameObject _HitEffect = Instantiate(go_HitEffect_Prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(_HitEffect, 1f);

                    if (hitInfo.transform.tag == "Zom")
                    {
                        //hitInfo.transform.GetComponent<PlayerController>().DecreaseHP(damage);
                    }
                }
            }
        }
    }
}
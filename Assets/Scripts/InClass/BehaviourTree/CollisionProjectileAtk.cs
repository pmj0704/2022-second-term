using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionProjectileAtk : MonoBehaviour
{
    public float projectileSpd;                     //발사체 속도
    public GameObject ObjProjectileStartEffect;     //발사체 발사 이펙트
    public GameObject ObjProjetileHitEffect;        //타겟 충돌 이펙트

    public AudioClip projectileStartClip;           //발사 소리
    public AudioClip projectileHitClip;             //충돌 소리

    private bool getFlagProjectileCollid;           //내부적 충돌 확인 변수
    private Rigidbody rb;                           //발사체 이동은 rigidbody Move로

    [HideInInspector]
    public AtkBehaviour atkBehaviour;               //행동 기본 정보

    [HideInInspector]
    public GameObject projectileParents;            //발사체 부모 객체

    [HideInInspector]
    public GameObject target;                       //타겟 객체

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();             //캐싱 -> 발사체 이동

        if (projectileParents != null)
        {
            Collider collidProjectile = GetComponent<Collider>();                               //발사체 충돌체
            Collider[] collidParents = projectileParents.GetComponentsInChildren<Collider>();   //발사체의 부모의 충돌체들
            foreach (Collider collider in collidParents)                                         //자식 충돌체 순회
            {
                Physics.IgnoreCollision(collidProjectile, collider);                            //발사체와 자식들 충돌 시 무시, 캐릭터가 안맞도록
            }
        }

        if(ObjProjectileStartEffect != null) //발사 시 발사체 이펙트가 존재 시
        {
            var projectileStartEffect = Instantiate(this.ObjProjectileStartEffect, transform.position, Quaternion.identity);

            projectileStartEffect.transform.forward = gameObject.transform.forward; //발사체와 이펙트 방향 동일

            ParticleSystem particleSystem = projectileStartEffect.GetComponent<ParticleSystem>();
           
            //발사체 발사 이펙트의 파티클이 존재 한다면
            if(particleSystem != null)
            {
                Destroy(projectileStartEffect, particleSystem.main.duration); //연출 시간 이후 삭제
            }
            else
            {
                ParticleSystem particleSystemChild = projectileStartEffect.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(projectileStartEffect, particleSystemChild.main.duration); //연출 시간 지연
            }

            if(projectileStartClip != null && GetComponent<AudioSource>())
            {
                GetComponent<AudioSource>().PlayOneShot(projectileStartClip); //오디오 소스
            }

            if(target != null)
            {
                Vector3 vecProjectile = target.transform.position;
                vecProjectile.y += 1.5f;
                transform.LookAt(vecProjectile);
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if(projectileSpd > 0 && rb != null)
        {
            rb.position += (transform.forward) * (projectileSpd * Time.deltaTime);
        }
    }

    protected virtual void OnProgectileStartCollision(Collision collision) //발사체 충돌 시
    {
        if (getFlagProjectileCollid) return; //전에 충돌 한 적이 있다면

        Collider projectileCollider = GetComponent<Collider>(); //발사체 충돌체 가져오기
        projectileCollider.enabled = false;
        getFlagProjectileCollid = true;

        if (projectileHitClip != null && GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().PlayOneShot(projectileHitClip);
        }

        projectileSpd = 0;
        rb.isKinematic = true; //Rigidbody에 물리연산을 하지 않는다. onCollisionenter가 호출 안됨

        //충돌 지점 정보 받아 오기
        ContactPoint contactPoint = collision.contacts[0]; //젤 먼저 받아 온 애 (충돌 면 정보)
        Quaternion rotationContact = Quaternion.FromToRotation(Vector3.up, contactPoint.normal); //빌보드 형식 Bill - Board, 객체가 카메라를 바라보는 형식
        Vector3 vecContact = contactPoint.point; //구해진 ContactPoint를 위치 변수로 만든다.

        if(ObjProjetileHitEffect != null)
        {
            var projectileHitEffect = Instantiate(ObjProjetileHitEffect, vecContact, rotationContact) as GameObject;
            //컨텍트 포인트에 이펙트를

            ParticleSystem particleSystem = projectileHitEffect.GetComponent<ParticleSystem>();

            if (particleSystem == null)
            {
                ParticleSystem particleSystemChild = projectileHitEffect.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(projectileHitEffect, particleSystemChild.main.duration);
            }
            else
            {
                Destroy(projectileHitEffect, particleSystem.main.duration);
            }

            //충돌 했으니 데미지
            IDmgAble iDmgAble = collision.gameObject.GetComponent<IDmgAble>(); //데미지를 델리게이트를 피해량을 가져온다.
            
            if(iDmgAble != null) //피해량이 확인 되면
            {
                iDmgAble.setDmg(atkBehaviour?.atkDmg ?? 0, null); //공격력을 가져오거나 없으면 0을 피해량 데미지
                //atkDmg가 없으면 0, 있으면 값
                // A?.[하위] / A [하위] 불러옴 / 근데 존재 하지 않으면 A = null / 근데 뒤에 ?? 0이 붙으면 -> 앞??뒤 앞이 null이면 뒤
            }

            StartCoroutine(DestroyParticle(0.0f)); //시간이 지나면 파티클 제거
        }
    }

    IEnumerator DestroyParticle(float waittingTime)
    {
        if(transform.childCount > 0 && waittingTime != 0)   //파티클 제거, 주어진 시간에 점점 사라지게 만듬
        {
            List<Transform> destroyParticleTopChilds = new List<Transform>();

            foreach(Transform t in transform.GetChild(0).transform)
            {
                destroyParticleTopChilds.Add(t);
            }    

            while(transform.GetChild(0).localScale.x > 0)
            {
                yield return new WaitForSeconds(0.01f);
                transform.GetChild(0).localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                for(int i =0; i < destroyParticleTopChilds.Count; i++)
                {
                    destroyParticleTopChilds[i].localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                }
            }
        }
        yield return new WaitForSeconds(waittingTime);
        Destroy(gameObject);
    }
}

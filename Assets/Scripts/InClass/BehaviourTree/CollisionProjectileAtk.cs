using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionProjectileAtk : MonoBehaviour
{
    public float projectileSpd;                     //�߻�ü �ӵ�
    public GameObject ObjProjectileStartEffect;     //�߻�ü �߻� ����Ʈ
    public GameObject ObjProjetileHitEffect;        //Ÿ�� �浹 ����Ʈ

    public AudioClip projectileStartClip;           //�߻� �Ҹ�
    public AudioClip projectileHitClip;             //�浹 �Ҹ�

    private bool getFlagProjectileCollid;           //������ �浹 Ȯ�� ����
    private Rigidbody rb;                           //�߻�ü �̵��� rigidbody Move��

    [HideInInspector]
    public AtkBehaviour atkBehaviour;               //�ൿ �⺻ ����

    [HideInInspector]
    public GameObject projectileParents;            //�߻�ü �θ� ��ü

    [HideInInspector]
    public GameObject target;                       //Ÿ�� ��ü

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();             //ĳ�� -> �߻�ü �̵�

        if (projectileParents != null)
        {
            Collider collidProjectile = GetComponent<Collider>();                               //�߻�ü �浹ü
            Collider[] collidParents = projectileParents.GetComponentsInChildren<Collider>();   //�߻�ü�� �θ��� �浹ü��
            foreach (Collider collider in collidParents)                                         //�ڽ� �浹ü ��ȸ
            {
                Physics.IgnoreCollision(collidProjectile, collider);                            //�߻�ü�� �ڽĵ� �浹 �� ����, ĳ���Ͱ� �ȸµ���
            }
        }

        if(ObjProjectileStartEffect != null) //�߻� �� �߻�ü ����Ʈ�� ���� ��
        {
            var projectileStartEffect = Instantiate(this.ObjProjectileStartEffect, transform.position, Quaternion.identity);

            projectileStartEffect.transform.forward = gameObject.transform.forward; //�߻�ü�� ����Ʈ ���� ����

            ParticleSystem particleSystem = projectileStartEffect.GetComponent<ParticleSystem>();
           
            //�߻�ü �߻� ����Ʈ�� ��ƼŬ�� ���� �Ѵٸ�
            if(particleSystem != null)
            {
                Destroy(projectileStartEffect, particleSystem.main.duration); //���� �ð� ���� ����
            }
            else
            {
                ParticleSystem particleSystemChild = projectileStartEffect.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(projectileStartEffect, particleSystemChild.main.duration); //���� �ð� ����
            }

            if(projectileStartClip != null && GetComponent<AudioSource>())
            {
                GetComponent<AudioSource>().PlayOneShot(projectileStartClip); //����� �ҽ�
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

    protected virtual void OnProgectileStartCollision(Collision collision) //�߻�ü �浹 ��
    {
        if (getFlagProjectileCollid) return; //���� �浹 �� ���� �ִٸ�

        Collider projectileCollider = GetComponent<Collider>(); //�߻�ü �浹ü ��������
        projectileCollider.enabled = false;
        getFlagProjectileCollid = true;

        if (projectileHitClip != null && GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().PlayOneShot(projectileHitClip);
        }

        projectileSpd = 0;
        rb.isKinematic = true; //Rigidbody�� ���������� ���� �ʴ´�. onCollisionenter�� ȣ�� �ȵ�

        //�浹 ���� ���� �޾� ����
        ContactPoint contactPoint = collision.contacts[0]; //�� ���� �޾� �� �� (�浹 �� ����)
        Quaternion rotationContact = Quaternion.FromToRotation(Vector3.up, contactPoint.normal); //������ ���� Bill - Board, ��ü�� ī�޶� �ٶ󺸴� ����
        Vector3 vecContact = contactPoint.point; //������ ContactPoint�� ��ġ ������ �����.

        if(ObjProjetileHitEffect != null)
        {
            var projectileHitEffect = Instantiate(ObjProjetileHitEffect, vecContact, rotationContact) as GameObject;
            //����Ʈ ����Ʈ�� ����Ʈ��

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

            //�浹 ������ ������
            IDmgAble iDmgAble = collision.gameObject.GetComponent<IDmgAble>(); //�������� ��������Ʈ�� ���ط��� �����´�.
            
            if(iDmgAble != null) //���ط��� Ȯ�� �Ǹ�
            {
                iDmgAble.setDmg(atkBehaviour?.atkDmg ?? 0, null); //���ݷ��� �������ų� ������ 0�� ���ط� ������
                //atkDmg�� ������ 0, ������ ��
                // A?.[����] / A [����] �ҷ��� / �ٵ� ���� ���� ������ A = null / �ٵ� �ڿ� ?? 0�� ������ -> ��??�� ���� null�̸� ��
            }

            StartCoroutine(DestroyParticle(0.0f)); //�ð��� ������ ��ƼŬ ����
        }
    }

    IEnumerator DestroyParticle(float waittingTime)
    {
        if(transform.childCount > 0 && waittingTime != 0)   //��ƼŬ ����, �־��� �ð��� ���� ������� ����
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTypeAtkBehaviour : AtkBehaviour
{
    public override void callAtkMotion(GameObject target = null, Transform posAtkStart = null)
    {
        if(target == null && posAtkStart != null)
        {
            return;
        }

        Vector3 vecProjectile = posAtkStart?.position ?? transform.position;
        //공격 위치를 넣었는데 null이면, 공격 행동을 컴포턴트로 가지고 있는 객체의 위치를 가져온다.
 
        if(atkEffectPrefab != null)
        {
            GameObject objProjectile = GameObject.Instantiate<GameObject>(atkEffectPrefab, vecProjectile, Quaternion.identity);

            objProjectile.transform.forward = transform.forward;

            CollisionProjectileAtk projectile = objProjectile.GetComponent<CollisionProjectileAtk>();
        
            if(projectile != null)
            {
                projectile.projectileParents = this.gameObject;
                projectile.target = target;
                projectile.atkBehaviour = this;
            }
        }
        nowAtkCoolTime = 0.0f;
    }
}

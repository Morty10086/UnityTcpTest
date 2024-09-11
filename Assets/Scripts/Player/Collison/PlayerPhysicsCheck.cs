using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsCheck : PhysicsCheck
{
    [Header("ÎïÀí²ÄÖÊ")]
    public PhysicMaterial normalMaterial;
    public PhysicMaterial smoothMaterial;

    public bool isCollisionEnmey;
    public LayerMask collisionEnemyLayer;
    protected override void Update()
    {
        base.Update();
        this.CheckGroundOrAir();
    }
    private void CheckGroundOrAir()
    {
        capsuleCollider.sharedMaterial = this.isGround ? normalMaterial : smoothMaterial;
        capsuleCollider.sharedMaterial = this.isWall ? smoothMaterial:normalMaterial;
    }

    protected override void Check()
    {
        base.Check();
        Collider[] cls3 = Physics.OverlapBox(transform.position + (Vector3)wallOffset,
           cubeSide,
           Quaternion.identity, collisionEnemyLayer);
        if (cls3.Length > 0)
        {
            isCollisionEnmey = true;
        }
        else
        {
            isCollisionEnmey = false;
        }
      
    }
}

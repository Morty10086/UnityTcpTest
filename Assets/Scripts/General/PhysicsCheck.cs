using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    
    [HideInInspector]
    public CapsuleCollider capsuleCollider;
    [HideInInspector]
    public Rigidbody rb;


    [Header("地面检测相关")]
    public bool isGround;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public Vector2 groundOffset;
    [HideInInspector]
    public float groundOffsetX;
    

    [Header("撞墙检测相关")]
    public bool isWall;
    public Vector2 wallOffset;
    public LayerMask wallLayer;
    [HideInInspector]
    public float wallOffsetX;
    public Vector3 cubeSide;


    protected virtual void Awake()
    {
        rb=GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        groundOffsetX=Mathf.Abs(groundOffset.x);
        wallOffsetX= Mathf.Abs(wallOffset.x);
    }

    protected virtual void Update()
    {
        CalculateOffset();
        Check();
    }

   protected virtual void CalculateOffset()
    {
        groundOffset = new Vector2(groundOffsetX * this.transform.localScale.x, groundOffset.y);
        wallOffset = new Vector2(wallOffsetX * this.transform.localScale.x, wallOffset.y);
    }
    protected virtual void Check()
    {        
        #region 地面检测

        Collider[] cls = Physics.OverlapSphere(transform.position + (Vector3)groundOffset, groundCheckRadius, groundLayer);
        if(cls.Length>0)
            isGround = true;
        else
            isGround = false;
        foreach(Collider c in cls)
        {
            if(c.gameObject.CompareTag("MovePlatform"))
            {
                if(c.gameObject.GetComponent<PlatformH>()!=null)
                {
                    this.gameObject.transform.position += c.gameObject.GetComponent<PlatformH>().displacement;
                }
                    
                if (c.gameObject.GetComponent<PlatformV>() != null)
                {
                    this.gameObject.transform.position += c.gameObject.GetComponent<PlatformV>().displacement;
                }
                    
            }            
        }
        
        #endregion

        #region 撞墙检测
        Collider[] cls2 = Physics.OverlapBox(transform.position + (Vector3)wallOffset,
            cubeSide,
            Quaternion.identity, wallLayer);
        if (cls2.Length>0)
            isWall = true;
        else
            isWall = false;
        #endregion
    }
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)groundOffset, groundCheckRadius);
        Gizmos.DrawWireCube(transform.position + (Vector3)wallOffset,cubeSide*2);
    }
}

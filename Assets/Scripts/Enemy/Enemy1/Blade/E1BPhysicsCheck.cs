using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class E1BPhysicsCheck:MonoBehaviour
{

    private E1B e1b;

    [Header("¼ì²âÍæ¼Ò")]
    public bool isFindPlayer;
    public Vector3 findPlayerSide;
    public float findPlayerDistance;
    public LayerMask playerLayer;
    public Vector2 findPlayerOffset;
    private float findPlayerOffsetX;
    public RaycastHit playerHit;
    public Transform playerTrans;
    [Header("¼ì²â¹¥»÷·¶Î§")]
    public bool isCanAttack;
    public float attackRadius;
    public LayerMask attackLayer;
    public Vector2 attackOffset;
    private float attackOffsetX;

    private void Awake()
    {
        findPlayerOffsetX= Mathf.Abs(findPlayerOffset.x);
        attackOffsetX= Mathf.Abs(attackOffset.x);
        e1b = GetComponent<E1B>();
    }

    private void Update()
    {
        CalculateOffset();
        Check();
    }
    private void CalculateOffset()
    {
        attackOffset = new Vector2(attackOffsetX * this.transform.localScale.x, attackOffset.y);
        findPlayerOffset=new Vector2(findPlayerOffsetX*this.transform.localScale.x, findPlayerOffset.y);
    }

    private void Check()
    {

        #region ¼ì²âÍæ¼Ò
        e1b.isFindPlayer = Physics.BoxCast(this.transform.position + (Vector3)findPlayerOffset,
            findPlayerSide,
            transform.localScale.x*transform.right,
            out playerHit,
            Quaternion.identity,
            findPlayerDistance,
            playerLayer);
        if(e1b.isFindPlayer)
        {
            e1b.playerTrans = playerHit.transform;
        }
        #endregion

        #region ¼ì²âÊÇ·ñÔÚ¹¥»÷·¶Î§ÄÚ
        Collider[] cls = Physics.OverlapSphere(this.transform.position + (Vector3)attackOffset, attackRadius, attackLayer);
        if(cls.Length>0)
            e1b.isCanAttack = true;
        else
            e1b.isCanAttack = false;
        #endregion
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.DrawWireCube(this.transform.position + (Vector3)findPlayerOffset+new Vector3(findPlayerDistance*transform.localScale.x,0,0), findPlayerSide*2);
        Gizmos.DrawWireSphere(this.transform.position + (Vector3)attackOffset, attackRadius);
    }
}

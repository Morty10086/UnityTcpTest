using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1GPhysicsCheck :MonoBehaviour
{
    private E1G e1g;

    [Header("¼ì²âÍæ¼Ò")]
    public bool isFindPlayer;
    public Vector3 findPlayerSide;
    public float findPlayerDistance;
    public LayerMask playerLayer;
    public Vector2 findPlayerOffset;
    private float findPlayerOffsetX;
    public RaycastHit playerHit;
    public Transform playerTrans;


    private void Awake()
    {
        e1g = GetComponent<E1G>();
        findPlayerOffsetX = Mathf.Abs(findPlayerOffset.x);
    }

    private void Update()
    {
        CalculateOffset();
        Check();
    }
    private void CalculateOffset()
    {       
        findPlayerOffset = new Vector2(findPlayerOffsetX * this.transform.localScale.x, findPlayerOffset.y);
    }

    private void Check()
    {
        
        #region ¼ì²âÍæ¼Ò
        e1g.isFindPlayer = Physics.BoxCast(this.transform.position + (Vector3)findPlayerOffset,
            findPlayerSide,
            transform.localScale.x * transform.right,
            out playerHit,
            Quaternion.identity,
            findPlayerDistance,
            playerLayer);
        if (e1g.isFindPlayer)
        {
            e1g.playerTrans = playerHit.transform;
        }


        #endregion
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(this.transform.position + (Vector3)findPlayerOffset + new Vector3(findPlayerDistance * transform.localScale.x, 0, 0), findPlayerSide * 2);
    }
}

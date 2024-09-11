using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1GShootBullet : MonoBehaviour
{
    public float drawBulletR;
    public Vector2 bulletOffset;
    private float bulletOffsetX;
    private void Awake()
    {
        bulletOffsetX=Mathf.Abs(bulletOffset.x);
    }
    public void GenerateBullet()
    {
        print("zidan");
        GameObject bulletObj = PoolManager.Instance.GetObj("Bullet/EnemyBullet/E1GBullet");
        bulletOffset = new Vector2(this.transform.parent.localScale.x * bulletOffsetX, bulletOffset.y);
        bulletObj.transform.position= this.transform.position+(Vector3)bulletOffset;
        bulletObj.transform.localScale = new Vector3(this.transform.parent.localScale.x, 1, 1);
        bulletObj.GetComponent<E1GBullet>().flyDir = (int)this.transform.parent.localScale.x;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position + (Vector3)bulletOffset, drawBulletR);
    }
}

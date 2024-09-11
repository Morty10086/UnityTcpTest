using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STShootBullet : MonoBehaviour
{
    [Header("“Ù–ß")]
    public AudioClip moveClip;
    public AudioClip attackClip;
    public AudioClip landClip;
    public AudioClip bulletClip;
    public AudioClip rocketClip;
    public AudioClip chargeClip;
    public AudioClip laserClip;
    public AudioClip deadClip;
    
    public float drawBulletR;
    public Vector2 bulletOffset;
    private float bulletOffsetX;
    private void Awake()
    {
        bulletOffsetX=Mathf.Abs(bulletOffset.x);
    }

    public void GenerateBullet()
    {
        GameObject bulletObj = PoolManager.Instance.GetObj("Bullet/EnemyBullet/STRocketSmall");
        bulletOffset = new Vector2(-this.transform.parent.localScale.x * bulletOffsetX, bulletOffset.y);
        bulletObj.transform.position = this.transform.position + (Vector3)bulletOffset;
        bulletObj.GetComponent<STBullet>().flyDir = -this.transform.parent.localScale.x;
        bulletObj.transform.right = new Vector3(-this.transform.parent.localScale.x, 0, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position + (Vector3)bulletOffset, drawBulletR);
    }

    public void StopSound()
    {
        AudioMgr.Instance.StopSound();
    }
    public void PlayMoveSound()
    {
        AudioMgr.Instance.PlaySound(moveClip,true,false,0.1f);
    }

    public void PlayAttackSound()
    {
        AudioMgr.Instance.PlaySound(attackClip,false,false,0.1f);
    }

    public void PlayLandSound()
    {
        AudioMgr.Instance.PlaySound(landClip, false, false, 0.2f);
    }
    public void PlayBulletSound()
    {
        AudioMgr.Instance.PlaySound(bulletClip, false, false, 0.2f);
    }
    public void PlayRocketSound()
    {
        AudioMgr.Instance.PlaySound(rocketClip, false, false, 0.2f);
    }

    public void PlayChargeSound()
    {
        AudioMgr.Instance.PlaySound(chargeClip,false,true,0.5f);
    }
    public void PlayLaserSound()
    {
        AudioMgr.Instance.PlaySound(laserClip);
    }
    public void PlayDeadSound()         
    {
        AudioMgr.Instance.PlaySound(deadClip);
    }
}

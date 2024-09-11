using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoToNextAttack : MonoBehaviour
{
    public PlayerController pController;
    public float drawBulletR;
    public Vector2 bulletOffset;
    public float bulletOffsetX;

    public float drawBulletAirR;
    public Vector2 bulletAirOffset;
    public float bulletAirOffsetX;
    private void Awake()
    {
        bulletOffsetX=Mathf.Abs(bulletOffset.x);
        bulletAirOffsetX = Mathf.Abs(bulletAirOffset.x);
    }
    public void GoToNextAttack()
    {
        this.GetComponent<Animator>().SetInteger("comboCounter", pController.comboCunter);
    }

    public void GetVelocity()
    {
        pController.rb.velocity = new Vector2( pController.jumpShootForceX * -pController.transform.localScale.x, pController.jumpShootForceY);
    }

    public void GenerateBullet()
    {
        GameObject bulletObj = PoolManager.Instance.GetObj("Bullet/PlayerBullet");
        bulletOffset = new Vector2(this.transform.parent.localScale.x * bulletOffsetX, bulletOffset.y);
        bulletObj.transform.position = this.transform.position + (Vector3)bulletOffset;
        bulletObj.transform.rotation = Quaternion.Euler(0, 0, 0);
        bulletObj.transform.localScale = new Vector3(this.transform.parent.localScale.x, 1, 1);
        bulletObj.GetComponent<PlayerBullet>().flyDir = (int)this.transform.parent.localScale.x;
    }
    public void GenerateBulletOnAir()
    {
        GameObject bulletObj = PoolManager.Instance.GetObj("Bullet/PlayerBullet");
        bulletAirOffset = new Vector2(this.transform.parent.localScale.x * bulletAirOffsetX, bulletAirOffset.y);
        bulletObj.transform.position = this.transform.position + (Vector3)bulletAirOffset;
        bulletObj.transform.localScale = new Vector3(this.transform.parent.localScale.x, 1, 1);
        if (this.transform.parent.localScale.x>0)
        {
            bulletObj.transform.rotation = Quaternion.Euler(0, 0, -40);
        }
        else
        {
            bulletObj.transform.rotation = Quaternion.Euler(0, 0, -140);
        }
        
        bulletObj.GetComponent<PlayerBullet>().flyDir = 1;
    }

    public void PlayMoveSound()
    {
        if (pController.isAttack || pController.isDash || pController.isHurt || pController.isShoot || !pController.phyCheck.isGround)
            return;
        AudioMgr.Instance.PlaySoundNew(AudioID.playerMove);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position + (Vector3)bulletOffset, drawBulletR);
        Gizmos.DrawWireSphere(this.transform.position + (Vector3)bulletAirOffset, drawBulletAirR);
    }
}

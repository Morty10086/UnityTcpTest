using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSprite : MonoBehaviour
{

    public float drawBulletR;
    public Vector2 bulletOffset;
    private float bulletOffsetX;
    private Vector2 targetPos;
    private Vector2 startPos;
    [Header("“Ù–ß")]
    public AudioClip shootClip;
    public AudioClip deadClip;
    private AudioSource audioSource;
    private void Awake()
    {
        bulletOffsetX = Mathf.Abs(bulletOffset.x);
        audioSource=this.GetComponentInParent<AudioSource>();
    }

    public void PlayShootSound()
    {
        audioSource.clip = shootClip;
        audioSource.Play();
    }
    public void PlayDeadSound()
    {
        audioSource.clip=deadClip;
        audioSource.Play();
    }
    public void GenerateBullet()
    {
        targetPos=this.GetComponentInParent<Drone>(true).playerTrans.position;     
        GameObject bulletObj = PoolManager.Instance.GetObj("Bullet/EnemyBullet/DroneBullet");
        bulletOffset = new Vector2(-this.transform.parent.localScale.x * bulletOffsetX, bulletOffset.y);
        bulletObj.transform.position = this.transform.position + (Vector3)bulletOffset;
        startPos = this.transform.position + (Vector3)bulletOffset;
        bulletObj.transform.right = targetPos - startPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position + (Vector3)bulletOffset, drawBulletR);
    }
}

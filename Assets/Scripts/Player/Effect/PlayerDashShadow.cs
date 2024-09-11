using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashShadow : MonoBehaviour
{
    private Transform player;
    private Transform playerSprite;
    private SpriteRenderer playerSpriteRender;
    private SpriteRenderer shadowSpriteRender;

    public float offsetX;
    public float offsetY;

    private Color color;
    [Header("ʱ����Ʋ���")]
    public float activeTime;  //��ʾʱ��
    public float activeStart; //��ʼ��ʾ��ʱ��

    [Header("��͸���ȿ���")]
    public float alpha;
    public float alphaStart; //��ʼֵ
    public float alphaAtten;  //͸����˥��
    private void OnEnable()
    {
        player = GameObject.Find("Player").transform;
        playerSprite = player.Find("PlayerSprite");
        shadowSpriteRender= GetComponent<SpriteRenderer>();
        playerSpriteRender = playerSprite.GetComponent<SpriteRenderer>();

        alpha = alphaStart;
        shadowSpriteRender.sprite = playerSpriteRender.sprite;

        this.transform.position = new Vector3(player.position.x+offsetX*-player.transform.localScale.x,player.position.y+offsetY,player.position.z);
        this.transform.rotation = player.rotation;
        this.transform.localScale= player.localScale;

        activeStart=Time.time;
    }
    void Update()
    {
        alpha -= alphaAtten;
        color = new Color(0.5f, 0.5f, 1f,alpha);

        shadowSpriteRender.color = color;
        if (Time.time >= activeStart + activeTime)
        {
            PoolManager.Instance.PushObj("Shadow/DashShadow", this.gameObject);
        }
    }
}

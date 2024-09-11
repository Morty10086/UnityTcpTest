using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    private Transform player;
    private Transform playerSprite;
    private SpriteRenderer playerSpriteRender;
    private SpriteRenderer shadowSpriteRender;

    private void OnEnable()
    {
        player = GameObject.Find("Player").transform;
        playerSprite = player.Find("PlayerSprite");
        shadowSpriteRender = this.GetComponent<SpriteRenderer>();
        playerSpriteRender = playerSprite.GetComponent<SpriteRenderer>();

        shadowSpriteRender.sprite = playerSpriteRender.sprite;
    }
    
    void Update()
    {
        shadowSpriteRender.sprite = playerSpriteRender.sprite;
    }
}

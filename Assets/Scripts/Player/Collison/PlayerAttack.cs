using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Attack
{
    public float timeGet;
    public bool isPlayer1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isPlayer1)
        {
            if (collision.gameObject.CompareTag("Player2"))
            {
                collision.GetComponentInParent<Character>()?.TakeDamage(this);
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Player1"))
            {
                collision.GetComponentInParent<Character>()?.TakeDamage(this);
            }
        }
            
    }
}

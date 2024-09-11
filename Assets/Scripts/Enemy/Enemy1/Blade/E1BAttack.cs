using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1BAttack : Attack
{
    private E1B e1b;
    private void Awake()
    {
        e1b = GetComponentInParent<E1B>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")/* && !e1b.isHurt*/)
        {
            collision.GetComponentInParent<Character>()?.TakeDamage(this);
        }
        
    }
}

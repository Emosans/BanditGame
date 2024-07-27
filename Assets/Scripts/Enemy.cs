using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //variables
    public int maxHealth = 100;
    int currentDamage;
    public float attackRange=0.5f;

    //components
    public Transform player;
    private bool isFlipped = false;
    public Transform enemyAttackPosition;
    public Animator animator;
    public LayerMask playerMask;

    // Start is called before the first frame update
    void Start()
    {
        currentDamage = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentDamage -= damage;

        //play hurt anim
        animator.SetTrigger("Hurt");

        if(currentDamage <= 0)
        {
            Debug.Log("Enemy Died!");

            animator.SetBool("isDead",true);

            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;
        }
    }


    public void LookAtPlayer()
    {
        Vector2 flipped = transform.localScale;

        if(transform.position.x > player.position.x && isFlipped) 
        { 
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }

        if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }


    public void Attack()
    {
        //play attack anim
        //check distance between enemy and player
        //if within attackRange play attack anim (behaviour in idle to attack transition)
        //reset the attack trigger(same as above)
        //animator.SetTrigger("Attack");

        Collider2D playerHit = Physics2D.OverlapCircle(enemyAttackPosition.position, attackRange, playerMask);

        if(playerHit != null)
        {
            player.GetComponent<PlayerMovement>().TakeDamage(20);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (enemyAttackPosition == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(enemyAttackPosition.position, attackRange);
    }
}

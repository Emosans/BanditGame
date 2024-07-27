using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //variables
    [SerializeField]
    private float moveSpeed = 7f;
    private float movementX;
    private bool isGrounded = true;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attakcRange = 0.5f;
    private float attackRate = 2f;
    float nextAttackTime = 0f;
    public int maxHealth=100;
    [SerializeField]
    int currentHealth;

    //components
    private Animator animator;
    private SpriteRenderer sr;
    private Rigidbody2D myBody;

    //strings
    private string RUN_ANIM = "run";
    private string ATTACK_ANIM = "attack";
    private string JUMP_ANIM = "jump";

    void Start()
    {
        currentHealth = maxHealth;
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        myBody = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        AnimatePlayer();
        PlayerAttack();
    }

    private void FixedUpdate()
    {
        PlayerJump();
    }

    void MovePlayer()
    {
        movementX = Input.GetAxisRaw("Horizontal");

        transform.position += new Vector3(movementX, 0, 0) * moveSpeed * Time.deltaTime;
    }

    void AnimatePlayer()
    {
        if(movementX > 0)
        {
            animator.SetBool(RUN_ANIM,true);
            sr.flipX = true;
        }

        else if(movementX < 0)
        {
            animator.SetBool(RUN_ANIM, true);
            sr.flipX= false;
        }
        else
        {
            animator.SetBool(RUN_ANIM, false);
        }
    }

    void PlayerAttack()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //play attack anim
            animator.SetBool(ATTACK_ANIM, true);

            //check attack point and enemies in range
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attakcRange, enemyLayers);

            //give damage
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(20);
            }
        }
        else
        {
            animator.SetBool(ATTACK_ANIM, false);
        }

        
    }

    void PlayerJump()
    {
        if(Input.GetKey(KeyCode.K) && isGrounded) 
        {
            animator.SetBool(JUMP_ANIM, true);
            isGrounded = false;
            myBody.AddForce(new Vector3(0f,6f,0f), ForceMode2D.Impulse);
        }
        else
        {
            animator.SetBool(JUMP_ANIM, false);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            animator.SetBool("isDead", true);

           myBody.bodyType = RigidbodyType2D.Kinematic;
           GetComponent<Collider2D>().enabled = false;
           this.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attakcRange);
    }
}

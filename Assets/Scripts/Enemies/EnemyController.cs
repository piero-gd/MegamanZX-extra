using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb2d;
    bool isInvincible;
    public bool isDying = false;
    RigidbodyConstraints2D rb2dConstraints;
    public bool freezeEnemy;
    public bool isLastEnemy;


    [Header("Enemy Settings")]    
    public int currentHealth;
    public int maxHealth = 1;
    public int contactDamage = 1;
    public int bulletDamage = 1;
    public float bulletSpeed = 3f;

    [Header("Positions and Prefabs")]
    public GameObject bulletShootPos;
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void Flip()
    {
        transform.Rotate(0, 180f, 0);
    }
    public void Invincible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            Mathf.Clamp(currentHealth, 0, maxHealth);
            if (currentHealth <= 0)
            {
                Over.contadorMuertes++;
                Defeat();
            }
        }
    }

    void StartDestroyAnimation()
    {
        animator.SetTrigger("Die");
        isDying = true;
    }

    void StopDestroyAnimation()
    {
        Destroy(gameObject);
        //Debug.Log("muertes: "+Over.contadorMuertes);
        if (isLastEnemy)
        {
            Over.showVictory();
        }
    }

    void Defeat()
    {
        StartDestroyAnimation();
        //Destroy(gameObject);
        
    }

    public void FreezeEnemy(bool freeze)
    {
        // freeze/unfreeze the enemy on screen
        // zero animation speed and freeze XYZ rigidbody constraints
        // NOTE: this will be called from the GameManager but could be used in other scripts
        if (freeze)
        {
            freezeEnemy = true;
            animator.speed = 0;
            rb2dConstraints = rb2d.constraints;
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            freezeEnemy = false;
            animator.speed = 1;
            rb2d.constraints = rb2dConstraints;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.HitSide(transform.position.x > player.transform.position.x);
            player.TakeDamage(this.contactDamage);
            Debug.Log("HACIENDO DAÑO:"+this.contactDamage);
        }
    }

}

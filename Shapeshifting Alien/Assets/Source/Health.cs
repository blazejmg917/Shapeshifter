using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health settings")]
    [Tooltip("the max health of this character")]
    public int maxHealth = 5;
    //this creature's current health
    public int currentHealth;
    [Tooltip("how long this character is invincible for after being hit")]
    public float invincibleTime=.5f;
    //timer for keeping track of invincibility
    private float invincibleTimer=0f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(invincibleTimer >= 0)
        {
            invincibleTimer -= Time.fixedDeltaTime;
        }
    }

    public void DealDamage(int damage)
    {
        Debug.Log(invincibleTimer);
        if(invincibleTimer <= 0)
        {
            currentHealth -= damage;
            invincibleTimer = invincibleTime;
        }
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("player died");
        GameManager.Instance().GameOver(false);
    }

    public void Reset()
    {
        currentHealth = maxHealth;
        invincibleTimer = 0;
    }
}

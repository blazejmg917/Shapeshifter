using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Tooltip("the projectile this enemy uses to attack")]
    public GameObject projectile;
    [Space(15)]
    [Tooltip("the cooldown between attacks")]
    public float cooldown;
    
    
    //float used to keep track of weapon cooldown
    private float cooldownTimer = 0;
    //the target this character will attack
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Attack()
    {
        if (target != null)
        {
            if (cooldownTimer <= 0)
            {
                cooldownTimer = cooldown;
                Fire();
            }
            else
            {
                cooldownTimer -= Time.fixedDeltaTime;
            }
        }
        else if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.fixedDeltaTime;
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public GameObject GetTarget()
    {
        return target;
    }

    private void Fire()
    {
        GameObject proj = Instantiate(projectile, position: transform.position, rotation: transform.rotation);
        proj.GetComponent<projectile>().SetDirection(target.transform.position - transform.position);
        proj.SetActive(true);
    }
}

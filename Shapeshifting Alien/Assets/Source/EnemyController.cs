using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("character settings")]
    [Tooltip("The detection script for this enemy")]
    public Detection detectScript;
    [Tooltip("The attack script for this enemy")]
    public EnemyAttack attackScript;
    //the types of movement that are options
    public enum MoveType {Still, TurnRandom, TurnCW, TurnCCW};
    [Tooltip("The movement type for this enemy")]
    public MoveType moveType = MoveType.Still;
    [Tooltip("the delay between any type of movement")]
    public float moveDelay = 1;
    //the timer to check moveDelay
    private float moveTimer = 0;
    [Tooltip("how long the enemy will wait facing the same direction after a target leaves it's FOV before resuming normal movement")]
    public float waitTime = 1;
    //the timer to check waitTime
    private float waitTimer = 0;
    //the directions that the character can face
    public enum Directions { Up, Right, Down, Left};
    [Tooltip("The direction this enemy starts out facing")]
    public Directions enemyStartDir = Directions.Down;
    //the current direction the enemy is facing
    private Directions enemyDir = Directions.Down;
    //the four direction vectors
    private static Vector3 upDir;
    private static Vector3 rightDir;
    private static Vector3 downDir;
    private static Vector3 leftDir;
    [Tooltip("whether the enemy is dead or not")]
    public bool isDead = false;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        enemyDir = enemyStartDir;
        SetRotation((int)enemyStartDir);
        if (detectScript == null)
        {
            detectScript = gameObject.GetComponent<Detection>();
        }
        if (attackScript == null)
        {
            attackScript = gameObject.GetComponent<EnemyAttack>();
        }
        upDir = new Vector3(0,0,0);
        rightDir = new Vector3(0,0,90);
        downDir = new Vector3(0,0,180);
        leftDir = new Vector3(0,0,270);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        GameObject target = detectScript.Detect();
        if (target != null)
        {
            AttackEnemy(target);
        }
        else
        {
            float angToTarget = detectScript.SearchForTarget(attackScript.GetTarget());
            if (!Mathf.Approximately(angToTarget, 0f))
            {
                if (angToTarget > 0)
                {
                    RotateEnemy(false);
                }
                else
                {
                    RotateEnemy(true);
                }
                AttackEnemy(target);
            }
            else
            {
                attackScript.SetTarget(null);
                if (waitTimer <= 0)
                {
                    Movement();
                }
                else
                {
                    waitTimer -= Time.fixedDeltaTime;

                }
            }
        }
        
        
    }

    //handles what to do when there is no enemy


    //handles what to do if there is an enemy found
    public void AttackEnemy(GameObject target)
    {
        attackScript.SetTarget(target);
        attackScript.Attack();
        waitTimer = waitTime;
    }


    //handles normal movement if there isn't an active enemy
    public void Movement()
    {
        if (moveTimer <= 0)
        {
            if (moveType == MoveType.TurnCW)
            {
                RotateEnemy(true);
                moveTimer = moveDelay;
            }
            else if (moveType == MoveType.TurnCCW)
            {
                RotateEnemy(false);
                moveTimer = moveDelay;
            }
            else if (moveType == MoveType.TurnRandom)
            {
                int newDir = Random.Range(minInclusive: 0, maxExclusive: 4);
                SetRotation(newDir);
                moveTimer = moveDelay;
            }
            else
            {
                SetRotation((int)enemyStartDir);
            }
        }
        else
        {
            moveTimer -= Time.fixedDeltaTime;
        }
    }

    //gets the rotation vector based on which direction is faced.
    public static Vector3 GetDirVector( Directions dir)
    {
        if(dir == Directions.Up)
        {
            return upDir;
        }
        else if (dir == Directions.Right)
        {
            return rightDir;
        }
        else if (dir == Directions.Down)
        {
            return downDir;
        }
        else
        {
            return leftDir;
        }
    }
    //rotate the enemy. Rotates clockwise if cw = true, counterclockwise if cw = false
    private void RotateEnemy( bool cw )
    {
        int rotate = 1;
        if (cw)
        {
            rotate = -1;
        }
        //Debug.Log("dir before addition: " + (int)enemyDir);
        int newDir = (int)enemyDir + rotate;
        //Debug.Log("dir before mod: " + newDir);
        if(newDir < 0)
        {
            newDir = 3;
        }
        newDir %= 4;
        //Debug.Log("dir after mod: " + newDir);
        SetRotation(newDir);
        
    }

    private void SetRotation( int newDir )
    {
        if(newDir > 3 || newDir < 0)
        {
            Debug.LogWarning("tried to rotate to direction out of bounds");
        }
        enemyDir = (Directions)newDir;
        animator.SetFloat("Direction", (float)enemyDir);
    }
}

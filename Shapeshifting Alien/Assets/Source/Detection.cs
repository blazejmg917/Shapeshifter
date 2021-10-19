using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    [Header("Layers")]
    [Tooltip("the layer for the player and all other characters")]
    public LayerMask characterMask;
    private LayerMask ignoreCharacterMask;
    //[Tooltip("the layer for all obstacles that block enemy sightlines")]
    //private LayerMask obstacleMask;
    [Header("view/detection stats")]
    [Tooltip("the distance this character can see")]
    public float viewDist;
    [Tooltip("the angle that this character can see in (360 to see in all directions)")]
    [Range(0, 180)]
    public float viewAngle;
    [Space(20)]
    [Tooltip("the length of time that this character will remember a hostile enemy before forgetting them")]
    public float memoryLength;

    private ArrayList enemies = new ArrayList();
    private AggroEnemy currentEnemy = null;

    void Start()
    {
        ignoreCharacterMask = ~characterMask;
    }
    // The general detection called by an enemy
    public void Detect()
    {
        //show FOV in gizmos
        ShowView();



        //if the current enemy exists but can't be seen, set the current enemy to null
        if (currentEnemy != null && !CanSee(currentEnemy))
        {
            currentEnemy = null;
        }
        //check if each character within range can be seen
        Collider2D[] objects = Physics2D.OverlapCircleAll(point: transform.position, radius: viewDist, layerMask: characterMask);
        //Debug.Log(objects.Length + " Objects detected");
        foreach( Collider2D col in objects)
        {
            if (CanSee(col))
            {
                OnSeeCharacter(col.gameObject);
            }
        }
        //check if each enemy in the list can be seen
        for( int i = 0; i < enemies.Count; i++)
        {
            //if they are listed as an active enemy and can't be seen, set them to deactive
            AggroEnemy e = (AggroEnemy)enemies[i];
            if (e.IsActiveEnemy() && !CanSee(e))
            {
                e.SetActiveEnemy(false);
                Debug.Log("set enemy deactive");
                Report();
            }
            //if they aren't active, and the time since they have been seen is greater than memory length, remove them from the list of enemies
            else if (!e.IsActiveEnemy() && e.TimeSinceSeen() >= memoryLength)
            {
                enemies.Remove(e);
                Debug.Log("enemy removed due to time");
                Report();
                i--;
            }
        }

        //respond based on if there is a visible enemy or not
        if(currentEnemy != null)
        {
            OnSeeEnemy(currentEnemy);
        }
        else
        {
            OnNotSeeEnemy();
        }


    }
    private void ShowView()
    {
        RaycastHit2D hitM = Physics2D.Raycast(transform.position, transform.up, distance: viewDist, layerMask: ~Physics2D.IgnoreRaycastLayer);
        if (hitM.collider != null)
        {
            Debug.DrawRay(transform.position, transform.up.normalized * hitM.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.up.normalized * viewDist, Color.yellow);
        }
        Vector3 right = (Quaternion.AngleAxis(viewAngle, transform.forward) * transform.up).normalized * viewDist;
        Vector3 left = (Quaternion.AngleAxis(-viewAngle, transform.forward) * transform.up).normalized * viewDist;
        RaycastHit2D hitR = Physics2D.Raycast(transform.position, right, distance: viewDist, layerMask: ~Physics2D.IgnoreRaycastLayer);
        if (hitR.collider != null)
        {
            Debug.DrawRay(transform.position, right.normalized * hitR.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(transform.position, right, Color.yellow);

        }
        RaycastHit2D hitL = Physics2D.Raycast(transform.position, left, distance: viewDist, layerMask: ~Physics2D.IgnoreRaycastLayer);
        if (hitL.collider != null)
        {
            Debug.DrawRay(transform.position, left.normalized * hitL.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(transform.position, left, Color.yellow);
        }
    }

    //returns whether or not there is a current enemy
    public bool IsCurrentEnemy()
    {
        return currentEnemy != null;
    }


    //checks if a collider can be seen
    public bool CanSee(Collider2D col)
    {
        if(col == null)
        {
            return false;
        }
        Vector3 toCollider = col.transform.position - transform.position;
        float angle = Vector3.Angle(toCollider, transform.up);
        //Debug.Log("object at angle " + angle + " from forward");
        if (angle <= viewAngle && toCollider.magnitude <= viewDist && col.gameObject != gameObject)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, toCollider.normalized, toCollider.magnitude, ignoreCharacterMask + ~Physics2D.IgnoreRaycastLayer);
            if (hit.collider == null)
            {
                Debug.DrawRay(transform.position, toCollider, Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, toCollider.normalized * hit.distance, Color.red);
            }
        }
        return false;
    }

    //checks if an AggroEnemy Can be seen
    public bool CanSee(AggroEnemy e)
    {
        if(e == null)
        {
            return false;
        }
        Vector3 toCollider = e.GetEnemy().transform.position - transform.position;
        float angle = Vector3.Angle(toCollider, transform.up);
        //Debug.Log("object at angle " + angle + " from forward");
        if (angle <= viewAngle && toCollider.magnitude <= viewDist && e.GetEnemy().gameObject != gameObject)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, toCollider.normalized, toCollider.magnitude, ignoreCharacterMask + ~Physics2D.IgnoreRaycastLayer);
            if (hit.collider == null)
            {
                Debug.DrawRay(transform.position, toCollider, Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, toCollider.normalized * hit.distance, Color.red);
            }
        }
        return false;
    }

    //sees if the current enemy character can be found to the left or right of this 

    //handles what to do when a character is seen
    public void OnSeeCharacter(GameObject character)
    {
        bool aggro = AggroCheck.CheckAggro(gameObject, character);
        AggroEnemy enemy = GetEnemy(character);
        if(aggro && enemy == null) //if the target is hostile but not yet an enemy, add them to the enemy list
        {
            AddEnemy(character);
        }
        else if (!aggro && enemy != null) //if the target is not hostile, but is on the enemy list while not an active enemy...
        {
            if (enemy.IsActiveEnemy())
            {
                if (!enemy.CorrectForm())
                {
                    enemy.SetForm(enemy.GetEnemy().GetComponent<CharacterForm>().GetForm());
                }
                
            }
            else if (enemy.CorrectForm()) //if they are still in their last seen form, set them active again
            {
                enemy.SetActiveEnemy(true);
                if (currentEnemy == null)
                {
                    currentEnemy = enemy;
                }
                Debug.Log("set enemy active");
                Report();
            }
            else //if they are in a different form, remove them from the enemy list
            {
                RemoveEnemy(enemy);
            }
        }
        else if (aggro && enemy != null) //if the target is hostile and on the list, set them active and update their form.
        {
            /** edit this to use enums */
            if (!enemy.CorrectForm())
            {
                enemy.SetForm(enemy.GetEnemy().GetComponent<CharacterForm>().GetForm()); // set the enemy's known form to its current form.
                enemy.SetActiveEnemy(true);
                Debug.Log("updated enemy form");
                Report();
            }
            
            if (currentEnemy == null)
            {
                currentEnemy = enemy;
            }
            
        
        }
    }

    //handles what to do when seeing a hostile character
    public virtual void OnSeeEnemy( AggroEnemy enemy )
    {
        Debug.DrawRay(transform.position, enemy.GetEnemy().transform.position - transform.position, Color.blue);
        gameObject.GetComponent<EnemyAttack>().SetTarget(enemy.GetEnemy());
    }

    //handles what to do when there is not an active hostile enemy
    public virtual void OnNotSeeEnemy()
    {
        gameObject.GetComponent<EnemyAttack>().SetTarget(null);
    }

    //adds an enemy to the list of AggroEnemies
    private void AddEnemy(GameObject enemy)
    {
        /** edit this to use enums */
        AggroEnemy newEnemy = new AggroEnemy(enemy, enemy.GetComponent<CharacterForm>().GetForm());
        enemies.Add(newEnemy);
        if (currentEnemy == null)
        {
            currentEnemy = newEnemy;
        }
        Debug.Log("new enemy added");
        Report();
    }

    //removes an enemy from the list of AggroEnemies
    private void RemoveEnemy( AggroEnemy enemy)
    {
        enemies.Remove(enemy);
        if (currentEnemy != null && currentEnemy.Equals(enemy))
        {
            currentEnemy = null;
        }
        Debug.Log("enemy removed");
        Report();
    }

    //looks for an AggroEnemy with the given target, and returns it.
    //returns null if none exists
    private AggroEnemy GetEnemy(GameObject target)
    {
        foreach(AggroEnemy e in enemies)
        {
            if (e.GetEnemy().Equals(target))
            {
                return e;
            }
        }
        return null;
    }

    private void Report()
    {
        string report = "current enemies: " + enemies.Count + "\n";
        foreach( AggroEnemy e in enemies )
        {
            report += "enemy - known form: " + e.GetForm() + ", actual form: " + e.GetEnemy().GetComponent<CharacterForm>().GetForm() + ", active: " + e.IsActiveEnemy() + ", elapsed time: " + e.TimeSinceSeen() + "\n";
        }
        Debug.Log(report);
    }

    // Class for holding an enemy, the form that this character remembers them in, and whether they are still an active threat or not
    public class AggroEnemy
    {
        private GameObject enemy;
        private Shapeshift.Forms form;
        private bool activeEnemy;
        private float lastSeen;

        public AggroEnemy(GameObject enemy, Shapeshift.Forms form)
        {
            this.enemy = enemy;
            this.form = form;
            activeEnemy = true;
            lastSeen = Time.fixedTime;
        }

        public GameObject GetEnemy()
        {
            return enemy;
        }

        public Shapeshift.Forms GetForm()
        {
            return form;
        }

        public void SetForm(Shapeshift.Forms form)
        {
            this.form = form;
            UpdateLastSeen();
        }

        public bool IsActiveEnemy()
        {
            return activeEnemy;
        }

        public void SetActiveEnemy( bool isActive)
        {
            activeEnemy = isActive;
            
            UpdateLastSeen();
        }

        public void UpdateLastSeen()
        {
            lastSeen = Time.fixedTime;
        }

        public float TimeSinceSeen()
        {
            return Time.fixedTime - lastSeen;
        }

        //checks if the last known form is the current form
        public bool CorrectForm()
        {
            /** edit this to use enums */
            if(form == enemy.GetComponent<CharacterForm>().GetForm()){
                return true;
            }
            return false;
        }
    }

}

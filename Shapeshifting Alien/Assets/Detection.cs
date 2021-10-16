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
    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, viewDist, characterMask);
        Debug.Log(objects.Length + " Objects detected");
        foreach( Collider2D col in objects)
        {
            if (CanSee(col))
            {
                OnSeeCharacter(col.gameObject);
            }
        }
        for( int i = 0; i < enemies.Count; i++)
        {
            AggroEnemy e = (AggroEnemy)enemies[i];
            if (e.IsActiveEnemy() && !CanSee(e))
            {
                e.SetActiveEnemy(false);
            }
            else if (!e.IsActiveEnemy() && e.TimeSinceSeen() >= memoryLength)
            {
                enemies.Remove(e);
                i--;
            }
        }

        if(currentEnemy == null && enemies.Count > 0)
        {
            currentEnemy = (AggroEnemy)enemies[0];
        }

        if(currentEnemy != null)
        {
            OnSeeEnemy(currentEnemy);
        }


    }

    //checks if a collider can be seen
    public bool CanSee(Collider2D col)
    {
        Vector3 toCollider = col.transform.position - transform.position;
        float angle = Vector3.Angle(toCollider, transform.forward);
        Debug.Log("object at angle " + angle + " from forward");
        if (angle <= viewAngle && col.gameObject != gameObject)
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, toCollider.normalized, out hit, toCollider.magnitude, ignoreCharacterMask))
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
        Vector3 toCollider = e.GetEnemy().transform.position - transform.position;
        float angle = Vector3.Angle(toCollider, transform.forward);
        Debug.Log("object at angle " + angle + " from forward");
        if (angle <= viewAngle && e.GetEnemy().gameObject != gameObject)
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, toCollider.normalized, out hit, toCollider.magnitude, ignoreCharacterMask))
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
                return;
            }
            else if (enemy.CorrectForm()) //if they are still in their last seen form, set them active again
            {
                enemy.SetActiveEnemy(true);
            }
            else //if they are in a different form, remove them from the enemy list
            {
                RemoveEnemy(enemy);
            }
        }
        else if (aggro && enemy != null) //if the target is hostile and on the list, set them active and update their form.
        {
            //enemy.SetForm(enemy.GetEnemy.GetComponent<characterScript>().GetForm()); // set the enemy's known form to its current form.
            enemy.SetActiveEnemy(true);
            if (currentEnemy == null)
            {
                currentEnemy = enemy;
            }
        
        }
    }

    //handles what to do when seeing a hostile character
    public virtual void OnSeeEnemy( AggroEnemy enemy )
    {

    }

    //adds an enemy to the list of AggroEnemies
    private void AddEnemy(GameObject enemy)
    {
        //AggroEnemy newEnemy = new AggroEnemy(enemy, enemy.GetComponent<characterScript>().GetForm()
        //enemies.Add(newEnemy);
        if (currentEnemy == null)
        {
            //currentEnemy = newEnemy;
        }
    }

    //removes an enemy from the list of AggroEnemies
    private void RemoveEnemy( AggroEnemy enemy)
    {
        enemies.Remove(enemy);
        if (currentEnemy.Equals(enemy))
        {
            currentEnemy = null;
        }
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

    // Class for holding an enemy, the form that this character remembers them in, and whether they are still an active threat or not
    public class AggroEnemy : MonoBehaviour
    {
        private GameObject enemy;
        private int form;
        private bool activeEnemy;
        private float lastSeen;

        public AggroEnemy(GameObject enemy, int form)
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

        public int GetForm()
        {
            return form;
        }

        public void SetForm(int form)
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
            //if(form == enemy.GetComponent<characterScript>.GetForm(){
            //   return true
            //}
            return false;
        }
    }

}

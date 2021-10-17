using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool CheckAggro(GameObject self, GameObject target)
    {
        TypeCheckerDetectionTest selfScript = self.GetComponent<TypeCheckerDetectionTest>();
        TypeCheckerDetectionTest targetScript = target.GetComponent<TypeCheckerDetectionTest>();
        if(selfScript == null || targetScript == null){
            return false;
        }
        if(Contained(selfScript.GetEnemyForms(), targetScript.GetForm())){
            return true;
        }
        return false;
    }

    private static bool Contained( int[] container, int contained)
    {
        for( int i = 0; i < container.Length; i++)
        {
            if( container[i] == contained)
            {
                return true;
            }
        }
        return false;
    }
}

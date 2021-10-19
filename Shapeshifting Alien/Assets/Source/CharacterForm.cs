using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterForm : MonoBehaviour
{
    public Shapeshift.Forms form;
    public Shapeshift.Forms[] enemyForms;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Shapeshift.Forms GetForm()
    {
        return form;
    }

    public Shapeshift.Forms[] GetEnemyForms()
    {
        return enemyForms;
    }

}

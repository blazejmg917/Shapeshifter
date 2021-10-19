using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterForm : MonoBehaviour
{
    public Shapeshift.Forms form;
    public Shapeshift.Forms[] enemyForms;

    public Shapeshift.Forms GetForm()
    {
        return form;
    }

    public Shapeshift.Forms[] GetEnemyForms()
    {
        return enemyForms;
    }

}

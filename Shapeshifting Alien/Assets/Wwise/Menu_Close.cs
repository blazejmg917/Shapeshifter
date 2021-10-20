using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Close : MonoBehaviour
{
    public Menu_Close Post;
    public AK.Wwise.Event MenuClose;
    public void Menuclose()
    {
        MenuClose.Post(gameObject);
    }
}

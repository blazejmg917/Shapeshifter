using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Popup_System : MonoBehaviour
{
    public GameObject popUpBox;
   public Animator animator;
    public TMP_Text popUpText;

    public void PopUp(string Text)
    {
        popUpBox.SetActive(true);
        popUpText.text = Text;
        animator.SetTrigger("pop");
    }
}

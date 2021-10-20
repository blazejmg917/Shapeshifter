using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;

    public Text PlayerDxText1;
    public Text PlayerDxText2;
    public Text PlayerDxText3;

    public string[] PlayerDialogue;
    public Queue<string> sentences;
    void Start()
    {
        sentences = new Queue<string>();
       
    }
    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        Debug.Log("Starting conversation with " + dialogue.name);
        nameText.text = dialogue.name;
        sentences.Clear();
       


        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);

        }
        
        
        DisplayNextSentence();
        
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        
       string sentence = sentences.Dequeue();
        

        dialogueText.text = sentence;
       
    }
    public void StartPlayerDialogue(PlayerDialogue PlayerResponse)
    {
        
        foreach(string Response in PlayerResponse.PlayerResponse)
        {

            
        }
           
        
     
        
      

    }
   public void DisplayAnswers()
    {
       

       

    }


    void EndDialogue()
        {
            Debug.Log("End of Conversation");
        animator.SetBool("IsOpen", false);

    }
   
    
}

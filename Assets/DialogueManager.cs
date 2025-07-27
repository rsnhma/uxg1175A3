using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using NUnit.Framework;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    //how the text will appear, i can change fonts this way and refs the text layer 
    public TextMeshProUGUI dialoguetext;

   
    [TextArea] //an array of dialogue lines to implement in inspector
    public string[] lines;
    //the delay between each char of the text appearing, lower the number lower the delay
    public float textSpeed;

    //sound that plays when each letter plays out
    public AudioClip dialogueTyping; 

    //the background music
    private AudioSource audioSource;
   
    //the image component in the ui category
    public Image dialoguepanel;
    //array of images that i had imported into the assets folder 
    public Sprite[] panel;

    private int index;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //clears dialogue 
        dialoguetext.text = string.Empty;
        //starts coroutine (can suspend execution anytime)
        StartDialogue();

        audioSource = this.gameObject.AddComponent<AudioSource>();
   
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetMouseButtonDown(0)) //click
        {
            if (dialoguetext.text == lines[index]) //show entire dialogue in 1 index
            {
                NextLine();
              
            }
            else
            {
                StopAllCoroutines();
              
                dialoguetext.text = lines[index];
                
            }
        }
    }


    void StartDialogue()
    {
        index = 0;
        dialoguepanel.sprite = panel[index];
        StartCoroutine(TypeLine());
    }
    //typewriter effect
    IEnumerator TypeLine()
    {
        //go through each character
        foreach (char c in lines[index].ToCharArray())
        {
            //increase by 1 for each iteration
            dialoguetext.text += c;
            //suspends the coroutine for textspeed amount of time 
            yield return new WaitForSeconds(textSpeed);
            audioSource.PlayOneShot(dialogueTyping);

        }
    }

    //goes from one index to the next
    void NextLine()
    {
        //if index is less than total amount of lines in array
        if (index < lines.Length - 1) 
        {
            index++; //go to next array element
            dialoguetext.text = string.Empty;
            dialoguepanel.sprite = panel[index];
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            SceneManager.LoadSceneAsync(5);
        }
    }
  


}

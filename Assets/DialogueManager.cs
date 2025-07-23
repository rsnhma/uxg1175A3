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
    public TextMeshProUGUI dialoguetext;

   
    [TextArea]
    public string[] lines;
    public float textSpeed;

    public AudioClip dialogueTyping;

    private AudioSource audioSource;

    public Image dialoguepanel;
    public Sprite[] panel;

    private int index;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialoguetext.text = string.Empty;
        StartDialogue();

        audioSource = this.gameObject.AddComponent<AudioSource>();
   
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetMouseButtonDown(0))
        {
            if (dialoguetext.text == lines[index])
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

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            dialoguetext.text += c;
            yield return new WaitForSeconds(textSpeed);
            audioSource.PlayOneShot(dialogueTyping);

        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1) 
        {
            index++;
            dialoguetext.text = string.Empty;
            dialoguepanel.sprite = panel[index];
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            SceneManager.LoadSceneAsync(4);
        }
    }
  


}

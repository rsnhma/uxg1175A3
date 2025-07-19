using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public GameObject selectedchar;
    public GameObject Player;

    private Sprite playersprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playersprite = selectedchar.GetComponent<SpriteRenderer>().sprite;
        Player.GetComponent<SpriteRenderer>().sprite = playersprite;
    }

   
}

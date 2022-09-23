using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDamage : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D (Collider2D col) {
        if(col.gameObject.tag == "Player") {
            Debug.Log("Restart");
            gameManager.Restart();
        }
    }
}

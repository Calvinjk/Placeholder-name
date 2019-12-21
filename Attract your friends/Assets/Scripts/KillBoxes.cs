using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS SCRIPT ONLY WORKS WHEN THERE IS A GAMEMANAGER IN THE SCENE
public class KillBoxes : MonoBehaviour {

    private GameManager gm;

    // The moment this object is created, find the GameManager and store a reference to it
    void Awake() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // This function gets called by Unity the moment ANY collider enters the boundaries of this one.
    void OnCollisionEnter2D(Collision2D collider){
        gm.KillPlayer(collider.gameObject);
    }
}

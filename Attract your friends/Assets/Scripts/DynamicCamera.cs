using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour {
    private GameObject[] players;   // An array that holds all of the players in the scene

    private float leftBound;
    private float rightBound;
    private float bottomBound;
    private float topBound;

    void Awake(){
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void FixedUpdate(){
        // We need to figure out what things we want in the camera that would be at the edges of the camera
        DetermineCameraEdges();

        // TODO - Determine margins and actually set the camera.
    }

    void DetermineCameraEdges() {
        leftBound = Mathf.Infinity;
        rightBound = -Mathf.Infinity;
        bottomBound = Mathf.Infinity;
        topBound = -Mathf.Infinity;
        
        for (int i = 0; i < players.Length; ++i) {
            Vector2 pos = players[i].transform.position;
            if (pos.x < leftBound) { leftBound = pos.x; }
            if (pos.x > rightBound) { rightBound = pos.x; }
            if (pos.y < bottomBound) { bottomBound = pos.y; }
            if (pos.y > topBound) { topBound = pos.y; }
        }
    }
}

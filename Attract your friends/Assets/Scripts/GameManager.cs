using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public float respawnWaitTime = 3f;
    public Vector2 respawnLocation;

    private List<GameObject> playersToRespawn;
    private float curRespawnTime;

    // Awake runs once and only once right when this object is created (kind of)
    void Awake(){
        playersToRespawn = new List<GameObject>();
    }

    // Update runs once every frame
    void Update() {
        // Keep counting down.  Don't ever stop.
        curRespawnTime -= Time.deltaTime;

        // If the coutdown is below 0 and there are players that need to be respawned, let's respawn them
        if (curRespawnTime < 0 && playersToRespawn.Count != 0){
            RespawnPlayer(playersToRespawn[0], respawnLocation);    // First actually respawn the first player in line
            playersToRespawn.RemoveAt(0);                           // Then remove them from the list of players that need to be respawned
            curRespawnTime = respawnWaitTime;                       // And finally reset the respawn coutdown so that the next player in line has to wait
        }
    }

    // This funtion is only used by this script to actually respawn a player at a set location
    private void RespawnPlayer(GameObject player, Vector2 location){
        player.transform.position = location;
        player.SetActive(true);
    }

    // This function is used by other scripts in order to add players to the respawn list
    public void KillPlayer(GameObject player){
        // First "kill" the player
        player.SetActive(false);

        // If there are no players to respawn, we need to reset the countdown
        if (playersToRespawn.Count == 0) {
            curRespawnTime = respawnWaitTime;
        }

        // Lastly, add the player we want to respawn to the list of players we need to respawn
        playersToRespawn.Add(player);
    }
}

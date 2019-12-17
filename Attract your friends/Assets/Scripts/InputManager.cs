using UnityEngine;
using System.Collections;

public static class InputManager{
    public static float HorizontalAxis(int playerNum){
        return Mathf.Clamp(Input.GetAxis("Horizontal_Xbox" + playerNum) + Input.GetAxis("Horizontal"), -1, 1);
    }

    public static bool PullButton(int playerNum){
        return (Input.GetAxis("LTrigger_Xbox" + playerNum) > 0 ? true : false) || Input.GetKey(KeyCode.DownArrow);
    }

    public static bool PushButton(int playerNum){
        return (Input.GetAxis("RTrigger_Xbox" + playerNum) > 0 ? true : false) || Input.GetKey(KeyCode.UpArrow);
    }

    public static bool JumpButton(int playerNum){
        return Input.GetButton("Jump_Xbox" + playerNum) || Input.GetButton("Jump");
    }
}

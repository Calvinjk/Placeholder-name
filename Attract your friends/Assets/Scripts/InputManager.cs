﻿using UnityEngine;
using System.Collections;

public static class InputManager{
    public static float HorizontalAxis(int playerNum){
        float horizontalInput = 0;
        if (playerNum == 1){
            horizontalInput = Input.GetAxis("Horizontal_Keyboard");
        }
        return Mathf.Clamp(Input.GetAxis("Horizontal_Xbox" + playerNum) + horizontalInput, -1, 1);
    }

    public static bool PullButton(int playerNum){
        bool pullInput = false;
        if (playerNum == 1) {
            pullInput = Input.GetButton("Pull_Keyboard");
        }
        return (Input.GetAxis("LTrigger_Xbox" + playerNum) > 0 ? true : false) || pullInput;
    }

    public static bool PushButton(int playerNum){
        bool pushInput = false;
        if (playerNum == 1) {
            pushInput = Input.GetButton("Push_Keyboard");
        }
        return (Input.GetAxis("RTrigger_Xbox" + playerNum) > 0 ? true : false) || pushInput;
    }

    public static bool JumpButton(int playerNum){
        bool jumpInput = false;
        if (playerNum == 1) {
            jumpInput = Input.GetButton("Jump_Keyboard");
        }
        return Input.GetButton("Jump_Xbox" + playerNum) || jumpInput;
    }
}

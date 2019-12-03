using UnityEngine;
using System.Collections;

public static class InputManager{
    public static float HorizontalAxis(int playerNum){
        return Input.GetAxis("Horizontal_Xbox");;
    }

    public static bool PullButton(int playerNum){
        return Input.GetAxis("LTrigger_Xbox") > 0 ? true : false;
    }

    public static bool PushButton(int playerNum){
        return Input.GetAxis("RTrigger_Xbox") > 0 ? true : false;
    }

    public static bool JumpButton(int playerNum){
        return Input.GetButton("Jump_Xbox") ||
                Input.GetButton("Jump");
    }
}

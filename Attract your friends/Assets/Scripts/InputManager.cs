using UnityEngine;
using System.Collections;

public static class InputManager{
    public static float HorizontalAxis(){
        float result = 0.0f;
        result += Input.GetAxis("Horizontal");
        result += Input.GetAxis("Horizontal_Xbox");

        // Make sure that the player cannot use multiple input types at once
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static bool JumpButton(){
        return Input.GetButton("Jump_Xbox") ||
                Input.GetButton("Jump");
    }
}

using UnityEngine;
using System.Collections;

public static class InputManager{
    // Easy way to get horizontal and vertical inputs from one function
    public static Vector2 MainJoystick(){
        return new Vector2(HorizontalAxis(), VerticalAxis());
    }

    public static float VerticalAxis(){
        float result = 0.0f;
        result += Input.GetAxis("Vertical");
        result += Input.GetAxis("Vertical_Xbox");

        // Make sure that the player cannot use multiple input types at once
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static float HorizontalAxis(){
        float result = 0.0f;
        result += Input.GetAxis("Horizontal");
        result += Input.GetAxis("Horizontal_Xbox");

        // Make sure that the player cannot use multiple input types at once
        return Mathf.Clamp(result, -1.0f, 1.0f);
    }

    public static bool JumpButton(){
        return Input.GetButtonDown("Jump_Xbox") ||
                Input.GetButtonDown("Jump");
    }
}

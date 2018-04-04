using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerController : Controller {

    public bool InputEnabled = true;

    // Get axis input if input is enabled
    public float GetInputAxis (string axisName) {
        if (InputEnabled) {
            return Input.GetAxis(axisName);
        }
        return 0;
    }

    // Get button input if input is enabled
    public bool GetInputButton(string buttonName) {
        if (InputEnabled) {
            return Input.GetButton(buttonName);
        }
        return false;
    }

    // Get button input if input is enabled
    public bool GetInputButtonUp(string buttonName) {
        if (InputEnabled) {
            return Input.GetButtonUp(buttonName);
        }
        return false;
    }

    // Get button input if input is enabled
    public bool GetInputButtonDown(string buttonName) {
        if (InputEnabled) {
            return Input.GetButtonDown(buttonName);
        }
        return false;
    }
}

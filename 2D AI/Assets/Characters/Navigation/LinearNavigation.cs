using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearNavigation : NavigationSystem {

    // Constructor
    public LinearNavigation(Pawn _pawn) : base(_pawn) {
    }

    // Get the direction the pawn should go to
    public override Vector3 GetDirection() {
        if (pawn) {
            return target - pawn.transform.position;
        }
        return Vector3.zero;
    }
}

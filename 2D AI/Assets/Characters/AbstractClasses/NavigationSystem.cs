using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NavigationSystem {

    protected Pawn pawn;
    protected Vector3 target;

    // Constructor
    public NavigationSystem (Pawn _pawn) {
        pawn = _pawn;
    }

    // Set new target for navigation
    public virtual void SetTarget (Vector3 position) {
        target = position;
    }

    // Get the current target position
    public Vector3 GetTarget() {
        return target;
    }

    // Get the direction the pawn should go
    public abstract Vector3 GetDirection();
}

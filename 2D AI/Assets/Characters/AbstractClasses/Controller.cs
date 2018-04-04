using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour {

    protected Pawn pawn;

    // Possess a pawn
	public void Possess(Pawn newPawn) {
        pawn = newPawn;
        newPawn.SetController(this);

        // parent this controller to the pawn
        if (newPawn.transform != transform) {
            transform.parent = newPawn.transform;
            transform.localPosition = Vector3.zero;
        }
    }

    // Return the current possessed pawn
    public Pawn GetPawn () {
        return pawn;
    }
}

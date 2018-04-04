using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrollerController : PlayerController {

    // FixedUpdate is called in sync with physics
    void FixedUpdate() {
        SideScrollerPawn pawn = (SideScrollerPawn)this.pawn;

        if (pawn) {
            //Add input velocity
            pawn.SetHorizontalInput(GetInputAxis("MovementX"));

            //Add jump velocity
            if (pawn.IsGrounded() && GetInputButtonDown("Jump")) {
                pawn.Jump();
            }
        }
    }
}

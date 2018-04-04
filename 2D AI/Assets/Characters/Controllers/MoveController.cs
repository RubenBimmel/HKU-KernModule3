using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : PlayerController {

    // Update is called once per frame
    protected void Update() {
        // Get movement input and apply directly to the pawn
        Vector2 movement = new Vector2(GetInputAxis("MovementX"), GetInputAxis("MovementY"));
        pawn.SetVelocity(movement);
	}
}

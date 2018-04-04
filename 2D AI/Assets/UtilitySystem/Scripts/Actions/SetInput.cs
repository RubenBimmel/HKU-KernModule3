using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInputAction", menuName = "Utility system/Actions/Set input")]
public class SetInput : Action {

    public TransformVariable pawn;
    public bool move;
    public float value;
    public bool Jump;

    // Called every frame the action is triggered
    public override void Execute(Context context) {
        Transform pwn = context.GetTransform(pawn);
        if (pwn) {
            SideScrollerPawn ssp = pwn.GetComponent<SideScrollerPawn>();
            if (ssp) {
                // Apply movement and jump if they are active
                if (move) {
                    ssp.SetHorizontalInput(value);
                }
                if (Jump) {
                    ssp.Jump();
                }
            }
        }
    }

}

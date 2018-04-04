using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTargetAction", menuName = "Utility system/Actions/Copy movement")]
public class CopyMovement : Action {
    public TransformVariable myPawn;
    public TransformVariable target;

    // Called every frame the action is triggered
    public override void Execute(Context context) {
        SideScrollerPawn me = context.GetTransform(myPawn).GetComponent<SideScrollerPawn>();
        SideScrollerPawn other = context.GetTransform(target).GetComponent<SideScrollerPawn>();

        // Copy the other players velocity
        if (me != null && other != null) {
            if (other.GetVelocity().y > 0) {
                me.Jump();
            }
            me.SetHorizontalInput(other.GetVelocity().x * 2);
        }
    }
}

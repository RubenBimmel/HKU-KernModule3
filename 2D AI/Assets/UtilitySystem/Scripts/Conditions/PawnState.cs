using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPawnStateCondition", menuName = "Utility system/Condition/PawnState")]
public class PawnState : Condition {
    public enum States {
        IsTagged,
        IsAlive,
        IsGrounded
    }

    public States stateToCheck;
    public bool equalsValue;

    public TransformVariable pawn;

    // Called every frame the condition needs to be checked
    public override bool Check(Context context) {
        switch (stateToCheck) {
            case States.IsTagged:
                TagModePawn tmp = context.GetTransform(pawn).GetComponent<TagModePawn>();
                if (tmp) {
                    return tmp.tagged == equalsValue;
                }
                return true;
            case States.IsAlive:
                GoombaModePawn gmp = context.GetTransform(pawn).GetComponent<GoombaModePawn>();
                if (gmp) {
                    return gmp.alive == equalsValue;
                }
                return true;
            case States.IsGrounded:
                SideScrollerPawn ssp = context.GetTransform(pawn).GetComponent<SideScrollerPawn>();
                if (ssp) {
                    return ssp.IsGrounded() == equalsValue;
                }
                return true;
        }
        return false;
    }
}

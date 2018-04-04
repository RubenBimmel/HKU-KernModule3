using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVelocityCondition", menuName = "Utility system/Condition/Check velocity")]
public class CheckVelocity : CompareFloats {
    public enum Axis {
        x, y, z
    }

    public TransformVariable pawn;
    public float value;
    public Axis axis;

    // Called every frame the condition needs to be checked
    public override bool Check(Context context) {
        Pawn pwn = context.GetTransform(pawn).GetComponent<Pawn>();

        if (pwn) {
            switch (axis) {
                case Axis.x:
                    return Compare(pwn.GetVelocity().x, value);
                case Axis.y:
                    return Compare(pwn.GetVelocity().y, value);
                case Axis.z:
                    return Compare(pwn.GetVelocity().z, value);
            }
        }

        return false;
    }
}

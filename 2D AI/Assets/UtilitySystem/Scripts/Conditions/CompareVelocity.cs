using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVelocityCondition", menuName = "Utility system/Condition/Compare velocity")]
public class CompareVelocity : CompareFloats {
    public enum Axis {
        x, y, z
    }

    public TransformVariable lhs;
    public TransformVariable rhs;
    public float value;
    public Axis axis;

    // Called every frame the condition needs to be checked
    public override bool Check(Context context) {
        Pawn pwn1 = context.GetTransform(lhs).GetComponent<Pawn>();
        Pawn pwn2 = context.GetTransform(rhs).GetComponent<Pawn>();

        if (pwn1 && pwn2) {
            switch (axis) {
                case Axis.x:
                    float dX = pwn2.GetVelocity().x - pwn1.GetVelocity().x;
                    return Compare(dX, value);
                case Axis.y:
                    float dY = pwn2.GetVelocity().y - pwn1.GetVelocity().y;
                    return Compare(dY, value);
                case Axis.z:
                    float dZ = pwn2.GetVelocity().z - pwn1.GetVelocity().z;
                    return Compare(dZ, value);
            }
        }

        return false;
    }
}

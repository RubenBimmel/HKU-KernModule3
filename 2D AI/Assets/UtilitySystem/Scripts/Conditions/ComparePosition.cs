using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDistenceCondition", menuName = "Utility system/Condition/Compare position")]
public class ComparePosition : CompareFloats {
    public enum Axis {
        x, y, z
    }

    public TransformVariable lhs;
    public TransformVariable rhs;
    public float value;
    public Axis axis;

    // Called every frame the condition needs to be checked
    public override bool Check(Context context) {
        Transform object1 = context.GetTransform(lhs);
        Transform object2 = context.GetTransform(rhs);

        if (object1 && object2) {
            switch (axis) {
                case Axis.x:
                    float dX = object2.position.x - object1.position.x;
                    return Compare(dX, value);
                case Axis.y:
                    float dY = object2.position.y - object1.position.y;
                    return Compare(dY, value);
                case Axis.z:
                    float dZ = object2.position.z - object1.position.z;
                    return Compare(dZ, value);
            }
        }

        return false;
    }
}

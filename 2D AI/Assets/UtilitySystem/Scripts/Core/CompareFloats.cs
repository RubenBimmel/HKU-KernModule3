using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompareFloats : Condition {
    public enum Type {
        Equals,
        LargerThan,
        SmallerThan
    }

    public Type conditionType;

    // Compare two floats given a condition type
    public bool Compare(float lhs, float rhs) {
        switch (conditionType) {
            case Type.Equals:
                return lhs == rhs;
            case Type.LargerThan:
                return lhs > rhs;
            case Type.SmallerThan:
                return lhs < rhs;
        }
        return false;
    }
}

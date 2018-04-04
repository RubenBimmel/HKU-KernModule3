using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject {
    // Called every frame the action is triggered
    public abstract void Execute(Context context);
}

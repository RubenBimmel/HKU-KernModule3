using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewState", menuName = "Utility system/State")]
public class State : ScriptableObject {
    public int priority;
    public Condition[] conditions;
    public Action[] actions;

    // Called every frame if the state needs to be evaluated. Returns true if all conditions are met
    public bool Evaluate (Context context) {
        foreach (Condition c in conditions) {
            if (!c.Check(context)) {
                return false;
            }
        }
        return true;
    }

    // Called every frame the state is active. Executes all actions
    public void ExecuteActions (Context context) {
        foreach (Action a in actions) {
            a.Execute(context);
        }
    }
}

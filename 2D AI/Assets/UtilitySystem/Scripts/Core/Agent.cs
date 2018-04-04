using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAgent", menuName = "Utility system/Agent")]
public class Agent : ScriptableObject {

    public State[] states;

    // Called to initialize this agent
    public void Initialise() {
        // Reorder the list of states based on their priority
        List<State> orderedList = new List<State>();
        for (int i = 0; i < states.Length; i++) {
            for (int j = 0; j < orderedList.Count; j++) {
                if (orderedList[j].priority > states[i].priority) {
                    orderedList.Insert(j, states[i]);
                    break;
                }
            }
            if (!orderedList.Contains(states[i])) {
                orderedList.Add(states[i]);
            }
        }
        states = orderedList.ToArray();
    }

    // Gets called every frame the agent is active
	public void UpdateStates (Context context) {
		for (int i = 0; i < states.Length; i++) {
            if (states[i].Evaluate(context)) {
                states[i].ExecuteActions(context);
                return;
            }
        }
	}
}

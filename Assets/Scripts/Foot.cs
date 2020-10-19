using UnityEngine;

public class Foot: MonoBehaviour, Interact.Target {
    // -- lifecycle --
    protected void Update() {
        // enable hover on foot step
        if (Game.Get().DidChangeToStep(Game.Step.Foot)) {
            var hover = GetComponent<Interact.OnHover>();
            hover.enabled = true;
        }
    }
}

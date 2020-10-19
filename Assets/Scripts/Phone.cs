using UnityEngine;

public class Phone: MonoBehaviour, Interact.Target {
    // -- lifecycle --
    protected void Update() {
        // enable hover on phone step
        if (Game.Get().DidChangeToStep(Game.Step.Phone)) {
            var hover = GetComponent<Interact.OnHover>();
            hover.enabled = true;
        }
    }
}

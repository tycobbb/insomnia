using UnityEngine;

public class Sheep: MonoBehaviour, Interact.Target {
    // -- lifecycle --
    protected void Update() {
        // enable hover on door step
        if (Game.Get().DidChangeToStep(Game.Step.Sheep)) {
            var hover = GetComponent<Interact.OnHover>();
            hover.enabled = true;
        }
    }
}

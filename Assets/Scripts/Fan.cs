using UnityEngine;

public class Fan: MonoBehaviour, Interact.Target {
    // -- lifecycle --
    protected void Update() {
        // enable hover on fan step
        if (Game.Get().DidChangeToStep(Game.Step.Fan)) {
            Hover().Reset();
        }
    }

    // -- Interact.Target --
    public Interact.OnHover Hover() {
        return GetComponent<Interact.OnHover>();
    }
}

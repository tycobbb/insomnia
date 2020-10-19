using UnityEngine;

public class Door: MonoBehaviour, Interact.Target {
    // -- constants --
    public const string kOpenAnim = "Open";

    // -- lifecycle --
    protected void Update() {
        // enable hover on door step
        if (Game.Get().DidChangeToStep(Game.Step.Door)) {
            var hover = GetComponent<Interact.OnHover>();
            hover.enabled = true;
        }
    }

    // -- commands --
    public void Open() {
        Animator().Play(kOpenAnim);
    }

    // -- dependencies --
    private Animator Animator() {
        return GetComponent<Animator>();
    }
}

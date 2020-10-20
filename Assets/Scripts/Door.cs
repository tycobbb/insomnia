using UnityEngine;

public class Door: MonoBehaviour, Interact.Target {
    // -- constants --
    public const string kOpenAnim = "Open";
    public const string kCloseAnim = "Close";

    // -- lifecycle --
    protected void Update() {
        // enable hover on door step
        if (Game.Get().DidChangeToStep(Game.Step.Door1 | Game.Step.Door2)) {
            Hover().enabled = true;
        }
    }

    protected void OnTriggerEnter(Collider closeSwitch) {
        Close();
    }

    // -- commands --
    public void Open() {
        Animator().Play(kOpenAnim);
    }

    public void Close() {
        Animator().Play(kCloseAnim);
    }

    // -- Interact.Target --
    public Interact.OnHover Hover() {
        return GetComponentInChildren<Interact.OnHover>();
    }

    // -- dependencies --
    private Animator Animator() {
        return GetComponent<Animator>();
    }
}

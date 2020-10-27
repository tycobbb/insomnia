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

    private IEnumerator EnableAsync() {
        mHover.Reset();

        // automatically trigger interaction after a few seconds
        yield return new WaitForSeconds(kAutoInteractDelay);
        if (mHover.enabled && Game.Get().CanAdvancePast(kStep)) {
            mHover.InteractWith(this);
        }
    }
}

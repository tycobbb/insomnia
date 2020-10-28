using System.Collections;
using UnityEngine;

public class Fan: MonoBehaviour, Interact.Target {
    // -- constants --
    private const Game.Step kStep = Game.Step.Fan;
    private const float kAutoInteractDelay = 10.0f;

    // -- props --
    private Interact.OnHover mHover;
    private AudioSource mSound;

    // -- lifecycle --
    protected void Awake() {
        mHover = GetComponent<Interact.OnHover>();
        mSound = GetComponent<AudioSource>();
    }

    protected void Update() {
        if (Game.Get().DidChangeToStep(kStep)) {
            Enable();
        }
    }

    // -- commands --
    private void Enable() {
        StartCoroutine(EnableAsync());
    }

    private IEnumerator EnableAsync() {
        mHover.Reset();

        // automatically trigger interaction after a few seconds
        yield return new WaitForSeconds(kAutoInteractDelay);
        if (mHover.enabled && Game.Get().CanAdvancePast(kStep)) {
            mHover.InteractWith(this);
        }
    }

    public void Silence() {
        mSound.Stop();
    }
}

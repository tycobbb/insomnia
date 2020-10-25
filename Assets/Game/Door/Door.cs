using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Door: MonoBehaviour, Interact.Target {
    // -- constants --
    private const Game.Step kStep = Game.Step.Door1 | Game.Step.Door2 | Game.Step.Door3;
    private const float kEnableDelay = 4.5f;
    private const string kOpenAnim = "Open";
    private const string kCloseAnim = "Close";

    // -- fields --
    [SerializeField]
    [Tooltip("The sheep sound.")]
    private AudioClip fSheepSound;

    // -- props --
    private Interact.OnHover mHover;
    private Animator mAnimator;
    private AmbientSound mAmbientSound;

    // -- lifecycle --
    protected void Start() {
        mHover = GetComponentInChildren<Interact.OnHover>();
        mAnimator = GetComponent<Animator>();
        mAmbientSound = GetComponent<AmbientSound>();
    }

    protected void Update() {
        // enable on door step
        var game = Game.Get();
        if (game.DidChangeToStep(kStep)) {
            Enable(game.GetStep());
        }
    }

    // -- commands --
    private void Enable(Game.Step step) {
        StartCoroutine(EnableAsync(step));
    }

    private IEnumerator EnableAsync(Game.Step step) {
        yield return new WaitForSeconds(kEnableDelay);

        mHover.Reset();

        switch (step) {
            case Game.Step.Door1:
                mAmbientSound.Play(fSheepSound); break;
            case Game.Step.Door2:
                break; // TODO: play a sound to cue the door to the kitchen
            case Game.Step.Door3:
                break; // TODO: play a sound to cue the door to the hall
        }
    }

    public void Open() {
        mAmbientSound.Stop();
        // TODO: play door sound
        mAnimator.Play(kOpenAnim);
    }

    public void Close() {
        // TODO: play door sound
        mAnimator.Play(kCloseAnim);
    }
}

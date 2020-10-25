using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Door: MonoBehaviour, Interact.Target {
    // -- constants --
    private const float kEnableDelay = 4.5f;
    private const string kOpenAnim = "Open";
    private const string kCloseAnim = "Close";

    // -- fields --
    [SerializeField]
    [Tooltip("The sheep sound.")]
    private AudioClip fSheepSound;

    // -- props --
    private Animator mAnimator;
    private AudioSource mAmbientAudio;
    private Coroutine mAmbientSound;

    // -- lifecycle --
    protected void Start() {
        mAnimator = GetComponent<Animator>();
        mAmbientAudio = GetComponent<AudioSource>();
    }

    protected void Update() {
        // enable on door step
        var game = Game.Get();
        if (game.DidChangeToStep(Game.Step.Door1 | Game.Step.Door2 | Game.Step.Door3)) {
            Enable(game.GetStep());
        }
    }

    // -- commands --
    private void Enable(Game.Step step) {
        StartCoroutine(EnableAsync(step));
    }

    private IEnumerator EnableAsync(Game.Step step) {
        yield return new WaitForSeconds(kEnableDelay);

        Hover().Reset();

        switch (step) {
            case Game.Step.Door1:
                PlayAmbientSound(fSheepSound); break;
        }
    }

    private void PlayAmbientSound(AudioClip clip) {
        StopAmbientSound();
        mAmbientAudio.clip = clip;
        mAmbientSound = StartCoroutine(PlayAmbientSoundAsync());
    }

    private void StopAmbientSound() {
        if (mAmbientSound != null) {
            StopCoroutine(mAmbientSound);
        }

        mAmbientSound = null;
    }

    private IEnumerator PlayAmbientSoundAsync() {
        var duration = mAmbientAudio.clip.length;

        while (true) {
            mAmbientAudio.Play();
            yield return new WaitForSeconds(duration + Random.Range(4.0f, 10.0f));
        }
    }

    public void Open() {
        StopAmbientSound();
        // TODO: play door sound
        mAnimator.Play(kOpenAnim);
    }

    public void Close() {
        // TODO: play door sound
        mAnimator.Play(kCloseAnim);
    }

    // -- Interact.Target --
    public Interact.OnHover Hover() {
        return GetComponentInChildren<Interact.OnHover>();
    }
}

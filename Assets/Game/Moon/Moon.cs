using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Moon: MonoBehaviour, Interact.Target {
    // -- constants --
    private const float kEnableDelay = 3.5f;
    private const float kAutoInteractDelay = 5.0f;
    private const string kShowMoonAnim = "ShowMoon";

    // -- fields --
    [SerializeField]
    [Tooltip("The actual moon.")]
    private GameObject fMoon = null;

    // -- props --
    private Interact.OnHover mHover;
    private Animator mAnimator;

    // -- lifecycle --
    void Awake() {
        mHover = fMoon.GetComponent<Interact.OnHover>();
        mAnimator = GetComponent<Animator>();
    }

    protected void Update() {
        if (Game.Get().DidChangeToStep(Game.Step.Moon)) {
            Enable();
        }
    }

    // -- commands --
    private void Enable() {
        StartCoroutine(EnableAsync());
    }

    private IEnumerator EnableAsync() {
        // enable after a few seconds
        yield return new WaitForSeconds(kEnableDelay);

        fMoon.SetActive(true);
        mHover.Reset();
        mAnimator.Play(kShowMoonAnim);

        // auto-interact after a few seconds
        yield return new WaitForSeconds(kAutoInteractDelay);
        if (mHover.enabled) {
            mHover.InteractWith(this);
        }
    }
}

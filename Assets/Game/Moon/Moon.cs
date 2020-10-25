using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Moon: MonoBehaviour, Interact.Target {
    // -- constants --
    private const float kEnableDelay = 3.5f;
    private const string kShowMoonAnim = "ShowMoon";

    // -- props --
    private Interact.OnHover mHover;
    private Animator mAnimator;

    // -- lifecycle --
    void Start() {
        mHover = GetComponentInChildren<Interact.OnHover>();
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
        yield return new WaitForSeconds(kEnableDelay);

        mHover.Reset();
        mAnimator.Play(kShowMoonAnim);
    }
}

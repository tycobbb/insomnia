using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class Body: MonoBehaviour, Interact.Target {
    // -- constants --
    private const string kWiggleWaitAnim = "WiggleWait";
    private const string kWiggleLeftAnim = "WiggleLeft";
    private const string kWiggleRightAnim = "WiggleRight";

    // -- props --
    private Interact.OnHover mHover;
    private Animator mAnimator;

    // -- lifecycle --
    protected void Start() {
        mHover = GetComponentInChildren<Interact.OnHover>();
        mAnimator = GetComponent<Animator>();
    }

    protected void Update() {
        // enable on foot step
        if (Game.Get().DidChangeToStep(Game.Step.Foot1 | Game.Step.Foot2 | Game.Step.Foot3)) {
            Enable();
        }
    }

    // -- commands --
    public void Show() {
        gameObject.SetActive(true);
    }

    private void Enable() {
        mHover.Reset();
        Wiggle();
    }

    private void Wiggle() {
        mAnimator.Play(GetRandomWiggleAnim());
    }

    public void Remove() {
        StartCoroutine(RemoveAsync());
    }

    private IEnumerator RemoveAsync() {
        yield return mHover.Transition();
        gameObject.SetActive(false);
    }

    // -- queries --
    private static string GetRandomWiggleAnim() {
        switch (Random.Range(0, 6)) {
            case 0:
                return kWiggleLeftAnim;
            case 1:
                return kWiggleRightAnim;
            default:
                return kWiggleWaitAnim;
        }
    }

    // -- events --
    [UsedImplicitly] // AnimationEvent
    private void DidWiggle() {
        Wiggle();
    }
}

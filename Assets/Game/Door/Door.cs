using UnityEngine;

public class Door: MonoBehaviour, Interact.Target {
    // -- constants --
    private const string kOpenAnim = "Open";
    private const string kCloseAnim = "Close";

    // -- props --
    private Animator mAnimator;

    // -- lifecycle --
    protected void Start() {
        mAnimator = GetComponent<Animator>();
    }

    // -- commands --
    public void Open() {
        // TODO: play door sound
        mAnimator.Play(kOpenAnim);
    }

    public void Close() {
        // TODO: play door sound
        mAnimator.Play(kCloseAnim);
    }
}

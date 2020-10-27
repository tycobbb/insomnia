using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class Eye: MonoBehaviour {
    // -- constants --
    private const string kOpenAnim = "Open";
    private const float kOpenDelay = 2.0f;

    // -- fields --
    // [SerializeField]
    // [Tooltip("The eye animator.")]
    private Animator mAnimator;
    private GameObject mOverlay;

    // -- lifecycle --
    private void Awake() {
        mAnimator = GetComponent<Animator>();
        mOverlay = transform.GetChild(0).gameObject;
    }

    // -- commands --
    public void Open() {
        mOverlay.SetActive(true);
        StartCoroutine(OpenAsync());
    }

    private IEnumerator OpenAsync() {
        yield return new WaitForSeconds(kOpenDelay);
        mAnimator.Play(kOpenAnim);
    }

    [UsedImplicitly] // AnimationEvent
    private void DidOpen() {
        mOverlay.SetActive(false);
    }
}

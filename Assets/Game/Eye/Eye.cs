using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class Eye: MonoBehaviour {
    // -- constants --
    private const string kOpenAnim = "Open";
    private const float kOpenDelay = 2.0f;

    // -- fields --
    [SerializeField]
    [Tooltip("The overlay container.")]
    private GameObject tOverlay;

    [SerializeField]
    [Tooltip("The overlay container.")]
    private GameObject tBlack;

    [SerializeField]
    [Tooltip("The overlay container.")]
    private GameObject tWhite;

    // -- fields --
    private Animator mAnimator;
    private GameObject mColor;

    // -- lifecycle --
    private void Awake() {
        mAnimator = GetComponent<Animator>();
    }

    // -- commands --
    public void Open(bool isWhite = false) {
        mColor = isWhite ? tWhite : tBlack;
        tOverlay.SetActive(true);
        mColor.SetActive(true);
        StartCoroutine(OpenAsync());
    }

    private IEnumerator OpenAsync() {
        yield return new WaitForSeconds(kOpenDelay);
        mAnimator.Play(kOpenAnim);
    }

    [UsedImplicitly] // AnimationEvent
    private void DidOpen() {
        tOverlay.SetActive(false);
        mColor.SetActive(false);
    }
}

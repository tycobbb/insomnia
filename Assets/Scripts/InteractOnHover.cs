using System.Collections;
using UnityEngine;
using TMPro;

public class InteractOnHover: MonoBehaviour {
    // -- constants --
    private const float kDuration = 0.2f;

    // -- fields --
    [SerializeField]
    [Tooltip("The text to display in the prompt.")]
    private string fPrompt = "";

    [SerializeField]
    [Tooltip("The minimum distance before showing the tooltip.")]
    private float fMinDistance = 20.0f;

    // -- props --
    private GameObject mPrompt;
    private bool mIsVisible = false;
    private IEnumerator mAnimation;

    // -- lifecycle --
    void Start() {
        mPrompt = Instantiate(LoadPrefab(), transform);
        mPrompt.GetComponent<TextMeshPro>().text = fPrompt;
    }

    void Update() {
        var isVisible = IsInView();

        // if visible, make sure the prompt faces the camera
        if (isVisible) {
            mPrompt.transform.forward = MainCamera().transform.forward;
        }

        // if visibility changed, update prompt
        if (isVisible != mIsVisible) {
            mIsVisible = isVisible;
            StartCoroutine(ShowPrompt(isVisible));
        }
    }

    // -- commands --
    private IEnumerator ShowPrompt(bool isVisible) {
        if (isVisible) {
            mPrompt.SetActive(true);
            yield return Animate(FadePrompt(from: 0.0f, to: 1.0f));
        } else {
            yield return Animate(FadePrompt(from: 1.0f, to: 0.0f));
            mPrompt.SetActive(false);
        }
    }

    // -- animations --
    private IEnumerator Animate(IEnumerator animation) {
        if (mAnimation != null) {
            StopCoroutine(mAnimation);
        }

        mAnimation = animation;
        yield return animation;
        mAnimation = null;
    }

    private IEnumerator FadePrompt(float from, float to) {
        // set initial alpha
        SetPromptAlpha(from);

        // capture delta
        var delta = to - from;

        // calculate end time
        var now = Time.time;
        var stop = now + kDuration;

        // animate every frame until the duration elapses
        while (now < stop) {
            yield return 0;

            now = Time.time;
            var percent = 1 - (stop - now) / kDuration;
            SetPromptAlpha(from + Mathf.Min(percent, 1.0f) * delta);
        }
    }

    private void SetPromptAlpha(float alpha) {
        var renderers = mPrompt.GetComponentsInChildren<MeshRenderer>();

        foreach (var renderer in renderers) {
            var label = renderer.GetComponent<TextMeshPro>();

            // set color on the text mesh if available
            if (label != null) {
                label.color = label.color.WithAlpha(alpha);
            }
            // otherwise try the material
            else {
                var material = renderer.material;
                material.color = material.color.WithAlpha(alpha);
            }
        }
    }

    // -- queries --
    private bool IsInView() {
        var camera = MainCamera();
        var screen = camera.WorldToScreenPoint(transform.position);

        // check if were behind camera
        if (screen.z < 0) {
            return false;
        }

        // check if were in frame
        if (!Screen.safeArea.Contains(screen)) {
            return false;
        }

        // check if the a raycast hits this object
        RaycastHit contact;
        var source = camera.transform;
        var didHit = Physics.Raycast(source.position, source.forward, out contact, fMinDistance);

        if (!didHit || contact.transform != transform) {
            return false;
        }

        return true;
    }

    // -- accessors --
    private Camera MainCamera() {
        return Camera.main;
    }

    private TextMeshPro TextMesh() {
        return GetComponent<TextMeshPro>();
    }

    // -- factories --
    private static GameObject LoadPrefab() {
        return Resources.Load<GameObject>("Prompt");
    }
}

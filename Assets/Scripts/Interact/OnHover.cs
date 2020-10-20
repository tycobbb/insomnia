using System.Collections;
using UnityEngine;
using TMPro;

namespace Interact {
    [RequireComponent(typeof(Collider))]
    public class OnHover: MonoBehaviour {
        // -- constants --
        private const float kFadeDuration = 0.2f;
        private const int kWaitFrames = 7;

        // -- fields --
        [SerializeField]
        [Tooltip("The prompt to show on hover")]
        private GameObject fPrompt = null;

        [SerializeField]
        [Tooltip("The radius from the center considered visible")]
        private float fRadius = 1.6f;

        [SerializeField]
        [Tooltip("The minimum distance to show the prompt")]
        private float fMinDistance = 20.0f;

        // -- props --
        // private GameObject mPrompt;
        private bool mIsHovering = false;
        private int mWaitFrame = 0;
        private IEnumerator mTransition;

        // -- lifecycle --
        protected void Start() {
            fPrompt.SetActive(mIsHovering);
        }

        protected void Update() {
            var isHovering = IsHovering();

            // if visible, make sure the prompt faces the camera
            if (isHovering) {
                fPrompt.transform.forward = MainCamera().transform.forward;
            }

            // if visibility is the same, tare wait frames
            if (isHovering == mIsHovering) {
                mWaitFrame = 0;
            }
            // otherwise, wait a few frames before changing visibility
            else {
                mWaitFrame++;
                if (mWaitFrame >= kWaitFrames) {
                    StartCoroutine(Transition(ShowPrompt(isHovering)));
                }
            }

            // on click (when hovering)
            if (mIsHovering && Input.GetMouseButtonDown(0)) {
                // send an event to the game
                Game.Get().OnInteract(GetComponentInParent<Target>());

                // and disable this component
                this.enabled = false;
                StartCoroutine(Transition(ShowPrompt(false)));
            }
        }

        // -- commands --
        private IEnumerator Transition(IEnumerator transition) {
            if (mTransition != null) {
                StopCoroutine(mTransition);
            }

            mTransition = transition;
            yield return transition;
            mTransition = null;
        }

        private IEnumerator ShowPrompt(bool isVisible) {
            mIsHovering = isVisible;

            if (isVisible) {
                fPrompt.SetActive(true);
                yield return FadePrompt(from: 0.0f, to: 1.0f);
            } else {
                yield return FadePrompt(from: 1.0f, to: 0.0f);
                fPrompt.SetActive(false);
            }
        }

        // -- queries --
        private bool IsHovering() {
            var camera = MainCamera();
            var screen = camera.WorldToScreenPoint(transform.position);

            // check if we're behind the camera
            if (screen.z < 0) {
                return false;
            }

            // check if we're in frame
            if (!Screen.safeArea.Contains(screen)) {
                return false;
            }

            // check if a spherecast hits this object
            var hits = Physics.SphereCastAll(
                camera.transform.position,
                fRadius,
                camera.transform.forward,
                fMinDistance
            );

            foreach (var hit in hits) {
                if (hit.transform == transform) {
                    return true;
                }
            }

            return false;
        }

        // -- animations --
        private IEnumerator FadePrompt(float from, float to) {
            // set initial alpha
            SetPromptAlpha(from);

            // capture delta
            var delta = to - from;

            // calculate end time
            var now = Time.time;
            var stop = now + kFadeDuration;

            // animate every frame until the duration elapses
            while (now < stop) {
                yield return 0;

                now = Time.time;
                var percent = 1 - (stop - now) / kFadeDuration;
                SetPromptAlpha(from + Mathf.Min(percent, 1.0f) * delta);
            }
        }

        private void SetPromptAlpha(float alpha) {
            var renderers = fPrompt.GetComponentsInChildren<MeshRenderer>();

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
        public IEnumerator Transition() {
            return mTransition;
        }

        // -- accessors --
        private Camera MainCamera() {
            return Camera.main;
        }
    }
}

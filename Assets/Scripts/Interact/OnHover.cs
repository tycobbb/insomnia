using System.Collections;
using UnityEngine;
using TMPro;

namespace Interact {
    public class OnHover: MonoBehaviour {
        // -- constants --
        private const float kFadeDuration = 0.2f;
        private const int kWaitFrames = 7;

        // -- types --
        enum Mode {
            Fixed,
            Dynamic,
        }

        // -- fields --
        [SerializeField]
        [Tooltip("The prompt to show on hover")]
        private GameObject fPrompt = null;

        [SerializeField]
        [Tooltip("The hover mode. Fixed or child-attaching.")]
        private Mode fMode = Mode.Fixed;

        [SerializeField]
        [Tooltip("The radius from the center considered visible")]
        private float fRadius = 1.6f;

        [SerializeField]
        [Tooltip("The minimum distance to show the prompt")]
        private float fMinDistance = 20.0f;

        // -- props --
        private GameObject mHovered = null;
        private GameObject mSelected = null;
        private int mWaitFrame = 0;
        private IEnumerator mTransition;

        // -- lifecycle --
        protected void Start() {
            switch (fMode) {
                case Mode.Fixed when GetComponent<Collider>() == null:
                    Debug.LogWarningFormat("OnHover requires at a collider on this object."); break;
                case Mode.Dynamic when GetComponentInChildren<Collider>() == null:
                    Debug.LogWarningFormat("OnHover requires at least one child collider."); break;
            }

            fPrompt.SetActive(false);
        }

        protected void Update() {
            var hovered = GetHoveredObject();

            // if the hovered object changes, tare the wait frames
            if (hovered != mHovered) {
                Hover(hovered);
                mWaitFrame = 0;
            }
            // otherwise, wait a few frames before changing targets
            else if (hovered != mSelected) {
                mWaitFrame++;

                if (mWaitFrame >= kWaitFrames) {
                    Log.Debug("OnHover - Select: {0}", hovered);
                    Select(mHovered);
                }
            }

            // re-orient the prompt towards the camera if something is selected
            if (mSelected != null) {
                fPrompt.transform.forward = MainCamera().transform.forward;
            }

            // if something is selected, send an interaction event on click
            if (mSelected != null && Input.GetMouseButtonDown(0)) {
                // send an event to the game
                Game.Get().OnInteract(GetComponentInParent<Target>());

                // and disable this component
                Select(null);
                this.enabled = false;
            }
        }

        // -- commands --
        public void Reset() {
            this.enabled = true;
            fPrompt.SetActive(false);
        }

        private void Hover(GameObject hovered) {
            Log.Debug("OnHover - Hover: {0}", hovered);
            mHovered = hovered;
        }

        private void Select(GameObject selected) {
            Log.Debug("OnHover - Select: {0}", selected);
            mHovered = selected;
            mSelected = selected;
            StartCoroutine(Transition(ShowPrompt(selected != null)));
        }

        // -- commands/transitions
        private IEnumerator ShowPrompt(bool isVisible) {
            if (isVisible && fMode == Mode.Dynamic) {
                var pos = mSelected.transform.position;
                pos += new Vector3(-0.5f, 0.5f);
                fPrompt.transform.position = pos;
            }

            if (isVisible) {
                fPrompt.SetActive(true);
                yield return FadePrompt(from: 0.0f, to: 1.0f);
            } else {
                yield return FadePrompt(from: 1.0f, to: 0.0f);
                fPrompt.SetActive(false);
            }
        }

        private IEnumerator Transition(IEnumerator transition) {
            if (mTransition != null) {
                StopCoroutine(mTransition);
            }

            mTransition = transition;
            yield return transition;
            mTransition = null;
        }

        // -- queries --
        private GameObject GetHoveredObject() {
            var camera = MainCamera();
            var screen = camera.WorldToScreenPoint(transform.position);

            // check if we're behind the camera
            if (screen.z < 0) {
                return null;
            }

            // check if we're in frame
            if (!Screen.safeArea.Contains(screen)) {
                return null;
            }

            // check if a spherecast hits this object
            var hits = Physics.SphereCastAll(
                camera.transform.position,
                fRadius,
                camera.transform.forward,
                fMinDistance
            );

            if (fMode == Mode.Fixed) {
                foreach (var hit in hits) {
                    if (hit.transform == transform) {
                        return hit.collider.gameObject;
                    }
                }
            } else {
                foreach (var hit in hits) {
                    if (hit.transform.IsChildOf(transform)) {
                        return hit.collider.gameObject;
                    }
                }
            }

            return null;
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
        public GameObject Selected() {
            return mSelected;
        }

        public IEnumerator Transition() {
            return mTransition;
        }

        // -- accessors --
        private Camera MainCamera() {
            return Camera.main;
        }
    }
}

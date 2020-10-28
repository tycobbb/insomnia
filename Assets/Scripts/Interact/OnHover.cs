using System.Collections;
using UnityEngine;
using TMPro;

namespace Interact {
    public class OnHover: MonoBehaviour {
        // -- constants --
        private const float kFadeDuration = 0.2f;
        private const int kWaitFrames = 7;

        // -- types --
        private enum Mode {
            Fixed,
            Dynamic,
        }

        // -- fields --
        [SerializeField]
        [Tooltip("The prompt to show on hover. The first child if not set")]
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

        [SerializeField]
        [Tooltip("The XZ proximity to check when evaluating visibility. Ignored if 0.")]
        private float fXzProximity = 0.0f;

        // -- props --
        private Camera mCamera;
        private GameObject mHovered = null;
        private GameObject mSelected = null;
        private int mWaitFrame = 0;
        private IEnumerator mTransition;

        // -- lifecycle --
        protected void Awake() {
            mCamera = Camera.main;

            if (fPrompt == null) {
                var child = transform.GetChild(0);
                if (child.GetComponent<TextMeshPro>() != null) {
                    fPrompt = child.gameObject;
                }
            }
        }

        protected void Start() {
            // warn on configuration errors
            switch (fMode) {
                case Mode.Fixed when GetComponent<Collider>() == null:
                    Debug.LogWarningFormat("OnHover requires at a collider on this object."); break;
                case Mode.Dynamic when GetComponentInChildren<Collider>() == null:
                    Debug.LogWarningFormat("OnHover requires at least one child collider."); break;
            }

            // configure prompt
            if (HasPrompt()) {
                fPrompt.SetActive(false);
            }
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
                    Select(mHovered);
                }
            }

            // if this item has a prompt and something is selected
            if (HasPrompt() && mSelected != null) {
                // re-orient the prompt towards the camera
                fPrompt.transform.forward = mCamera.transform.forward;

                // interact on click
                if (Input.GetMouseButtonDown(0)) {
                    Interact();
                }
            }
        }

        // -- commands --
        public void Reset() {
            enabled = true;

            if (HasPrompt()) {
                fPrompt.SetActive(false);
            }
        }

        private void Hover(GameObject hovered) {
            Log.Debug("OnHover - Hover: {0}", hovered);
            mHovered = hovered;
        }

        private void Select(GameObject selected) {
            Log.Debug("OnHover - Select: {0}", selected);
            mSelected = selected;

            // if this has a prompt to click, show it
            if (HasPrompt()) {
                StartCoroutine(Transition(ShowPrompt(selected != null)));
            }
            // otherwise, interact immediately on select
            else if (selected != null) {
                Interact();
            }
        }

        private void Interact() {
            Log.Debug("OnHover - Interact: {0}", mSelected);
            InteractWith(GetComponentInParent<Target>());
        }

        public void InteractWith(Interact.Target target) {
            // send an event to the game
            Game.Get().DidInteract(target);

            // and disable this component
            Select(null);
            enabled = false;
        }

        // -- commands/transitions
        private IEnumerator ShowPrompt(bool isVisible) {
            if (isVisible && fMode == Mode.Dynamic) {
                fPrompt.transform.position = GetDynamicAnchorPosition();
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
            var ip = transform.position;
            var vt = mCamera.transform;
            var vp = vt.position;

            // get the screen point of the item
            var screen = mCamera.WorldToScreenPoint(ip);

            // check if we're behind the camera
            if (screen.z < 0) {
                Log.Verbose("OnHover - Behind Camera: {0}", this);
                return null;
            }

            // check if we're in frame
            if (!Screen.safeArea.Contains(screen)) {
                if (fXzProximity == 0.0f) {
                    Log.Verbose("OnHover - Outside Frame: {0}", this);
                    return null;
                }

                // or if our XZ distance is really close
                var distance = Vector2.Distance(new Vector2(ip.x, ip.z), new Vector2(vp.x, vp.z));
                if (distance > fXzProximity) {
                    Log.Verbose("OnHover - Outsize Proximity: {0} Dist: {1}", this, distance);
                    return null;
                }
            }

            // check if a spherecast hits this object
            var hits = Physics.SphereCastAll(
                vp,
                fRadius,
                vt.forward,
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

            Log.Verbose("OnHover - Raycast Miss: {0}", this);
            return null;
        }

        private Vector3 GetDynamicAnchorPosition() {
            var t = mSelected.transform;
            var box = mSelected.GetComponent<BoxCollider>();

            // use the transform position if we can't find a box collider
            if (box == null) {
                return t.position + new Vector3(-0.5f, 0.5f);
            }

            // otherwise, use the box center
            var pos = box.center;
            pos = t.TransformPoint(pos);
            pos.x -= 0.5f;

            return pos;
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

        private bool HasPrompt() {
            return fPrompt != null;
        }
    }
}

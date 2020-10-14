using UnityEngine;

public class Player: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("Whether the player is locked in place.")]
    private bool fIsLocked = false;

    // -- props --
    private Vector3 mLockedPosition;

    // -- lifecycle --
    protected void Start() {
        if (IsLocked()) {
            SetLock(true);
        }
    }

    protected void LateUpdate() {
        if (IsLocked()) {
            transform.position = mLockedPosition;
        }
    }

    // -- commands --
    private void SetLock(bool isLocked) {
        fIsLocked = isLocked;

        if (isLocked) {
            mLockedPosition = transform.position;
        }

        var bob = GetComponentInChildren<HeadBob>();
        if (bob != null) {
            bob.enabled = !isLocked;
        }
    }

    // -- queries --
    private bool IsLocked() {
        return fIsLocked;
    }
}

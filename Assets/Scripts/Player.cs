using UnityEngine;

public class Player: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("Whether the player is locked in place.")]
    private bool fIsLocked = false;

    [SerializeField]
    [Tooltip("The phone in the player's inventory")]
    private GameObject fInventoryPhone = null;

    // -- props --
    private Vector3 mLockedPosition;

    // -- lifecycle --
    protected void Start() {
        Container.Get().player = this;

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
    public void PickUp(Phone phone) {
        // destroy the in-world phone
        // TODO: play "pickup" sound
        Destroy(phone.gameObject);

        // and move it to the inventory
        fInventoryPhone.SetActive(true);
    }

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

    // -- module --
    public static Player Get() {
        return Container.Get().player;
    }
}

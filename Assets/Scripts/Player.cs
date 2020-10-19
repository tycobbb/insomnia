using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Player: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("Whether the player is locked in place.")]
    private bool fIsLocked = false;

    [SerializeField]
    [Tooltip("The player's body when lying down.")]
    private GameObject fBody;

    [SerializeField]
    [Tooltip("The transform to apply to the player on standing.")]
    private Transform fSleepLoc;

    [SerializeField]
    [Tooltip("The transform to apply to the player on standing.")]
    private Transform fStandLoc;

    // -- props --
    private Vector3? mLockedPos;

    // -- lifecycle --
    protected void Awake() {
        Game.Get().Register(player: this);
    }

    protected void Start() {
        if (IsLocked()) {
            SetLock(true);
        }
    }

    protected void LateUpdate() {
        if (IsLocked() && mLockedPos != null) {
            transform.position = mLockedPos.Value;
        }
    }

    // -- commands --
    public void PickUp(Phone phone) {
        // destroy the in-world phone
        if (phone != null) {
            // TODO: play "pickup" sound
            Destroy(phone.gameObject);
        }

        // and move it to the inventory
        Inventory().PickUpPhone();
    }

    public void Sleep() {
        // show sleeping body
        fBody.SetActive(true);

        // move to bed and lock player
        Warp(fSleepLoc.position);
        Look(fSleepLoc.rotation);
        SetLock(true);
    }

    public void StandUp() {
        // hide sleeping body
        fBody.SetActive(false);

        // unlock player and move out of bed
        SetLock(false);
        Warp(fStandLoc.position);
        Look(fStandLoc.rotation);
    }

    private void Warp(Vector3 position) {
        var c = GetComponent<CharacterController>();
        c.enabled = false;
        c.transform.position = position;
        c.enabled = true;
    }

    private void Look(Quaternion rotation) {
        var v = GetComponent<MouseLook>();
        v.Rotate(rotation);
    }

    private void SetLock(bool isLocked) {
        fIsLocked = isLocked;
        mLockedPos = isLocked ? transform.position : (Vector3?)null;

        var bob = GetComponentInChildren<HeadBob>();
        if (bob != null) {
            bob.enabled = !isLocked;
        }
    }

    // -- queries --
    private bool IsLocked() {
        return fIsLocked;
    }

    // -- dependencies --
    private Inventory Inventory() {
        return GetComponent<Inventory>();
    }
}

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Player: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("Whether the player is locked in place.")]
    private bool fIsLocked = false;

    [SerializeField]
    [Tooltip("The player's resting body.")]
    private Body fBody;

    [SerializeField]
    [Tooltip("The transform to apply to the player on standing.")]
    private Transform fSleepLoc;

    [SerializeField]
    [Tooltip("The transform to apply to the player on standing.")]
    private Transform fStandLoc;

    // -- props --
    private Vector3? mLockedPos;

    // -- lifecycle --
    protected void Start() {
        if (Game.Get().IsFree()) {
            return;
        }

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
        // hide the in-world phone
        // TODO: play "pickup" sound
        phone.StartRemove();

        // and move it to the inventory
        Inventory().PickUpPhone();
    }

    public void PickUp(Food food) {
        // move in-world food into inventory
        // TODO: play "pickup" sound
        Inventory().PickUpFood(food.Selected());
    }

    public void Sleep() {
        fBody.Show();

        // move to bed and lock player
        Warp(fSleepLoc.position);
        Look(fSleepLoc.rotation);
        SetLock(true);
    }

    public void StandUp() {
        fBody.StartRemove();

        // unlock player and move out of bed
        SetLock(false);
        Warp(fStandLoc.position);
        Look(fStandLoc.rotation);
    }

    public void PickUp(Sheep sheep) {
        // hide the in-world sheep
        sheep.StartRemove();

        // and move it to the inventory
        Inventory().PickUpSheep();
    }

    public void SetLock(bool isLocked) {
        fIsLocked = isLocked;
        mLockedPos = isLocked ? transform.position : (Vector3?)null;

        var bob = GetComponentInChildren<HeadBob>();
        if (bob != null) {
            bob.enabled = !isLocked;
        }
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

    // -- queries --
    private bool IsLocked() {
        return fIsLocked;
    }

    // -- dependencies --
    private Inventory Inventory() {
        return GetComponent<Inventory>();
    }
}

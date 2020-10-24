﻿using System.Collections;
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
    [Tooltip("The player's inventory.")]
    private Inventory fInventory;

    [SerializeField]
    [Tooltip("The transform to apply to the player on standing.")]
    private Transform fSleepLoc;

    [SerializeField]
    [Tooltip("The transform to apply to the player on standing.")]
    private Transform fStandLoc;

    // -- lifecycle --
    protected void Start() {
        if (Game.Get().IsFree()) {
            return;
        }

        if (IsLocked()) {
            SetLock(true);
        }
    }

    // -- commands --
    public void PickUp(Phone phone) {
        // hide the in-world phone
        // TODO: play "pickup" sound
        phone.StartRemove();

        // and move it to the inventory
        fInventory.PickUpPhone();
    }

    public void PickUp(Sheep sheep) {
        // hide the in-world sheep
        sheep.StartRemove();

        // and move it to the inventory
        fInventory.PickUpSheep();
    }

    public void PickUp(Food food) {
        // move in-world food into inventory
        // TODO: play "pickup" sound
        fInventory.PickUpFood(food.Selected());
    }

    public void SetPhoneTime(string time) {
        fInventory.SetPhoneTime(time);
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

    public void SetLock(bool isLocked) {
        fIsLocked = isLocked;

        var c = GetComponent<CharacterController>();
        c.enabled = !isLocked;

        var bob = GetComponentInChildren<HeadBob>();
        if (bob != null) {
            bob.enabled = !isLocked;
        }
    }

    private void Warp(Vector3 position) {
        Log.Debug("Player - Warp: {0} Locked: {1}", position, fIsLocked);

        var c = GetComponent<CharacterController>();
        c.enabled = false;
        c.transform.position = position;
        c.enabled = !fIsLocked;
    }

    private void Look(Quaternion rotation) {
        var v = GetComponent<MouseLook>();
        v.Rotate(rotation);
    }

    // -- queries --
    private bool IsLocked() {
        return fIsLocked;
    }
}

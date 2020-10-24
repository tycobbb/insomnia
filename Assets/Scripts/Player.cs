using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Inventory))]
public class Player: MonoBehaviour {
    // -- constants --
    private const string kStandUpAnim = "StandUp";
    private const float kStandUpAnimDuration = 2.75f;
        
    // -- fields --
    [SerializeField]
    [Tooltip("Whether the player is locked in place.")]
    private bool fIsLocked = false;

    [SerializeField]
    [Tooltip("The player's attached body.")]
    private GameObject fBody;

    [SerializeField]
    [Tooltip("The player's resting body.")]
    private Body fFixedBody;
    
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
    public void SetPhoneTime(string time) {
        fInventory.SetPhoneTime(time);
    }
    
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

    public void Sleep() {
        // move to bed and lock player
        Warp(fSleepLoc.position);
        Look(fSleepLoc.rotation);
        SetLock(true);
        
        // snap fixed body to attached body's position
        fFixedBody.transform.position = fBody.transform.position;
        fFixedBody.Show();
    }

    public void StandUp() {
        StartCoroutine(StandUpAsync());
    }

    private IEnumerator StandUpAsync() {
        // prepare scene for animation
        fFixedBody.StartRemove();
        
        // disable mouse look
        var controls = GetComponentsInChildren<MouseLook>();
        foreach (var control in controls) {
            control.enabled = false;
        }
        
        // play the animation
        Animator().Play(kStandUpAnim);
        yield return new WaitForSeconds(kStandUpAnimDuration);
        fBody.SetActive(false);
        
        // unlock player and move out of bed
        SetLock(false); 
        Warp(fStandLoc.position);
        Look(fStandLoc.rotation);

        // re-enable mouse look
        foreach (var control in controls) {
            control.enabled = true;
        }
    }

    public void SetLock(bool isLocked) {
        fIsLocked = isLocked;

        var c = GetComponent<CharacterController>();
        c.enabled = !isLocked;

        var h = GetComponent<HeadBob>();
        h.enabled = !isLocked;
    }

    private void Warp(Vector3 position) {
        Log.Debug("Player - Warp: {0}", position);

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
    
    // -- dependencies --
    private Animator Animator() {
        return GetComponent<Animator>();
    }
}

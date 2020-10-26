using JetBrains.Annotations;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Player: MonoBehaviour {
    // -- constants --
    private const string kStandUpAnim = "StandUp";

    // -- fields --
    [SerializeField]
    [Tooltip("Whether the player is locked in place.")]
    private bool fIsLocked = false;

    [SerializeField]
    [Tooltip("The player's attached body.")]
    private GameObject fBody = null;

    [SerializeField]
    [Tooltip("The player's resting body.")]
    private Body fFixedBody = null;

    [SerializeField]
    [Tooltip("The player's inventory.")]
    private Inventory fInventory = null;

    [SerializeField]
    [Tooltip("The transform to apply to the player on standing.")]
    private Transform fSleepLoc = null;

    // -- props --
    private Animator mAnimator;
    private CharacterController mCharacter;
    private HeadBob mHeadBob;
    private MouseLook[] mControls;

    // -- lifecycle --
    protected void Awake() {
        mAnimator = GetComponent<Animator>();
        mCharacter = GetComponent<CharacterController>();
        mHeadBob = GetComponentInChildren<HeadBob>();
        mControls = GetComponentsInChildren<MouseLook>();
    }

    // -- commands --
    public void Sleep() {
        // move to bed and lock player
        Warp(fSleepLoc.position);
        SetLock(true);

        // snap fixed body to attached body's position
        fFixedBody.transform.position = fBody.transform.position;
        fFixedBody.Show();
    }

    public void SetPhoneTime(string time) {
        fInventory.SetPhoneTime(time);
    }

    public void PickUp(Phone phone) {
        // hide the in-world phone
        // TODO: play "pickup" sound
        phone.Remove();

        // and move it to the inventory
        fInventory.PickUpPhone();
    }

    public void PickUp(Sheep sheep) {
        // hide the in-world sheep
        sheep.Remove();

        // and move it to the inventory
        fInventory.PickUpSheep();
    }

    public void PickUp(Food food) {
        // move in-world food into inventory
        // TODO: play "pickup" sound
        fInventory.PickUpFood(food.Selected());

        // hide in-world food
        food.Remove();
    }

    public void StandUp(bool isAnimated = true) {
        StartCoroutine(StandUpAsync(isAnimated));
    }

    private IEnumerator StandUpAsync(bool isAnimated) {
        // disable mouse look
        foreach (var mouse in mControls) {
            mouse.enabled = false;
        }

        // recenter camera over a few frames
        const int frames = 5;
        var rotations = mControls
            .Select((look) => look.AnimationTo(Quaternion.identity))
            .ToArray();

        for (var i = 0; i < frames; i++) {
            var percent = (float) i / frames;
            yield return 0;
            foreach (var rotate in rotations) {
                rotate(percent);
            }
        }

        // prepare scene for animation
        fFixedBody.Remove();

        // play the animation
        fBody.SetActive(true);

        if (isAnimated) {
            mAnimator.Play(kStandUpAnim);
        } else {
            DidStandUp();
        }
    }

    [UsedImplicitly] // AnimationEvent
    private void DidStandUp() {
        fBody.SetActive(false);

        // unlock player and assign final position
        SetLock(false);
        Warp(transform.position);

        // re-enable mouse look
        foreach (var mouse in mControls) {
            mouse.Reset();
            mouse.enabled = true;
        }
    }

    private void SetLock(bool isLocked) {
        Log.Debug("Player - Lock: {0}", isLocked);

        fIsLocked = isLocked;
        mCharacter.enabled = !isLocked;
        mHeadBob.enabled = !isLocked;
    }

    private void Warp(Vector3 position) {
        Log.Debug("Player - Warp: {0}", position);

        var c = mCharacter;
        c.enabled = false;
        c.transform.position = position;
        c.enabled = !fIsLocked;
    }
}

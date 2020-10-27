using System;
using System.Collections;
using UnityEngine;

public class KitchenExit: MonoBehaviour {
    // -- constants --
    private const float kEnableDelay = 2.0f;
    private const Game.Step kStep = Game.Step.Exit2;

    // -- props --
    private Door mDoor;

    // -- lifecycle --
    protected void Awake() {
        mDoor = GetComponent<Door>();
    }

    protected void Update() {
        if (Game.Get().DidChangeToStep(kStep)) {
            Enable();
        }
    }

    // -- commands --
    private void Enable() {
        StartCoroutine(EnableAsync());
    }

    private IEnumerator EnableAsync() {
        yield return new WaitForSeconds(kEnableDelay);
        mDoor.Open();
    }

    // -- events --
    private void OnTriggerEnter(Collider _) {
        Game.Get().ExitKitchen();
    }
}

using System;
using System.Collections;
using UnityEngine;

public class Phone: MonoBehaviour, Interact.Target {
    // -- constants --
    private const Game.Step kStep = Game.Step.Phone;
    private const float kEnableDelay = 2.0f;

    // -- props --
    private PhoneScreen mScreen;
    private Interact.OnHover mHover;

    // -- lifecycle --
    protected void Awake() {
        mScreen = GetComponentInChildren<PhoneScreen>();
        mHover = GetComponent<Interact.OnHover>();
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

        mHover.Reset();
        mScreen.TurnOn();
    }

    public void Remove() {
        StartCoroutine(RemoveAsync());
    }

    private IEnumerator RemoveAsync() {
        yield return mHover.Transition();
        gameObject.SetActive(false);
    }
}

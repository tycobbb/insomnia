using System;
using System.Collections;
using UnityEngine;

public class Sheep: MonoBehaviour, Interact.Target {
    // -- constants --
    private const Game.Step kStep = Game.Step.Sheep;

    // -- props --
    private Rigidbody mBody;
    private Interact.OnHover mHover;

    // -- lifecycle --
    protected void Awake() {
        mBody = GetComponent<Rigidbody>();
        mHover = GetComponent<Interact.OnHover>();
    }

    protected void Update() {
        if (Game.Get().DidChangeToStep(kStep)) {
            Enable();
        }
    }

    // -- commands --
    private void Enable() {
        mHover.Reset();
        StartCoroutine(Hop());
    }

    private IEnumerator Hop() {
        while (mHover.enabled) {
            yield return new WaitForSeconds(3.0f);
            mBody.velocity = Vector3.up * 5.0f;
        }
    }

    public void Remove() {
        StartCoroutine(RemoveAsync());
    }

    private IEnumerator RemoveAsync() {
        yield return mHover.Transition();
        gameObject.SetActive(false);
    }
}

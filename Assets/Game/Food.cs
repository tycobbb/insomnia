using System.Collections;
using UnityEngine;

public class Food: MonoBehaviour, Interact.Target {
    // -- constants --
    private const Game.Step kStep = Game.Step.Food;

    // -- props --
    private Interact.OnHover mHover;

    // -- lifecycle --
    protected void Start() {
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
    }

    public void Remove() {
        StartCoroutine(RemoveAsync());
    }

    private IEnumerator RemoveAsync() {
        yield return mHover.Transition();
    }

    // -- queries --
    public GameObject Selected() {
        return mHover.Selected();
    }
}

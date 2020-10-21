using System.Collections;
using UnityEngine;

public class Food: MonoBehaviour, Interact.Target {
    // -- lifecycle --
    protected void Update() {
        // enable hover on door step
        if (Game.Get().DidChangeToStep(Game.Step.Food)) {
            Hover().Reset();
        }
    }

    // -- commands --
    public void StartRemove() {
        StartCoroutine(Remove());
    }

    private IEnumerator Remove() {
        yield return Hover().Transition();
    }

    // -- queries --
    public GameObject Selected() {
        return Hover().Selected();
    }

    // -- Interact.Target --
    public Interact.OnHover Hover() {
        return GetComponent<Interact.OnHover>();
    }
}

using System.Collections;
using UnityEngine;

public class Body: MonoBehaviour, Interact.Target {
    // -- lifecycle --
    protected void Update() {
        // enable hover on foot step
        if (Game.Get().DidChangeToStep(Game.Step.Foot1 | Game.Step.Foot2 | Game.Step.Foot3)) {
            Hover().Reset();
        }
    }

    // -- commands --
    public void Show() {
        gameObject.SetActive(true);
    }

    public void Remove() {
        StartCoroutine(RemoveAsync());
    }

    private IEnumerator RemoveAsync() {
        yield return Hover().Transition();
        gameObject.SetActive(false);
    }

    // -- Interact.Target --
    public Interact.OnHover Hover() {
        return GetComponentInChildren<Interact.OnHover>();
    }
}

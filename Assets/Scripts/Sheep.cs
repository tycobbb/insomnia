using System.Collections;
using UnityEngine;

public class Sheep: MonoBehaviour, Interact.Target {
    // -- lifecycle --
    protected void Update() {
        // enable hover on door step
        if (Game.Get().DidChangeToStep(Game.Step.Sheep)) {
            Hover().Reset();
        }
    }

    // -- commands --
    public void Remove() {
        StartCoroutine(RemoveAsync());
    }

    private IEnumerator RemoveAsync() {
        yield return Hover().Transition();
        gameObject.SetActive(false);
    }

    // -- Interact.Target --
    public Interact.OnHover Hover() {
        return GetComponent<Interact.OnHover>();
    }
}

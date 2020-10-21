using System.Collections;
using UnityEngine;

public class Phone: MonoBehaviour, Interact.Target {
    // -- lifecycle --
    protected void Update() {
        // enable hover on phone step
        if (Game.Get().DidChangeToStep(Game.Step.Phone)) {
            Debug.LogFormat("Enable phone");
            Hover().Reset();
        }
    }

    // -- commands --
    public void StartRemove() {
        StartCoroutine(Remove());
    }

    private IEnumerator Remove() {
        yield return Hover().Transition();
        gameObject.SetActive(false);
    }

    // -- Interact.Target --
    public Interact.OnHover Hover() {
        return GetComponent<Interact.OnHover>();
    }
}

using System.Collections;
using UnityEngine;

public class Phone: MonoBehaviour, Interact.Target {
    // -- fields --
    [SerializeField]
    [Tooltip("The phone screen.")]
    private PhoneScreen fScreen;

    // -- lifecycle --
    protected void Update() {
        // enable hover on phone step
        if (Game.Get().DidChangeToStep(Game.Step.Phone)) {
            Enable();
        }
    }

    // -- commands --
    private void Enable() {
        Hover().Reset();
        fScreen.TurnOn();
    }

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

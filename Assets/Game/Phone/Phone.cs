using System.Collections;
using UnityEngine;

public class Phone: MonoBehaviour, Interact.Target {
    // -- constants --
    private const float kEnableDelay = 2.0f;

    // -- fields --
    [SerializeField]
    [Tooltip("The phone's screen.")]
    private PhoneScreen fScreen;

    [SerializeField]
    [Tooltip("The notification sound.")]
    private AudioSource fNotification;

    // -- lifecycle --
    protected void Update() {
        // enable hover on phone step
        if (Game.Get().DidChangeToStep(Game.Step.Phone)) {
            Enable();
        }
    }

    // -- commands --
    private void Enable() {
        StartCoroutine(EnableAsync());
    }

    private IEnumerator EnableAsync() {
        yield return new WaitForSeconds(kEnableDelay);

        Hover().Reset();
        fScreen.TurnOn();
        fNotification.Play();
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
        return GetComponent<Interact.OnHover>();
    }
}

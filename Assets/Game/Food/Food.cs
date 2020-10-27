using System.Collections;
using UnityEngine;

public class Food: MonoBehaviour, Interact.Target {
    // -- fields --
    [SerializeField]
    [Tooltip("The food game step.")]
    private Game.Step fStep = Game.Step.Food2;

    [SerializeField]
    [Tooltip("If the food should face up top-forward when eaten")]
    private bool fIsFacingUp = false;

    // -- props --
    private Interact.OnHover mHover;

    // -- lifecycle --
    protected void Start() {
        mHover = GetComponent<Interact.OnHover>();
    }

    protected void Update() {
        if (Game.Get().DidChangeToStep(fStep)) {
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
    public bool IsFacingUp() {
        return fIsFacingUp;
    }

    public GameObject Selected() {
        var selected = mHover.Selected();
        if (selected != null) {
            return selected;
        }

        return gameObject;
    }
}

using UnityEngine;

public class FieldExit: MonoBehaviour {
    // -- constants --
    private const Game.Step kStep = Game.Step.Exit1;

    // -- props --
    private Door mDoor;

    // -- lifecycle --
    protected void Awake() {
        mDoor = GetComponentInChildren<Door>();
    }

    protected void Update() {
        if (Game.Get().DidChangeToStep(kStep)) {
            Enable();
        }
    }

    // -- commands --
    private void Enable() {
        mDoor.Open();
    }

    // -- events --
    private void OnTriggerEnter(Collider _) {
        Game.Get().ExitField();
    }
}


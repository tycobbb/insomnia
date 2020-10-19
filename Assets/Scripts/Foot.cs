using UnityEngine;

public class Foot: MonoBehaviour, Interact.Target {
    // -- lifecycle --
    protected void Update() {
        var game = Game.Get();

        // enable hover
        if (game.NewStep() == Game.Step.Foot) {
            GetComponent<Interact.OnHover>().enabled = true;
        }
    }

    // -- Interact.Target --
    public void OnInteract() {
        Game.Get().StandUp();
    }
}

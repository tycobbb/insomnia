using UnityEngine;

public class Phone: MonoBehaviour, Interact.Target {
    // -- Interact.Target --
    public void OnInteract() {
        Player.Get().PickUp(this);
    }
}

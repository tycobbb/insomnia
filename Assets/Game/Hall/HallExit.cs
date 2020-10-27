using UnityEngine;

public class HallExit: MonoBehaviour {
    // -- events --
    private void OnTriggerEnter(Collider _) {
        Game.Get().ExitHall();
    }
}

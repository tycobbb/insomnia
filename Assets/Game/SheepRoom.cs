using UnityEngine;

public class SheepRoom: MonoBehaviour {
    // -- commands --
    private void Enter() {
        Game.Get().EnterSheepRoom();
    }

    // -- events --
    protected void OnTriggerEnter(Collider _) {
        Enter();
    }
}

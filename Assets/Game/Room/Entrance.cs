using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Entrance: MonoBehaviour {
    protected void OnTriggerEnter(Collider _) {
        Game.Get().DidStartEnterRoom(GetComponentInParent<Room>());
    }

    protected void OnTriggerExit(Collider _) {
        Game.Get().DidFinishEnterRoom(GetComponentInParent<Room>());
        enabled = false;
    }
}

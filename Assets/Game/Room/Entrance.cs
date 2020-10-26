using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Entrance: MonoBehaviour {
    protected void OnTriggerEnter(Collider _) {
        Game.Get().DidEnter(GetComponentInParent<Room>());
    }
}

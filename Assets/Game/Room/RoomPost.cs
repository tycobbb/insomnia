using UnityEngine;

public class RoomPost: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("The collider for the full, unblended volume.")]
    private Collider fFull = null;

    // -- commands --
    public void SetBlended(bool isBlended) {
        fFull.enabled = !isBlended;
    }
}

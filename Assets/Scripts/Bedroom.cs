using UnityEngine;

public class Bedroom: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("The bedroom door.")]
    private GameObject fDoor;

    [SerializeField]
    [Tooltip("The entrance door to the sheep room.")]
    private GameObject fSheepDoor;

    // -- lifecycle --
    protected void Awake() {
        Game.Get().Register(bedroom: this);
    }

    // to mask out the house
    // protected void Start() {
    //     var renderers = GetComponentsInChildren<Renderer>();

    //     foreach (var renderer in renderers) {
    //         if (renderer.material.renderQueue < 2001 && renderer.gameObject != fDoor) {
    //             renderer.material.renderQueue = 9001;
    //         }
    //     }
    // }

    // -- commands --
    public void WarpToSheep() {
        WarpToDoor(fSheepDoor);
    }

    private void WarpToDoor(GameObject door) {
        // calculate position based on the new door's position
        var position = door.transform.position;
        // offset by the bedroom door's local position
        position -= fDoor.transform.localPosition;
        // and center the door
        position -= new Vector3(0.5f, 0.0f, 0.0f);

        // move room and hide door
        door.SetActive(false);
        transform.position = position;
    }
}

using UnityEngine;

public class Kitchen: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("The wall between the kitchen and bedroom.")]
    private GameObject kDoorWall;

    // -- commands --
    private void Enter() {
        kDoorWall.SetActive(true);
        Game.Get().EnterKitchen();
    }

    // -- events --
    protected void OnTriggerEnter(Collider collider) {
        Enter();
    }
}

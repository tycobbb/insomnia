using UnityEngine;

public class Kitchen: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("The wall between the kitchen and bedroom.")]
    private GameObject fDoorWall;

    // -- commands --
    private void Enter() {
        fDoorWall.SetActive(true);
        Game.Get().EnterKitchen();
    }

    // -- events --
    protected void OnTriggerEnter(Collider _) {
        Enter();
    }
}

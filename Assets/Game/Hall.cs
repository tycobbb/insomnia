using UnityEngine;

public class Hall: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("The wall between the hall and bedroom.")]
    private GameObject fDoorWall = null;

    // -- commands --
    private void Enter() {
        fDoorWall.SetActive(true);
        Game.Get().EnterHall();
    }

    // -- events --
    protected void OnTriggerEnter(Collider _) {
        Enter();
    }
}

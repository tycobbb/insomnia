using UnityEngine;

public class Inventory: MonoBehaviour {
    // -- constants --
    private const string kShowPhone = "ShowPhone";

    // -- fields --
    [SerializeField]
    [Tooltip("The player's phone")]
    private GameObject fInventoryPhone = null;

    // -- comands --
    public void PickUpPhone() {
        fInventoryPhone.SetActive(true);
        Animator().Play(kShowPhone);
    }

    // -- dependencies --
    private Animator Animator() {
        return GetComponentInChildren<Animator>();
    }
}

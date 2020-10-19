using UnityEngine;

public class Inventory: MonoBehaviour {
    // -- constants --
    private const string kShowPhoneAnim = "ShowPhone";

    // -- fields --
    [SerializeField]
    [Tooltip("The player's phone")]
    private GameObject fInventoryPhone = null;

    // -- comands --
    public void PickUpPhone() {
        fInventoryPhone.SetActive(true);
        Animator().Play(kShowPhoneAnim);
    }

    // -- dependencies --
    private Animator Animator() {
        return GetComponentInChildren<Animator>();
    }
}

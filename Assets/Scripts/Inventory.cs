using UnityEngine;

public class Inventory: MonoBehaviour {
    // -- constants --
    private const string kShowPhoneAnim = "ShowPhone";
    private const string kShowSheepAnim = "ShowSheep";

    // -- fields --
    [SerializeField]
    [Tooltip("The player's phone")]
    private GameObject fInventoryPhone = null;

    [SerializeField]
    [Tooltip("The player's sheep")]
    private GameObject fInventorySheep = null;

    // -- comands --
    public void PickUpPhone() {
        fInventoryPhone.SetActive(true);
        Animator().Play(kShowPhoneAnim);
    }

    public void PickUpSheep() {
        fInventorySheep.SetActive(true);
        Animator().Play(kShowSheepAnim);
    }

    // -- dependencies --
    private Animator Animator() {
        return GetComponentInChildren<Animator>();
    }
}

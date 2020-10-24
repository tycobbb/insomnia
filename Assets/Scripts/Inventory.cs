using UnityEngine;

public class Inventory: MonoBehaviour {
    // -- constants --
    private const string kShowPhoneAnim = "ShowPhone";
    private const string kShowSheepAnim = "ShowSheep";
    private const string kShowFoodAnim = "ShowFood";

    // -- fields --
    [SerializeField]
    [Tooltip("The player's phone")]
    private GameObject fInventoryPhone;

    [SerializeField]
    [Tooltip("The player's sheep")]
    private GameObject fInventorySheep;

    [SerializeField]
    [Tooltip("The player's food")]
    private GameObject fInventoryFood;

    [SerializeField]
    [Tooltip("The position of the player's food item")]
    private Transform fFoodPos;

    [SerializeField]
    [Tooltip("The player's phone's time.")]
    private PhoneTime fPhoneTime;

    // -- comands --
    public void SetPhoneTime(string time) {
        fPhoneTime.Set(time);
    }

    public void PickUpPhone() {
        fInventoryPhone.SetActive(true);
        Animator().Play(kShowPhoneAnim);
    }

    public void PickUpSheep() {
        fInventorySheep.SetActive(true);
        Animator().Play(kShowSheepAnim);
    }

    public void PickUpFood(GameObject item) {
        fInventoryFood.SetActive(true);

        // nest food in the inventory slot
        var t = item.transform;
        t.parent = fInventoryFood.transform;

        // move it to the pre-baked position
        var p = fFoodPos;
        t.position = p.position;
        t.rotation = p.rotation;
        t.localScale *= 0.5f;

        Animator().Play(kShowFoodAnim);
    }

    // -- dependencies --
    private Animator Animator() {
        return GetComponentInChildren<Animator>();
    }
}

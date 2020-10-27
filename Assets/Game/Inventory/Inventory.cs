using UnityEngine;
using UnityEngine.Serialization;

public class Inventory: MonoBehaviour {
    // -- constants --
    private const string kShowPhoneAnim = "ShowPhone";
    private const string kShowSheepAnim = "ShowSheep";
    private const string kShowFoodAnim = "ShowFood";

    // -- fields --
    [SerializeField]
    [Tooltip("The player's phone")]
    private GameObject fPhone = null;

    [SerializeField]
    [Tooltip("The player's sheep")]
    private GameObject fSheep = null;

    [SerializeField]
    [Tooltip("The player's food")]
    private GameObject fFood = null;

    [SerializeField]
    [Tooltip("The player's phone's screen.")]
    private PhoneScreen fPhoneScreen = null;

    // -- props --
    private Animator mAnimator;

    // -- lifecycle --
    protected void Start() {
        mAnimator = GetComponent<Animator>();
    }

    // -- commands --
    public void SetPhoneTime(string time) {
        fPhoneScreen.SetTime(time);
    }

    public void PickUpPhone() {
        fPhone.SetActive(true);
        mAnimator.Play(kShowPhoneAnim);
    }

    public void PickUpSheep() {
        fSheep.SetActive(true);
        mAnimator.Play(kShowSheepAnim);
    }

    public void PickUpFood(GameObject item) {
        fFood.SetActive(true);

        // nest food in the inventory slot
        var tf = fFood.transform;
        var ti = item.transform;
        ti.parent = tf;

        // move it to the pre-baked position
        ti.localPosition = Vector3.zero;
        ti.forward = tf.forward;
        ti.localScale *= 0.5f;

        mAnimator.Play(kShowFoodAnim);
    }

    public void Clear() {
        gameObject.SetActive(false);
    }
}

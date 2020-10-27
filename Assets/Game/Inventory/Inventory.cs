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
    private int mFoodEaten = 0;

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

    public void PickUpFood(Food food) {
        fFood.SetActive(true);

        // nest food in the inventory slot
        var tf = fFood.transform;
        var ti = food.Selected().transform;
        ti.parent = tf;

        // i: 0  1  2  3  4 ...
        // o: 0  1 -1  2 -2 ...
        // move it to the pre-baked position (oscillate around center)
        var i = mFoodEaten;
        var o = (i + 1) / 2 * (int)((i % 2 - 0.5f) * 2);
        ti.localPosition = new Vector3(Mathf.Clamp(o, -4, 4) * 0.04f, 0.0f);
        ti.localScale *= 0.33f;

        if (food.IsFacingUp()) {
            ti.up = tf.forward;
        } else {
            ti.forward = tf.forward;
        }


        if (mFoodEaten == 0) {
            mAnimator.Play(kShowFoodAnim);
        }

        mFoodEaten++;
    }

    public void TrimFood() {
        var tf = fFood.transform;

        // remove all but the first five food items
        for (var i = tf.childCount - 1; i > 4; i--) {
            Destroy(tf.GetChild(i).gameObject);
        }
    }

    public void Clear() {
        gameObject.SetActive(false);
    }

    // -- queries --
    public int FoodEaten() {
        return mFoodEaten;
    }
}

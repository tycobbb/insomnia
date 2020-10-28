using UnityEngine;

public class HallExit: MonoBehaviour {
    // -- constants --
    private static readonly int kAlbedoColorId = Shader.PropertyToID("_AlbedoColor");
    private static readonly int kEmissionColorId = Shader.PropertyToID("_EmissionColor");

    // -- fields --
    [SerializeField]
    [Tooltip("The minimum distance before the exit starts color shifting.")]
    private float fThreshold = 100.0f;

    // -- props --
    private Material mMaterial;
    private Light[] mSpotlights;

    // -- lifecycle --
    protected void Awake() {
        mMaterial = GetComponent<Renderer>().material;
        mSpotlights = GetComponentsInChildren<Light>();
    }

    private void Update() {
        // calculate distance to player
        var pos = Game.Get().GetPlayerPos();
        var distance = Vector3.Distance(transform.position, pos);

        // calculate percent past threshold
        var percent = 1 - Mathf.Min(distance, fThreshold) / fThreshold;

        // update light colors
        if (!Mathf.Approximately(percent, 0.0f)) {
            var color = Color.white + Color.yellow * percent;

            mMaterial.SetColor(kAlbedoColorId, color);
            mMaterial.SetColor(kEmissionColorId, color);

            foreach (var spotlight in mSpotlights) {
                spotlight.color = color;
            }
        }
    }

    // -- events --
    private void OnTriggerEnter(Collider _) {
        Game.Get().ExitHall();
    }
}

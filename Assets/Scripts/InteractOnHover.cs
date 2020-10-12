using System;
using UnityEngine;
using TMPro;

public class InteractOnHover: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("The text to display in the prompt.")]
    private string f_Prompt = "";

    [SerializeField]
    [Tooltip("The minimum distance before showing the tooltip.")]
    private float f_MinDistance = 20.0f;

    // -- props --
    private GameObject m_Prompt;

    // -- lifecycle --
    void Start() {
        m_Prompt = Instantiate(LoadPrefab(), transform);
        m_Prompt.GetComponent<TextMeshPro>().text = f_Prompt;
    }

    void Update() {
        var isVisible = IsInView();

        // only show the prompt when in view
        m_Prompt.SetActive(isVisible);

        // if visible, make sure the prompt faces the camera
        if (isVisible) {
            m_Prompt.transform.forward = MainCamera().transform.forward;
        }
    }

    // -- queries --
    private bool IsInView() {
        var camera = MainCamera();
        var screen = camera.WorldToScreenPoint(transform.position);

        // check if were behind camera
        if (screen.z < 0) {
            return false;
        }

        // check if were in frame
        if (!Screen.safeArea.Contains(screen)) {
            return false;
        }

        // check if the a raycast hits this object
        RaycastHit contact;
        var source = camera.transform;
        var didHit = Physics.Raycast(source.position, source.forward, out contact, f_MinDistance);

        if (!didHit || contact.transform != transform) {
            return false;
        }

        return true;
    }

    private Camera MainCamera() {
        return Camera.main;
    }

    // -- factories --
    private static GameObject LoadPrefab() {
        return Resources.Load<GameObject>("Prompt");
    }
}

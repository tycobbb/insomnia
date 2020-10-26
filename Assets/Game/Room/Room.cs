using UnityEngine;

public interface Room {
    // -- commands --
    void SetActive(bool isActive);

    // -- queries --
    GameObject Door();
}

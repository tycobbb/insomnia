using UnityEngine;

public interface Room {
    // -- commands --
    void SetActive(bool isActive);
    void EnterStart();
    void EnterEnd();

    // -- queries --
    GameObject Door();
}

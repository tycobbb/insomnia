using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 기능

public class DonDestroy : MonoBehaviour
{
    // 사라지지 않을 오브젝트
    public GameObject donDestroy;

    private void Awake()
    {
        DontDestroyOnLoad(donDestroy);
    }
}

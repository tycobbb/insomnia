using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUp : MonoBehaviour
{
    // 일어나는 동안 회전하도록 bool값 생성
    bool isWakeUp;

    void Start()
    {
        // 1초 뒤에 일어나기
        Invoke("WakeUpStart", 0.5f);
    }

    // 일어나기 시작
    void WakeUpStart()
    {
        // 일어나기
        isWakeUp = true;

        // 2초 뒤에 일어나기 끝
        Invoke("WakeUpStop", 2f);
    }

    // 일어나기 끝
    void WakeUpStop()
    {
        // 일어나기 그만
        isWakeUp = false;

        // 각도가 완벽하게 0이 되기 전 일어나기를 끝내기 때문에 완벽하게 0으로 맞춰주기
        transform.localEulerAngles = new Vector3(0, 0, 0);

        // 꺼놨던 중력 다시 작동 (0 -> 10)
        GetComponent<FirstPersonDrifter>().gravity = 10;

        // 플레이어 속 꺼놨던 MouseLook 다시 작동
        GetComponent<MouseLook>().enabled = true;

        // 메인카메라 속 꺼놨던 MouseLook 다시 작동
        transform.GetChild(0).GetComponent<MouseLook>().enabled = true;
    }

    void Update()
    {
        // 일어날 때
        if (isWakeUp)
        {
            // x회전값을 서서히 이동
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(360, 0, 0), Time.deltaTime * 2);
        }
    }
}

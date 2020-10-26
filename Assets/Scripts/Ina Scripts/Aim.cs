using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 사용하기 위함
using UnityEngine.UI; // Image를 사용하기 위함


public class Aim : MonoBehaviour
{
    RaycastHit hit;

    // 화면 가운데 점 (에임) 이미지
    public Image aim;

    // 아이템 슬롯 스크립트
    public ItemSlot itemSlot_script;

    private void Update()
    {
        // 카메라의 정 가운데에서부터 레이캐스트 발사 + 거리는 2만큼만 + (물건레이어(레이어9)만 감지)
        if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2)),
            out hit, 2f, 1 << 9))
        {
            // 집을 수 있다는 표시로 Aim의 색상 변경 (주황색)
            aim.color = new Vector4(1, 0.7f, 0, 1);

            // 마우스 좌클릭을 했다면
            if (Input.GetMouseButtonDown(0))
            {
                // 문이라면
                if(hit.transform.gameObject.tag == "Door")
                {
                    // 양 있는 씬으로 이동
                    SceneManager.LoadScene("Scene_Sheep");
                }

                // 문이 아니라면 (= 아이템)
                else
                {
                    // 인벤토리에 이미지 넣는 함수 호출 (집은 물건과 같은 이름의 이미지를 가져올 거라서 이름 전달)
                    itemSlot_script.ItmeInput(hit.transform.gameObject.name);

                    // 물건 삭제
                    Destroy(hit.transform.gameObject);
                }
            }
        }

        // 감지 된게 없다면
        else
        {
            // 집을 수 없다는 표시로 Aim의 색상 변경 (하얀색)
            aim.color = new Vector4(1, 1, 1, 1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    // 슬롯창_애니메이션용
    public RectTransform slot_anim;

    // 슬롯창
    public Transform[] slots;

    // 아이템을 들고 있을 자리
    public Transform hand;

    // 슬롯 선택 표시 이미지
    public RectTransform slotSelect;

    // 슬롯 선택 표시 이미지의 x포지션 (디폴트는 30)
    float select_x = 30;

    // 선택된 슬롯의 상태
    int slotState, beforeState;

    // 아이템이 처음 들어왔다면
    bool isFirst = true;

    // 애니메이션을 할건지
    bool isSlotAnim;

    // 애니메이션 종류
    bool isDownUp, isUp, isDown;






    // --- 슬롯에 순서대로 아이템 여러개 넣는 기능 --- //
    // 집은 아이템 슬롯에 넣기
    public void ItmeInput(string itmeName)
    {
        // 아이템 집은 게 처음이라면
        if (isFirst)
        {
            // 슬롯창 올라오게 (update에서 실행해야 함)
            isSlotAnim = true;
        }

        // 슬롯의 개수만큼 반복해서 검사
        for (int i = 0; i < slots.Length; i++)
        {
            // 슬롯에게 자식이 없다면 (= 아이템이 채워지지 않은 슬롯이라면)
            if (slots[i].childCount == 0)
            {
                // 리소스폴더 속 Item_Image 폴더에 있는 이미지를(집은 물건과 이름이 같은) 슬롯창에 가져오기
                Instantiate(Resources.Load("Item_Image/" + itmeName), slots[i]);

                // 리소스폴더 속 Item 폴더에 있는 프리팹을(집은 물건과 이름이 같은) 가져와서 손에 붙여두기
                GameObject item = Instantiate(Resources.Load("Item/" + itmeName), hand) as GameObject;

                // 슬롯 선택 표시 이미지를 아이템 저장한 슬롯 창으로 이동 (바로 손에 들 수 있도록)
                select_x = 30 + (i * 60);

                // 원래 상태를 이전 상태로 바꾸고
                beforeState = slotState;

                // 새로운 상태 저장
                slotState = i;

                // 손 애니메이션 실행
                HandCtrl();

                // 아이템 넣었으면 for문 끝내기 (i가 하나씩 늘어나다가 slots.Length - 1이 되면 끝나므로 강제로 slots.Length - 1로 만들어서 끝냄)
                i = slots.Length - 1;
            }
        }
    }
    // --- 슬롯에 순서대로 아이템 여러개 넣는 기능 --- //







    private void Update()
    {
        // 슬롯창 올라오는 애니메이션 (-60 -> 0)
        if (isSlotAnim)
        {
            slot_anim.anchoredPosition = Vector3.Lerp(slot_anim.anchoredPosition, new Vector3(50, 0, 0), Time.deltaTime * 10);
        }






        // --- 스크롤로 아이템 선택하는 기능 --- //
        // 휠 값이 0이 아니라면 (= 휠을 돌리면)
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            // 슬롯 선택 표시이미지 x포지션 이동
            select_x -= Input.GetAxis("Mouse ScrollWheel") * 10 * 60;

            // 원래 상태를 이전 상태로 바꾸고
            beforeState = slotState;

            // 새로운 상태 저장 (0 ~ 4)
            slotState -= (int)(Input.GetAxis("Mouse ScrollWheel") * 10);

            // 상태가 0 이하가 되면 0
            if (slotState <= 0)
            {
                slotState = 0;
            }
            // 상태가 4 이상이 되면 4
            if(slotState >= 4)
            {
                slotState = 4;
            }

            // 손에 물건이 하나라도 있다면
            if (hand.childCount != 0)
            {
                // 슬롯 선택 상태가 이전과 달라졌다면
                if (beforeState != slotState)
                {
                    HandCtrl();
                }
            }
        }
        
        // x포지션의 제한두기 : 30 ~ 270
        select_x = Mathf.Clamp(select_x, 30, 270);

        // 슬롯 선택 표시이미지의 x포지션에 select_x 넣기
        slotSelect.anchoredPosition = new Vector2(Mathf.Clamp(select_x, 30, 270), slotSelect.anchoredPosition.y);
        // --- 스크롤로 아이템 선택하는 기능 --- //






        // --- 손 애니메이션 종류--- //
        if (isDownUp)
        {
            // 이전 물건 내려가기
            hand.GetChild(beforeState).localPosition = Vector3.Lerp(hand.GetChild(beforeState).localPosition, new Vector3(0, -0.2f, 0), Time.deltaTime * 10);

            // 어느정도 내려갔다면
            if (hand.GetChild(beforeState).transform.localPosition.y <= -0.15f)
            {
                // 다음 물건 올라오기 (다음 물건이 있는 경우에만)
                hand.GetChild(slotState).gameObject.SetActive(true);
                hand.GetChild(slotState).localPosition = Vector3.Lerp(hand.GetChild(slotState).localPosition, new Vector3(0, 0, 0), Time.deltaTime * 10);
            }
        }

        // 이전 물건 내려가기 (이전 물건이 있는 경우에만)
        if (isDown)
        {
            hand.GetChild(beforeState).localPosition = Vector3.Lerp(hand.GetChild(beforeState).localPosition, new Vector3(0, -0.2f, 0), Time.deltaTime * 10);
        }

        // 다음 물건 올라오기 (다음 물건이 있는 경우에만)
        if (isUp)
        {
            hand.GetChild(slotState).gameObject.SetActive(true);
            hand.GetChild(slotState).localPosition = Vector3.Lerp(hand.GetChild(slotState).localPosition, new Vector3(0, 0, 0), Time.deltaTime * 10);
        }
        // --- 손 애니메이션 종류--- //
    }


    // 손 애니메이션 On/Off 관리
    void HandCtrl()
    {
        // 처음이라면
        if (isFirst)
        {
            // 올라오기
            isDownUp = false;
            isDown = false;
            isUp = true;

            isFirst = false;
        }
        else
        {
            // 아이템이 있는 슬롯
            if (hand.childCount > beforeState)
            {
                // -> 아이템이 있는 슬롯
                if (hand.childCount > slotState)
                {
                    // 내려갔다가 올라가기
                    isDown = false;
                    isUp = false;
                    isDownUp = true;
                }
                // -> 아이템이 없는 슬롯
                else
                {
                    // 내려가기
                    isDownUp = false;
                    isUp = false;
                    isDown = true;
                }
            }
            // 아이템이 없는 슬롯
            else
            {
                // -> 아이템이 있는 슬롯
                if (hand.childCount > slotState)
                {
                    // 올라오기
                    isDownUp = false;
                    isDown = false;
                    isUp = true;
                }
                // -> 아이템이 없는 슬롯
                else
                {
                    // 아무일도X
                    isDownUp = false;
                    isDown = false;
                    isUp = false;
                }
            }
        }
    }
}

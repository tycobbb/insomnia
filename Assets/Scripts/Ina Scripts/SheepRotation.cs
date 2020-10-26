using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepRotation : MonoBehaviour
{
    Transform player;
    GameObject sheep_model;

    public float seeDistance = 5f; // 양이 보이기 시작할 범위
    public float turnSpeed = 4f; // 양이 보이기 시작할 범위

    private void Start()
    {
        // 플레이어의 트랜스폼 정보 저장
        player = Camera.main.transform;

        // 양의 첫번째 자식(Base 61) 저장
        sheep_model = transform.GetChild(0).gameObject;
    }

    protected void Update()
    {
        // 양과 플레이어의 거리가 distance 이하라면
        if (Vector3.Distance(transform.position, player.position) <= seeDistance)
        {
            // 양 보이도록
            sheep_model.SetActive(true);

            // 플레이어를 쳐다볼 때 Y축으로만 회전하도록 고정 (타겟의 y포지션을 양과 같게 하면 y축으로는 회전 안 함)
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);

            // 양과 플레이어사이의 거리 구하기
            Vector3 vec = targetPosition - transform.position;

            // 구한 거리를 방향 벡터로 정규화시키기
            vec.Normalize();

            // 방향벡터의 방향으로 회전값 저장
            Quaternion q = Quaternion.LookRotation(vec);

            // 저장한 회전값으로 서서히 회전
            transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * turnSpeed);
        }
    }
}

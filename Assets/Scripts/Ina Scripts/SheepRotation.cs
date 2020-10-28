using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepRotation : MonoBehaviour
{
    Transform player;
    GameObject sheep_model;

    public float activeDistance = 7f;
    public float seeDistance = 5f; // 양이 보이기 시작할 범위
    public float turnSpeed = 4f; // 양이 보이기 시작할 범위
    public bool hasCollisions = true;
    public bool hidesOnceActive = true;

    private void Start()
    {
        // 플레이어의 트랜스폼 정보 저장
        player = Camera.main.transform;

        // 양의 첫번째 자식(Base 61) 저장
        sheep_model = transform.GetChild(0).gameObject;

        // make sure the models start hidden
        sheep_model.SetActive(false);

        // enable the collider if necessary
        var collider = sheep_model.GetComponent<BoxCollider>();
        if (collider != null) {
            collider.enabled = hasCollisions;
        }
    }

    protected void Update() {
        Vector3 sheepPosition = transform.position;
        Vector3 playerPosition = player.position;

        // get the distance between the sheep and player
        float distance = Vector3.Distance(sheepPosition, playerPosition);

        // if the player is close, show the sheep
        bool isActive = distance <= activeDistance;
        if (isActive || hidesOnceActive) {
            // 양 보이도록
            sheep_model.SetActive(isActive);
        }

        // 양과 플레이어의 거리가 distance 이하라면
        if (distance <= seeDistance)
        {
            // 플레이어를 쳐다볼 때 Y축으로만 회전하도록 고정 (타겟의 y포지션을 양과 같게 하면 y축으로는 회전 안 함)
            Vector3 targetPosition = new Vector3(playerPosition.x, sheepPosition.y, playerPosition.z);

            // 양과 플레이어사이의 거리 구하기
            Vector3 vec = targetPosition - sheepPosition;

            // 구한 거리를 방향 벡터로 정규화시키기
            vec.Normalize();

            // 방향벡터의 방향으로 회전값 저장
            Quaternion q = Quaternion.LookRotation(vec);

            // 저장한 회전값으로 서서히 회전
            transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * turnSpeed);
        }
    }
}

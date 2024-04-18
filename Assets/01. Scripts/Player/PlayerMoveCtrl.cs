using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCtrl : MonoBehaviour
{
    [Header("[# Move Var Header]")]
    [SerializeField] private int moveSpeed;
    [SerializeField] private Vector2 moveVec;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        // 키를 눌러서 방향 받아오기
        float dirVec = Input.GetAxis("Horizontal");
        if(dirVec == 0) return;

        // 입력받은 방향으로 이동하기
        moveVec.x = dirVec * moveSpeed * Time.deltaTime;
        transform.Translate(moveVec);
    }
}
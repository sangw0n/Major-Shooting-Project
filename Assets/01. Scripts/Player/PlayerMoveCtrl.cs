using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCtrl : MonoBehaviour
{
    [Header("[# Move Var Header]")]
    [SerializeField] private int moveSpeed;
    private float inputMoveDir;
    private Vector2 moveVec;

    // Object :: Component
    private Animator anim;

    // Animator :: Hash Value
    private readonly int hashIsMove = Animator.StringToHash("isMove");
    private readonly int hashMove = Animator.StringToHash("Move");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }

    private void LateUpdate()
    {
        // Animator :: 설정 

        // inputMoveDir 값이 0과 거의 동일하면 애니메이션 비활성화
        if (Mathf.Approximately(inputMoveDir, 0)) anim.SetBool(hashIsMove, false);
        // inputMoveDir 값이 0과 거의 동일하지 않으면 애니메이션 활성화
        else anim.SetBool(hashIsMove, true);

        // 이동 방향에 따라 애니메이터의 MoveVec 파라미터 값을 설정
        anim.SetFloat(hashMove, inputMoveDir);
    }

    private void Move()
    {
        // 키를 눌러서 방향 받아오기
        inputMoveDir = Input.GetAxis("Horizontal");
        // 입력 값이 없으면 실행하지 않고 리턴
        if (inputMoveDir == 0) return;

        // 입력받은 방향으로 이동하기
        moveVec.x = inputMoveDir * moveSpeed * Time.deltaTime;
        transform.Translate(moveVec);
    }
}
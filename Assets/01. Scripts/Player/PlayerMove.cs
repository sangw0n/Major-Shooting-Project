using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    // ----------- [ SerializeField Field ] -----------
    [SerializeField] private int    moveSpeed; // 움직임 속도 

    // ----------- [ Private Field ] -----------
    private Vector2                 moveDir;   // 움직임 방향

    // ----------- [ Component Field ] -----------
    private Rigidbody2D             rigid;     
    private Animator                animator;

    // ----------- [ Animator Hash Field ] -----------
    private readonly int moveXHash  = Animator.StringToHash("moveX");
    private readonly int isMoveHash = Animator.StringToHash("isMove");
    
    private void Start()
    {
        rigid       = GetComponent<Rigidbody2D>();
        animator    = GetComponent<Animator>();
    }
    
    private void Update()
    {
        UpdateState();
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + (moveDir * moveSpeed * Time.fixedDeltaTime));
    }

    ///<summary> New Input System - Invoke Unity Events 등록용 움직임 함수 </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir.x = context.ReadValue<float>();
    }

    ///<summary> 플레이어의 움직임을 체크해 상태 업데이트 함수 </summary>
    private void UpdateState()
    {
        if(moveDir.x == 0)
        {
            animator.SetBool(isMoveHash, false);
        }
        else 
        {
            animator.SetBool(isMoveHash, true);
        }

        // isMoveHash 값이 true 일 때 코드 실행
        if( animator.GetBool(isMoveHash) )
        {
            animator.SetFloat("MoveX", moveDir.x);
        }
    }
}

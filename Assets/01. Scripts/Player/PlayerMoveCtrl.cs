// # Unity  
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveCtrl : MonoBehaviour
{
    [Header("[# Move Var Header]")]
    [SerializeField] private int moveSpeed;
    [SerializeField] private float inputDir;
    private Vector2 moveVec;

    // Object :: Component
    private Animator anim;

    // Animator :: Hash Value
    private readonly int hashIsMove = Animator.StringToHash("isMove");
    private readonly int hashMove = Animator.StringToHash("Move");

    private void Awake()
    {
        // Component 가져오기
        anim = GetComponent<Animator>();

    }

    private void Update()
    {
        if(moveVec != Vector2.zero)
            transform.Translate(moveVec * moveSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        // Animator :: 설정 
        // inputMoveDir 값이 0과 거의 동일하면 애니메이션 비활성화
        if (Mathf.Approximately(inputDir, 0)) 
             anim.SetBool(hashIsMove, false);
        // inputMoveDir 값이 0과 거의 동일하지 않으면 애니메이션 활성화
        else anim.SetBool(hashIsMove, true);

        // 이동 방향에 따라 애니메이터의 MoveVec 파라미터 값을 설정
         anim.SetFloat(hashMove, inputDir);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 입력한 키 값을 가져오기
        inputDir = context.ReadValue<float>();

        // 움직이는 방향 설정해주기
        moveVec.x = inputDir;
    }
}
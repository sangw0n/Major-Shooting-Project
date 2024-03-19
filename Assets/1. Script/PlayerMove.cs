using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("# Movement Var")]
    [SerializeField] private float moveSpeed;
    private Vector3 inputVec;

    [Header("# Dash Var")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCoolTime;
    [SerializeField] private float dashCurCoolTime;
    [SerializeField] private TrailRenderer trailObj;

    private bool isDash;

    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        InputKey();
        Move();

        DashCoolTime();
        StartCoroutine(IDash());
    }

    #region Movement Functions

    // # Move Function
    private void InputKey()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
    }

    private void Move()
    {
        transform.position += inputVec * moveSpeed * Time.deltaTime;
    }

    // # Dash Functions
    private void DashCoolTime()
    {
        if (dashCurCoolTime >= dashCoolTime)
        {
            isDash = true;
        }
        else if (isDash == false)
        {
            dashCurCoolTime += Time.deltaTime;
        }
    }

    private IEnumerator IDash()
    {
        if (isDash == true && Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashCurCoolTime = 0;
            isDash = false;
            trailObj.emitting = true;
            Vector3 dashVec = inputVec * dashForce;
            transform.position = Vector3.Lerp(transform.position, transform.position += dashVec, 10.0f * Time.deltaTime);

            yield return new WaitForSeconds(0.25f);
            trailObj.emitting = false;
        }
    }
    #endregion

}

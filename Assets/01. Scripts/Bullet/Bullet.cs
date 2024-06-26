using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    // ----------- [ SerializeField Field ] -----------
    [SerializeField] private int moveSpeed;             // 총알 움직임 속도
    [SerializeField] private int damage;                // 총알 데미지

    // ----------- [ Private Field ] -----------
    private GameObject           target;                // 타겟
    private Vector2              direction;             // 총알 움직임 방향
    private bool                 isBulletTargeted;      // 총알이 타겟에 고정되었는지 여부

    // ----------- [ Component Field ] -----------
    private Rigidbody2D          rigid;
    
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        Destroy(gameObject, 3.0f);
    }

    private void Update()
    {
        if(isBulletTargeted && target == null)
        {
            Destroy(gameObject);
        }

        RotateTowardsTarget();
    }

    private void FixedUpdate()
    {
        Move();
    }

    ///<summary> 총알의 변수들을 기본 세팅 해주는 함수 </summary>
    public void Initialize(Vector2 direction, bool isBulletTargeted, GameObject target = null)
    {
        this.direction        = direction;
        this.isBulletTargeted = isBulletTargeted;
        this.target           = target;  
    }

    private void Move()
    {
        rigid.MovePosition(rigid.position + ( direction * moveSpeed * Time.fixedDeltaTime ));
    }

    private void RotateTowardsTarget()
    {
        if(  target == null ) return;
        if ( !isBulletTargeted ) return;

        // 타겟 방향으로 회전함
        float angle        = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward * 50);
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("ENEMY"))
    //     {
    //         if( isBulletTargeted && collision.gameObject == target ) return;
            
    //         collision.GetComponent<Enemy>().TakeDamage(damage);

    //         Destroy(gameObject);
    //     }
    // }
}
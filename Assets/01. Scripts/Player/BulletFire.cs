using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire : MonoBehaviour
{
    // ----------- [ SerializeField Field ] -----------
    [SerializeField] private GameObject     bulletPrefab;   // 총알 프리팹
    [SerializeField] private float          fireCooldown;   // 총알 발사 쿨타임
    [SerializeField] private Vector3        firePosition;   // 총알 발사 위치

    // ----------- [ Private Field ] -----------
    private bool                            isFire;         // 총알 발사 가능 여부

    // ----------- [ Component Field ] -----------
    private EnemyDetector                   enemyDetector;

    private void Start()
    {
        enemyDetector = GetComponent<EnemyDetector>();

        isFire = true;
    }
    
    private void Update()
    {
        if(Input.GetKey(KeyCode.X))
        {
            Fire();
        }
    }

    private void Fire()
    {
        // 타겟에게 총알 발사
        if( isFire && enemyDetector.IsDetecting && enemyDetector.CloseEnemy != null)
        {
            FireAtTarget();
        }
        // 앞 방향에 총알 발사 
        else if( isFire )
        {
            FireForward();
        }
    }

    private void FireAtTarget()
    {
        isFire = false;

        // 타겟의 방향 구하기 
        Vector3    targetDir    = (enemyDetector.CloseEnemy.transform.position - transform.position).normalized;

        GameObject cloneBullet  = Instantiate( bulletPrefab, transform.position + firePosition, Quaternion.Euler(0, 0, 90) );
        cloneBullet.GetComponent<Bullet>().Initialize(targetDir, true, enemyDetector.CloseEnemy);

        StartCoroutine(FireCooldown());
    }

    private void FireForward()
    {
        isFire = false;

        GameObject cloneBullet = Instantiate( bulletPrefab, transform.position + firePosition, Quaternion.Euler(0, 0, 90) );
        cloneBullet.GetComponent<Bullet>().Initialize(Vector2.up, false);

        StartCoroutine(FireCooldown());
    }

    private IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(fireCooldown);

        isFire = true;
    }
}

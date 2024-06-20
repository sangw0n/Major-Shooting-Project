// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class Enemy01 : Enemy
{
    // ----------- [ SerializeField Field ] -----------
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int        fireCount;
    [SerializeField] private float      fireCooltime;

    // ----------- [ Private Field ] -----------
    private EnemyAi enemyAi;

    protected override void Start()
    {
        base.Start();

        enemyAi = GetComponent<EnemyAi>();
    }

    public override void Attack()
    {
        if( isAttacking ) return;

        StartCoroutine(Co_Attack());
    }

    private IEnumerator Co_Attack()
    {
        isAttacking = true;
    
        yield return StartCoroutine(Fire());

        isAttacking = false;

        enemyAi.SetState(EnemyState.SearchRoute);
    }

    private IEnumerator Fire()
    {
        for(int i = 0; i < fireCount; i++)
        {
            GameObject bulletClone = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, 90));

            bulletClone.GetComponent<Bullet>().Initialize(Vector2.down, false);

            yield return new WaitForSeconds(fireCooltime);
        }
    }
}

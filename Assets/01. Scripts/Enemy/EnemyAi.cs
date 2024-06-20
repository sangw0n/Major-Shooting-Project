using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public EnemyState  enemyState;

    private Transform   targetPoint;
    private Transform[] pointNode;

    private Enemy       enemy;

    private void Start()
    {
        Initlaized();
    }

    private void Update()
    {
        UpdateState();
    }

    private void Initlaized()
    {
        // # Var Field Setting
        enemy      = GetComponent<Enemy>();
        pointNode  = PointNodeManager.Instance.enemyPointNode;
        enemyState = EnemyState.Move;

        // # Find Near Poind
        Collider2D[] pointInField     = Physics2D.OverlapCircleAll(transform.position, 3, 1 << LayerMask.NameToLayer("Point"));
        float        targetDistance   = float.MaxValue;

        foreach(Collider2D point in pointInField)
        {
            float  distanceToEnemy = Vector3.Distance(point.transform.position, transform.position);       // 플레이어와 적의 거리

            if( distanceToEnemy < targetDistance )                                                         // 플레이어와 적의 거리가 현재 타겟과의 거리보다 가까우면
            {
                targetDistance = distanceToEnemy;                                                           
                targetPoint    = point.transform;                                                        
            }
        }
    }

    private void UpdateState()
    {
        switch (enemyState)
        {
            case EnemyState.Default:
                enemyState = EnemyState.Move;
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                enemy.Attack();
                break;
        }
    }

    private void Move()
    {
        
    }
}

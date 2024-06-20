using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    // ----------- [ SerializeField Field ] -----------
    [SerializeField] private float detectorRadius;

    // ----------- [ Private Field ] -----------
    private EnemyState   enemyState;

    private Transform    targetPoint;
    private Collider2D[] nearPointNode;
    private Transform[]  pointNode;

    private Enemy       enemy;

    private IEnumerator Start()
    {
        Initlaized();

        while( true )
        {
            UpdateState();

            yield return null;
        }
    }

    private void FixedUpdate()
    {
        nearPointNode = Physics2D.OverlapCircleAll(transform.position, detectorRadius, 1 << LayerMask.NameToLayer("Point"));
    }

    private void Initlaized()
    {
        enemy         = GetComponent<Enemy>();
        pointNode     = PointNodeManager.Instance.enemyPointNode;
        nearPointNode = Physics2D.OverlapCircleAll(transform.position, detectorRadius, 1 << LayerMask.NameToLayer("Point"));

        // Find Near Point
        float targetDistance = float.MaxValue;
        
        foreach(Collider2D point in nearPointNode)
        {
            float  distanceToPoint = Vector3.Distance(point.transform.position, transform.position);

            if(distanceToPoint < targetDistance)
            {
                targetPoint     = point.transform;
                targetDistance  = distanceToPoint;                
            }
        }

        PointNodeManager.Instance.SetPointNodeData(targetPoint, true);

        enemyState    = EnemyState.Move;
    }

    // 이동 포인트 탐지 -> 움직이기 -> 공격 
    private void UpdateState()
    {
        switch (enemyState)
        {
            case EnemyState.SearchRoute:
                SearchRoute();
                break;

            case EnemyState.Move:
                Move();
                break;

            case EnemyState.Attack:
                enemy.Attack();
                break;
        }
    }

    public void SetState(EnemyState enemyState)
    {
        this.enemyState = enemyState;
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, 
            targetPoint.position, Time.deltaTime * enemy.MoveSpeed);

        // 목표 Point 근처에 닿으면
        if(Vector3.Distance(targetPoint.position, transform.position) < 0.01 * enemy.MoveSpeed)
        {
            transform.position = targetPoint.position;
            enemyState         = EnemyState.Attack;
        }
    }

    private void SearchRoute()
    {
        int random      = 0;

        while( true )
        {
            random  = Random.Range(0, nearPointNode.Length);

            if( targetPoint != nearPointNode[random].transform && !PointNodeManager.Instance.GetPointNodeDataKey(nearPointNode[random].transform) )
                break;
        }

        targetPoint = nearPointNode[random].transform;

        PointNodeManager.Instance.SetPointNodeData(targetPoint, true);

        enemyState  = EnemyState.Move;
    }
}

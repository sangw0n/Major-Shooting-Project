// # Unity
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Vector2 aimAreaSize;
    [SerializeField] private Vector3 aimAreaPos;

    [Space(5)]
    [SerializeField] private Collider2D[] enemyInArea;
    [SerializeField] private Collider2D nearEnemyTarget;

    // Function :: 근처 적들을 탐지하는 함수
    private void FindNearEnemy()
    {
        enemyInArea = Physics2D.OverlapBoxAll(transform.position + aimAreaPos, aimAreaSize, 0, enemyLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + aimAreaPos, aimAreaSize);
    }
}

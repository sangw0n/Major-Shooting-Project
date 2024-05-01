namespace MajorProject.Character.Player
{
    // # Unity
    using UnityEngine;

    // 가까운 적을 탐지하는 클래스
    public class EnemyDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private Vector2 aimAreaSize;
        [SerializeField] private Vector3 aimAreaPosOffset;

        [Space(5)]
        [SerializeField] private Collider2D[] enemyInArea;
        [SerializeField] private Collider2D nearEnemyTarget;

        public Collider2D NearEnemyTarget { get => nearEnemyTarget; }

        // 타켓팅 하고 있는 타겟과 플레이어 사이의 거리
        private float curDistanceToTarget;

        private void Awake()
        {
            curDistanceToTarget = float.MaxValue;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.X))
                FindNearEnemy();
            else
                nearEnemyTarget = null;
        }

        private void FixedUpdate()
        {
            // 적들을 감지하는 OverlapBox
            enemyInArea = Physics2D.OverlapBoxAll(transform.position + aimAreaPosOffset, aimAreaSize, 0, enemyLayer);
        }

        // Function :: 근처 적을 탐지하는 함수
        private void FindNearEnemy()
        {
            curDistanceToTarget = float.MaxValue;

            foreach (Collider2D enemyTarget in enemyInArea)
            {
                // 타켓 몬스터와 플레이어 사이의 거리를 알아내기 
                float distanceToTarget = Vector2.Distance(enemyTarget.transform.position, transform.position);

                if (distanceToTarget <= curDistanceToTarget)
                {
                    // 타켓팅 몬스터 설정
                    nearEnemyTarget = enemyTarget;

                    // 타켓팅 중인 몬스터와 플레이어 사이의 거리 저장
                    curDistanceToTarget = distanceToTarget;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + aimAreaPosOffset, aimAreaSize);
        }
    }
}
namespace MajorProject.Play
{
    // # System
    using System.Collections;
    using System.Data.Common;

    // # Unity
    using UnityEngine;

    public class Monster : Entity
    {
        private const int INIT_HP = 100;

        [SerializeField] private ObjecyKeyType keyType;
        [SerializeField] private MonsterState monsterState = MonsterState.Idle;

        [Header("[# Point ]")]
        [SerializeField] private Transform targetPoint;
        [SerializeField] private Collider2D[] pointNode;

        [Header("[# Point Detector]")]
        [SerializeField] private Vector2 aimAreaSize;
        [SerializeField] private Vector3 aimAreaPosOffeset;
        [SerializeField] private LayerMask pointLayer;

        [Header("[# Monster Movement]")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float targetDistance;
        [SerializeField] private int pointKey;

        [Header("[# Monster Attack]")]
        [SerializeField] private int fireCount;

        // Variable :: Bool
        private bool isMoveing = false;
        private bool isAttacking = false;

        // Variable :: Component
        private new Collider2D collider;

        // Variable :: WaitForSeconds
        private WaitForSeconds coolTimeWaitForSeconds = new WaitForSeconds(0.5f);
        private WaitForSeconds fireCoolTimeWaitForSeconds = new WaitForSeconds(0.3f);

        private void Awake()
        {
            collider = GetComponent<Collider2D>();
        }

        private IEnumerator Start()
        {
            pointNode = Physics2D.OverlapBoxAll(this.transform.position + aimAreaPosOffeset, aimAreaSize, 0, pointLayer);

            yield return new WaitForSeconds(0.5f);
            while (true)
            {
                switch (monsterState)
                {
                    case MonsterState.Idle: SetTargetPoint(); break;
                    case MonsterState.Moving: StartCoroutine(Move()); break;
                    case MonsterState.Attack:
                        if (!isAttacking)
                        {
                            isAttacking = true;
                            StartCoroutine(Attack());
                        }
                        break;
                }
                yield return null; ;
            }
        }

        private void FixedUpdate()
        {
            // 근처 좌표 포인트를 찾는 코드 
            pointNode = Physics2D.OverlapBoxAll(this.transform.position + aimAreaPosOffeset, aimAreaSize, 0, pointLayer);
        }

        #region Move Fucntions
        private void SetTargetPoint()
        {
            int randomIdx = Random.Range(0, pointNode.Length);
            pointKey = pointNode[randomIdx].GetComponent<Point>().key;

            if (!PointNodeManager.Instance.pointNodeDictionary[pointKey].isUsing)
            {
                PointNodeManager.Instance.pointNodeDictionary[pointKey].isUsing = true;

                // 목표 좌표 설정 
                targetPoint = PointNodeManager.Instance.pointNodeDictionary[pointKey].transform;
                monsterState = MonsterState.Moving;
            }
            else return;
        }

        private IEnumerator Move()
        {
            // 목표 포인트와 몬스터 사이의 거리 구하기
            targetDistance = Vector3.Distance(transform.position, targetPoint.position);

            // 목표 포인트에 도착하면 
            if (targetDistance <= 0.01f)
            {
                yield return coolTimeWaitForSeconds;

                // 움직임 비활성화    
                isMoveing = false;
                // 공격 상태 전환
                monsterState = MonsterState.Attack;
            }

            // 목표 좌표로 몬스터 이동
            transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
        }
        #endregion  

        private IEnumerator Attack()
        {
            for (int i = 0; i < fireCount; i++)
            {
                // 총알 발사 
                GameObject bullet = PoolManager.Instance.GetObject(ObjecyKeyType.MONSTERBULLET);

                // 플레이어 방향으로 총알 발사
                bullet.transform.position = transform.position;
                bullet.GetComponent<EnemyBullet>().TrackPlayer();

                yield return fireCoolTimeWaitForSeconds;
            }

            yield return coolTimeWaitForSeconds;
            isAttacking = false;
            PointNodeManager.Instance.pointNodeDictionary[pointKey].isUsing = false;
            monsterState = MonsterState.Idle;
        }

        #region CallBack Functions
        private void OnEnable()
        {
            health = INIT_HP;
            collider.enabled = true;
        }

        private void OnDisable()
        {
            collider.enabled = false;
            isAttacking = false;
            isMoveing = false;
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.gameObject.CompareTag("BULLET"))
            {
                // 총알 풀로 리턴
                PoolManager.Instance.ReturnObject(coll.gameObject, ObjecyKeyType.PLAYERBULLET);
                // 체력 감소
                health -= coll.gameObject.GetComponent<Bullet>().ApplyDamage();

                if (health <= 0)
                {
                    PoolManager.Instance.ReturnObject(this.gameObject, keyType);
                    MonsterPatternManager.Instance.RemainingMonsterCount--;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(this.transform.position + aimAreaPosOffeset, aimAreaSize);
        }
        #endregion
    }
}
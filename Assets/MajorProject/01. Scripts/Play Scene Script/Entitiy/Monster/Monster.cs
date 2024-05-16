namespace MajorProject.Play
{
    using System.Collections;
    using UnityEngine;

    public enum MonsterState
    {
        Idle,
        Moving,
        Attack
    }

    public class Monster : Entity
    {
        private const int INIT_HP = 100;

        [SerializeField] private ObjecyKeyType keyType;
        [SerializeField] private MonsterState monsterState = MonsterState.Idle;

        [Header("[# Point Detector]")]
        [SerializeField] private Transform targetPoint;
        [SerializeField] private Collider2D[] pointNode;
        [SerializeField] private Vector2 aimAreaSize;
        [SerializeField] private Vector3 aimAreaPosOffeset;
        [SerializeField] private LayerMask pointLayer;

        [Header("[# Monster Move]")]
        [SerializeField] private float moveSpeed;
        private float targetDistance;

        [Header("[# Monster Attack]")]
        public int fireCount;

        // Variable :: Bool
        private bool isMoveing = false;
        private bool isAttacking = false;
        // private bool isAttackCoroutine = false; // Attack Coroutine 작동 여부

        // Object :: Component
        private new Collider2D collider;

        private WaitForSeconds coolTimeWaitForSeconds = new WaitForSeconds(0.5f);

        private void Awake()
        {
            collider = GetComponent<Collider2D>();
        }

        private IEnumerator Start()
        {
            pointNode = Physics2D.OverlapBoxAll(this.transform.position + aimAreaPosOffeset, aimAreaSize, 0, pointLayer);

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
                yield return null;
            }
        }

        private void FixedUpdate()
        {
            // 근처 좌표 포인트를 찾는 코드 
            pointNode = Physics2D.OverlapBoxAll(this.transform.position + aimAreaPosOffeset, aimAreaSize, 0, pointLayer);
        }

        #region Move Fucntions
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

        private void SetTargetPoint()
        {
            // 근처 포인트 중 목표 포인트 선택
            int randomIdx = Random.Range(0, pointNode.Length);
            // 목표 포인트의 Key 값 가져오기 
            int pointKey = pointNode[randomIdx].GetComponent<PointKey>().pointKey;

            // 목표 포인트가 사용중이면 건너뜀 
            if (PointNodeManager.Instance.pointUsageStatus[pointKey]) return;
            
            // 목표 포인트 사용 활성화 ( Bool )
            PointNodeManager.Instance.pointUsageStatus[pointKey] = true;

            // 목표 포인트로 움직이기 
            targetPoint = pointNode[randomIdx].transform;
            monsterState = MonsterState.Moving;
            isMoveing = true;
        }
        #endregion

        // ToDo. 아래 코드 최적화
        private IEnumerator Attack()
        {
            for (int i = 0; i < fireCount; i++)
            {
                // 총알 발사 
                GameObject bullet = PoolManager.Instance.GetObject(ObjecyKeyType.MONSTERBULLET);
                // 플레이어 방향으로 총알 발사
                bullet.transform.position = transform.position;
                bullet.GetComponent<Bullet>().TrackPlayer();

                yield return new WaitForSeconds(0.5f);
            }

            yield return coolTimeWaitForSeconds;
            isAttacking = false;
            monsterState = MonsterState.Idle;
        }

        #region CallBack Functions
        private void OnEnable()
        {
            isMoveing = false;
            isAttacking = false;
            health = INIT_HP;
            collider.enabled = true;
        }

        private void OnDisable()
        {
            collider.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.gameObject.CompareTag("BULLET"))
            {
                PoolManager.Instance.ReturnObject(coll.gameObject, ObjecyKeyType.PLAYERBULLET);
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
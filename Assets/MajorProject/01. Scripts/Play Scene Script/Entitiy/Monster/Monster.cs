namespace MajorProject.Play
{
    using System.Collections;
    using UnityEngine;

    public enum MonsterState
    {
        Default,
        Moving,
        Attack
    }

    public class Monster : Entity
    {
        private const int INIT_HP = 100;

        [SerializeField] private ObjecyKeyType keyType;
        [SerializeField] private MonsterState monsterState = MonsterState.Default;

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
        private bool isAttackCoroutine = false; // Attack Coroutine 작동 여부

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
                    case MonsterState.Default:
                        monsterState = MonsterState.Moving;
                        break;
                    case MonsterState.Moving: StartCoroutine(Move()); break;
                    case MonsterState.Attack:
                        if (!isAttacking)
                        {
                            // 공격중이 아니면 공격 함수 실행
                            isAttacking = true;
                            if (!isAttackCoroutine)
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
            // 목표 포인트 지정
            if (!isMoveing) SetTargetPoint();
            isMoveing = true;

            // 목표 지점에 도착하면 다음 상태 지정
            targetDistance = Vector3.Distance(transform.position, targetPoint.position);
            if (targetDistance <= 0.01f)
            {
                yield return coolTimeWaitForSeconds;
                isMoveing = false;
                monsterState = MonsterState.Attack;
            }

            // 목표 좌표로 몬스터 이동
            transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
        }

        private void SetTargetPoint()
        {
            // 목표 좌표 설정하기
            int randomIdx = Random.Range(0, pointNode.Length);
            targetPoint = pointNode[randomIdx].transform;
        }
        #endregion

        private IEnumerator Attack()
        {
            isAttackCoroutine = true;

            for (int i = 0; i < fireCount; i++)
            {
                // 총알 발사 
                GameObject bullet = PoolManager.Instance.GetObject(ObjecyKeyType.MONSTERBULLET);
                // 생성한 Bullet 위치 초기화
                bullet.transform.position = transform.position;
                // 방향
                Vector3 direction = GameManager.Instance.player.transform.position - bullet.transform.position;
                // 2D 환경에서의 회전 각도 생성
                float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                //총알의 Z축을 기준으로 회전 적용
                bullet.transform.rotation = Quaternion.Euler(0, 0, -angle);
                // 움직일 방향 구해주기 
                bullet.GetComponent<Bullet>().InitDir(direction.normalized);

                yield return new WaitForSeconds(0.5f);
            }

            yield return coolTimeWaitForSeconds;
            isAttacking = false;
            isAttackCoroutine = false;
            monsterState = MonsterState.Moving;
        }

        #region CallBack Functions
        private void OnEnable()
        {
            isMoveing = false;
            isAttacking = false;
            isAttackCoroutine = false;

            health = INIT_HP;
            collider.enabled = true;
            monsterState = MonsterState.Default;
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
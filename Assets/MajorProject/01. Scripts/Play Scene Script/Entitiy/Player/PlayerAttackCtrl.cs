namespace MajorProject.Play
{
    // # System
    using System.Collections;

    // # Unity
    using UnityEngine;

    public class PlayerAttackCtrl : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject weaponObject;

        [Header("[# Cooltime Var Header]")]
        [SerializeField] private float curFireCooltime;
        [SerializeField] private float fireCooltime;

        [Header("[# Bool Var Header]")]
        [SerializeField] private bool isFire;

        private EnemyDetector enemyDetector;

        private void Awake()
        {
            enemyDetector = GetComponent<EnemyDetector>();
        }

        private void Update()
        {
            FireCooltime();
            Fire();
        }

        // Function :: 총알 발사 함수 
        private void Fire()
        {
            // Key Value 를 입력하면 총알 발사
            if (Input.GetKey(KeyCode.C) && isFire)
            {
                // 쿨타임 초기화
                isFire = false;
                curFireCooltime = 0.0f;

                // 총알 발사 
                GameObject bullet = PoolManager.Instance.GetObject(ObjecyKeyType.PLAYERBULLET);
                // 생성한 Bullet 위치 초기화
                bullet.transform.position = weaponObject.transform.position;

                // 타켓팅중인 오브젝트 방향으로 발사
                if (enemyDetector.NearEnemyTarget != null && enemyDetector.NearEnemyTarget.gameObject.activeSelf)
                {
                    // 방향
                    Vector3 direction = enemyDetector.NearEnemyTarget.transform.position - bullet.transform.position;
                    // 2D 환경에서의 회전 각도 생성
                    float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                    //총알의 Z축을 기준으로 회전 적용
                    bullet.transform.rotation = Quaternion.Euler(0, 0, -angle);
                    // 움직일 방향 구해주기 
                    bullet.GetComponent<Bullet>().InitDir(direction.normalized);
                }
                else
                {
                    //총알의 Z축을 기준으로 회전 적용
                    bullet.transform.rotation = Quaternion.Euler(Vector3.zero);
                    // 움직일 방향 구해주기
                    bullet.GetComponent<Bullet>().InitDir(transform.up);
                }
            }
        }

        // Function :: 총알 발사 쿨타임 함수
        private void FireCooltime()
        {
            // 현재 쿨타임이 발사 쿨타임에 도달했다면
            if (curFireCooltime >= fireCooltime)
            {
                isFire = true;
                return;
            }
            curFireCooltime += Time.deltaTime;
        }
    }
}
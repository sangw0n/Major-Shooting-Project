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

    // Test :: Component 
    private SpriteRenderer weaponSpriteRdr;

    private void Awake()
    {
        enemyDetector = GetComponent<EnemyDetector>();

        // Test :: GetCompoenet 
        weaponSpriteRdr = weaponObject.GetComponent<SpriteRenderer>();
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
            // 무기 스프라이트 표시
            StartCoroutine(ChangeWeaponSprite());

            // 총알 발사 
            GameObject bullet = PoolManager.Instance.GetObject("BULLET");
            // 생성한 Bullet 위치 초기화
            bullet.transform.position = weaponObject.transform.position;

            // 총알이 조준하는 오브젝트 방향으로 발사
            if (enemyDetector.NearEnemyTarget != null)
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
                // 회전 각도 생성
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

    // Function :: 무기 스프라이트 바꾸기 함수
    private IEnumerator ChangeWeaponSprite()
    {
        // GunWeapon Object 활성화
        weaponObject.SetActive(true);

        // Sprite Color 바꿔주기 
        int randomColorIndex = Random.Range(0, 3);
        weaponSpriteRdr.color = SetColor(randomColorIndex);

        // 일정시간 후 GunWeapon Object 비활성화        
        yield return new WaitForSeconds(0.3f);
        weaponObject.SetActive(false);
    }

    // Function :: 색상 설정 함수
    private Color SetColor(int index)
    {
        Color _color;

        // index 에 맞는 color 적용시키기 
        switch (index)
        {
            case 0: _color = Color.red; break;
            case 1: _color = Color.green; break;
            case 2: _color = Color.blue; break;
            default: _color = Color.black; break;
        }

        // color 값 반환 
        return _color;
    }
}

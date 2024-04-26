// # System
using System.Collections;

// # Unity
using UnityEngine;

public class PlayerAttackCtrl : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject weaponObj;

    [Header("[# Cooltime Var Header]")]
    [SerializeField] private float curFireCooltime;
    [SerializeField] private float fireCooltime;

    [Header("[# Bool Var Header]")]
    [SerializeField] private bool isFire;

    // Test :: Component 
    private SpriteRenderer weaponSpriteRdr;

    private void Awake()
    {
        // Test :: GetCompoenet 
        weaponSpriteRdr = weaponObj.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        FireCooltime();
        Fire();
    }

    // F :: 총알 발사 쿨타임 함수
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

    // F :: 총알 발사 함수 
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
            bullet.transform.position = weaponObj.transform.position;
        }
    }

    // F :: 무기 스프라이트 바꾸기 함수
    private IEnumerator ChangeWeaponSprite()
    {
        // GunWeapon Object 활성화
        weaponObj.SetActive(true);

        // Sprite Color 바꿔주기 
        int randomColorIndex = Random.Range(0, 3);
        weaponSpriteRdr.color = SetColor(randomColorIndex);

        // 일정시간 후 GunWeapon Object 비활성화        
        yield return new WaitForSeconds(0.3f);
        weaponObj.SetActive(false);
    }

    // F :: 색상 설정 함수
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

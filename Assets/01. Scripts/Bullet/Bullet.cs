using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private int bulletSpeed;

    // Bullet :: Component
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRdr;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRdr = GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        // 총알 색상 설정
        int randomColorIndex = Random.Range(0, 3);
        spriteRdr.color = SetColor(randomColorIndex);

        // 총알 움직이기 
        rigid.AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);

        // Test :: Destory Object
        Destroy(this.gameObject, 0.5f);
    }

    private Color SetColor(int index)
    {
        Color _color;

        // index 에 맞는 color 적용시키기 
        switch (index)
        {
            case 0: _color = Color.cyan; break;
            case 1: _color = Color.green; break;
            case 2: _color = Color.white; break;
            default: _color = Color.black; break;
        }

        // color 값 반환 
        return _color;
    }
}
// # Unity
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private int bulletSpeed;
    public Vector3 dirVec;

    // Bullet :: Component
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRdr;

    private void OnEnable()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRdr = GetComponent<SpriteRenderer>();

        // 총알 색상 설정
        int randomColorIndex = Random.Range(0, 3);
        spriteRdr.color = SetColor(randomColorIndex);

        // 일정 시간 후 풀로 리턴
        PoolManager.Instance.ReturnObject(this.gameObject, "BULLET", 2.0f);
    }

    private void FixedUpdate()
    {
        rigid.velocity = dirVec * bulletSpeed;
    }

    // Function :: 총알 발사 방향 설정해주는 함수
    public void InitDir(Vector3 dirVec)
    {
        this.dirVec = dirVec;
    }

    // Function :: 색을 설정해주는 함수
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
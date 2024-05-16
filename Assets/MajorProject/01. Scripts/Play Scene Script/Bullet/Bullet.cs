namespace MajorProject.Play
{
    // # Unity
    using System.Collections;
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private int bulletSpeed;
        [SerializeField] private int bulletDamage;

        private Vector3 dirVec;

        // Bullet :: Component
        private Rigidbody2D rigid;
        private SpriteRenderer spriteRdr;

        private Coroutine coroutine;

        private void OnEnable()
        {
            // Bullet :: Component
            rigid = GetComponent<Rigidbody2D>();
            spriteRdr = GetComponent<SpriteRenderer>();

            // 총알 색상 설정
            int randomColorIndex = Random.Range(0, 3);
            spriteRdr.color = SetColor(randomColorIndex);

            // 이전에 시작된 코루틴이 있다면 취소
            if (coroutine != null) StopCoroutine(coroutine);

            // 새로운 코루틴 시작
            coroutine = StartCoroutine(CoReturnToPoolAfterDelay(2.5f));
        }

        private void FixedUpdate()
        {
            rigid.velocity = dirVec * bulletSpeed;
        }

        // Function :: 플레이어 방향으로 총알이 날아가는 함수
        public void TrackPlayer()
        {
            // 방향
            Vector3 direction = GameManager.Instance.player.transform.position - transform.position;
            // 2D 환경에서의 회전 각도 생성
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            //총알의 Z축을 기준으로 회전 적용
            transform.rotation = Quaternion.Euler(0, 0, -angle);
            // 움직일 방향 구해주기 
            InitDir(direction.normalized);
        }

        // Function :: 총알 발사 방향 설정해주는 함수
        public void InitDir(Vector3 dirVec)
        {
            this.dirVec = dirVec;
        }

        // Function :: 총알을 맞은 오브젝트한테 데미지를 주는 함수 
        public int ApplyDamage()
        {
            return bulletDamage;
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

        // Function :: 일정 시간 뒤 풀로 돌아가게 하는 함수 
        private IEnumerator CoReturnToPoolAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            PoolManager.Instance.ReturnObject(this.gameObject, ObjecyKeyType.PLAYERBULLET);
        }


    }
}
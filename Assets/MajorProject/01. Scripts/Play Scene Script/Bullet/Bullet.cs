namespace MajorProject.Play
{
    // # Unity
    using System.Collections;
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Bullet : MonoBehaviour
    {
        [SerializeField] private int bulletSpeed;
        [SerializeField] private int bulletDamage;

        private Vector3 dirVec;

        // Bullet :: Component
        private Rigidbody2D rigid;

        private Coroutine coroutine;

        protected virtual void OnEnable()
        {
            // Bullet :: Component
            rigid = GetComponent<Rigidbody2D>();

            // 이전에 시작된 코루틴이 있다면 취소
            if (coroutine != null) StopCoroutine(coroutine);

            // 새로운 코루틴 시작
            coroutine = StartCoroutine(CoReturnToPoolAfterDelay(3.5f));
        }

        protected virtual void FixedUpdate()
        {
            rigid.velocity = dirVec * bulletSpeed;
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

        // Function :: 일정 시간 뒤 풀로 돌아가게 하는 함수 
        protected abstract IEnumerator CoReturnToPoolAfterDelay(float delay);
    }
}
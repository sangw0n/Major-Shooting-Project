namespace MajorProject.Play
{
    using UnityEngine;

    public class Monster : Entity
    {
        [SerializeField] private const int INIT_HP = 100;
        [SerializeField] private ObjecyKeyType keyType;

        private void OnEnable()
        {
            // 초기 HP Value 설정
            health = INIT_HP;
        }

        private void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.CompareTag("BULLET"))
            {
                // 풀로 리턴
                PoolManager.Instance.ReturnObject(coll.gameObject, ObjecyKeyType.BULLET);
                // 데미지 받음
                health -= coll.gameObject.GetComponent<Bullet>().ApplyDamage();

                // 체력을 전부 소모해 사망했다면
                if (health <= 0)
                {
                    // 풀로 다시 리턴
                    PoolManager.Instance.ReturnObject(this.gameObject, keyType);
                }
            }
        }
    }

}
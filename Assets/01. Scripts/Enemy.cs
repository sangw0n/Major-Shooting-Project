using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int enemyInitHp = 100;
    [SerializeField] private int enemyHp;
    [SerializeField] private ObjecyKeyType keyType;

    private void OnEnable()
    {
        enemyHp = enemyInitHp;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("BULLET"))
        {
            // 풀로 리턴
            PoolManager.Instance.ReturnObject(coll.gameObject, ObjecyKeyType.BULLET);
            // 데미지 받음
            enemyHp -= coll.gameObject.GetComponent<Bullet>().ApplyDamage();

            // 체력을 전부 소모해 사망했다면
            if (enemyHp <= 0)
            {
                // 풀로 다시 리턴
                PoolManager.Instance.ReturnObject(this.gameObject, keyType);
                
                // 웨이브에 남아있는 적 카운트 차감
                MonsterPatternManager.Instance.RemainingMosters--;
            }
        }
    }
}

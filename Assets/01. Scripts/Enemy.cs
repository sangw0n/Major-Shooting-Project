using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("BULLET"))
        {
            // 풀로 리턴
            PoolManager.Instance.ReturnObject(coll.gameObject, "BULLET");
        }
    }
}

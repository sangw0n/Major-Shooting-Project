namespace MajorProject.Play
{
    // # Unity
    using System.Collections;
    using UnityEngine;

    public class Barrel : Entity
    {
        [SerializeField] private float force;
        [SerializeField] private GameObject explosionEffect;
        [SerializeField] private Vector3 effectPosOffset;

        private new Collider2D collider;
        private Rigidbody2D rigid;

        private void Start()
        {
            rigid = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();
        }

        // private void Explode()
        // {
        //     rigid.constraints = RigidbodyConstraints2D.None;
        //     collider.enabled = false;

        //     rigid.AddForce(Vector3.up * force, ForceMode2D.Impulse);
        // }

        private IEnumerator OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.CompareTag("BULLET"))
            {
                PoolManager.Instance.ReturnObject(coll.gameObject, ObjecyKeyType.BULLET);

                if (--health <= 0)
                {
                    // 폭발하는 함수 
                    Destroy(this.gameObject);
                    // 폭발 이펙트 생성
                    GameObject effect = Instantiate(explosionEffect, transform.position + effectPosOffset, Quaternion.identity);
                    // 일정시간 후 폭발 이펙트 삭제
                    yield return new WaitForSeconds(0.5f);
                    Destroy(effect);
                }
            }
        }
    }
}
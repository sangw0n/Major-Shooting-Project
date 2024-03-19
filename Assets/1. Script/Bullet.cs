using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;

    private TrailRenderer trail;
    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        //StartCoroutine(TrailEffect());
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 dirVec = Vector3.up * bulletSpeed;
        rigid.velocity = dirVec;
    }

    private IEnumerator TrailEffect()
    {
        trail.emitting = true;
        yield return new WaitForSeconds(1f);
        trail.emitting = false;
    }
}

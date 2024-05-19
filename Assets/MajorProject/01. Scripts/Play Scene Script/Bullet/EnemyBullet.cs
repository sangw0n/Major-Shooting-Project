namespace MajorProject.Play
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EnemyBullet : Bullet
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
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

        protected override IEnumerator CoReturnToPoolAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            PoolManager.Instance.ReturnObject(this.gameObject, MajorProject.ObjecyKeyType.MONSTERBULLET);
        }
    }

}
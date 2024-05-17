using System.Collections;
using System.Collections.Generic;
using MajorProject.Play;
using UnityEngine;

public class PlayerBullet : Bullet
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    protected override IEnumerator CoReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PoolManager.Instance.ReturnObject(this.gameObject, MajorProject.ObjecyKeyType.PLAYERBULLET);
    }
}

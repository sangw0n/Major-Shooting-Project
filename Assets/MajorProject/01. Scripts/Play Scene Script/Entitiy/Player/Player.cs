namespace MajorProject.Play
{
    // # System
    using System.Collections;
    using System.Collections.Generic;

    // # Unity
    using UnityEngine;

    public class Player : Entity
    {
        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.gameObject.CompareTag("BULLET"))
            {
                PoolManager.Instance.ReturnObject(coll.gameObject, ObjecyKeyType.MONSTERBULLET);
            }
        }
    }
}
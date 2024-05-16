namespace MajorProject.Play
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class UsageState : MonoBehaviour
    {
        private int pointKey;

        private void Start()
        {
            pointKey = GetComponentInParent<PointKey>().pointKey;
        }

        private void OnTriggerExit2D(Collider2D trigger)
        {
            if (trigger.CompareTag("MONSTER"))
            {
                Debug.Log("작동");
                PointNodeManager.Instance.pointUsageStatus[pointKey] = false;
            }
        }
    }

}
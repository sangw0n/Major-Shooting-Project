namespace MajorProject.Play
{
    // # System
    using System.Collections;
    using System.Collections.Generic;

    // # Unity
    using UnityEngine;

    public class PointNodeManager : MonoBehaviour
    {
        public static PointNodeManager Instance { get; private set; }

        public Dictionary<int, bool> pointUsageStatus = new Dictionary<int, bool>();

        private PointKey[] pointKeys;

        private void Awake()
        {
            Instance = this;

            // PointKey 가져오기
            pointKeys = GetComponentsInChildren<PointKey>();

            // Dictionary 생성  
            pointUsageStatus = new Dictionary<int, bool>();

            // Dictionary 에 데이터 추가
            for (int i = 0; i < pointKeys.Length; i++)
                pointUsageStatus.Add(i, false);
        }
    }
}

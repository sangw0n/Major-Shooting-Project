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

        public Dictionary<int, Point> pointNodeDictionary = new Dictionary<int, Point>();

        [SerializeField] private GameObject pointNode;
        [SerializeField] private Point[] points;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            // Point 가져오기
            points = pointNode.GetComponentsInChildren<Point>();

            // Point Key, Dictionary 설정
            for (int i = 0; i < points.Length; i++)
            {
                points[i].key = i;
                pointNodeDictionary.Add(points[i].key, points[i]);
            }
        }
    }
}

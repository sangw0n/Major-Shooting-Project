// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class Point : MonoBehaviour
{
    public Transform point;
    public bool isUsing;

    private void Start() => point = this.transform;
}

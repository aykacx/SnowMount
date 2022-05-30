using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform _target;
    public float camSpeed;
    public Vector3 offset;

    //bu script sadece player� takip etmesi i�in yazd�m verdi�in offsetle takip eder

    private void Update()
    {
        var target = _target.transform.position + offset;
        var targetPos = new Vector3(transform.position.x, target.y, target.z);
        var smoothPos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * camSpeed);
        transform.position = smoothPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float followSmooth;
    void LateUpdate(){
        Vector3 targetPos = followTarget.position;
        targetPos.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, targetPos, followSmooth * Time.deltaTime);
    }
}

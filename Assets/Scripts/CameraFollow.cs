using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float followSmooth;
    [SerializeField] private float maxYOffset;
    [SerializeField] private Rect camPosLimit;
    void LateUpdate(){
        Vector3 targetPos = transform.position;
        targetPos.x = followTarget.position.x;
        float diff = Mathf.Abs(followTarget.position.y - targetPos.y) - maxYOffset;
        if (diff>0){
            targetPos.y = transform.position.y + ((followTarget.position.y-targetPos.y)>0?1:-1)* diff;
        }

        targetPos.x = Mathf.Clamp(targetPos.x, camPosLimit.xMin, camPosLimit.xMax);
        targetPos.y = Mathf.Clamp(targetPos.y, camPosLimit.yMin, camPosLimit.yMax);

        if (transform.position != targetPos) { 
            transform.position = Vector3.Lerp(transform.position, targetPos, followSmooth * Time.deltaTime);
            if (Vector3.Distance(targetPos, transform.position)<=0.01f) {
                transform.position = targetPos;
            }
        }
    }
    void OnDrawGizmosSelected(){
        Bounds bounds = new Bounds(new Vector3(camPosLimit.x+camPosLimit.width/2f, camPosLimit.y+camPosLimit.height/2f), new Vector3(camPosLimit.width, camPosLimit.height));
        DebugExtension.DrawBounds(bounds, Color.cyan);

        var newCenter = bounds.center;
        var newSize   = bounds.size;
        newCenter.y   = transform.position.y;
        newSize.y     = 2*maxYOffset;
        bounds.center = newCenter;
        bounds.size   = newSize;
        DebugExtension.DrawBounds(bounds, Color.red);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform lookAtTarget;
    public Vector3 offset;
    public float lookAtOffset;

    public float moveSpeed;
    public float rotaSpeed;

    private Vector3 cameraPos;
    private Quaternion targetRotation;

    private void Update()
    {
        if(lookAtTarget==null)
            return;
        //摄像机偏移设置
        cameraPos = lookAtTarget.position + lookAtTarget.forward * offset.z;
        cameraPos += Vector3.up * offset.y;
        cameraPos += lookAtTarget.right * offset.x;
        
        this.transform.position = Vector3.Lerp(this.transform.position, cameraPos, moveSpeed * Time.deltaTime);

        targetRotation =
            Quaternion.LookRotation(lookAtTarget.position + Vector3.up * lookAtOffset - this.transform.position);
        this.transform.rotation=Quaternion.Slerp(this.transform.rotation,targetRotation,rotaSpeed*Time.deltaTime);
    }

    public void SetLookAtTarget(Transform player)
    {
        lookAtTarget = player;
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    //视角偏移值
    private Transform lookAtTarget;
    public Vector3 offset;
    public float lookAtOffset;

    public float moveSpeed;
    public float rotaSpeed;
    //滚轮调整相关
    public float zoomSpeed = 5f;
    public float minDistance = 2f;
    public float maxDistance = 10f;

    private Vector3 cameraPos;
    private Quaternion targetRotation;
    private float distance;

    private void LateUpdate()
    {
        if(lookAtTarget==null)
            return;
        
        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;
            if (Mathf.Abs(scroll) > 0.01f)
            {
                distance -= scroll * zoomSpeed * Time.deltaTime * 0.2f;
                distance = Mathf.Clamp(distance, minDistance, maxDistance);
            }
        }
        
        cameraPos = lookAtTarget.position - lookAtTarget.forward * distance;
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
        float baseDistance = -offset.z;
        if (baseDistance <= 0f)
        {
            baseDistance = Vector3.Distance(transform.position, lookAtTarget.position);
        }
        distance = Mathf.Clamp(baseDistance, minDistance, maxDistance);
    }
    
}

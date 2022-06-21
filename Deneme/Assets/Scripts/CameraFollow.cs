using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform FpsCamera;
    public Transform Plane;
    private Transform DefaultPos;
    public Transform player;
    public Transform Bow;
    [HideInInspector] public bool CanShoot = false;
    
    [HideInInspector] public bool Final = false;
    private float elapsedTime;
    private float duration = 4f;
    float percentageComplete = 0f;
    Vector3 offset,PlaneOffset;
    Camera Camera;
    void Start()
    {
        DefaultPos = transform;
        offset = transform.position - player.position;
        PlaneOffset = Plane.position - player.position;
        Camera = this.GetComponent<Camera>();
    }

    void Update()
    {
        if(!Final)
        {
            Vector3 targetPos = player.position + offset;
            targetPos.x = 0;
            transform.position = targetPos;

            Vector3 targetPosPlane = player.position + PlaneOffset;
            targetPos.x = 0;
            Plane.position = targetPosPlane;
        }
    }
    
    void LateUpdate()
    {
        if(Final)
        {
            if(Final && percentageComplete < .4)
            {
                
                transform.rotation = Quaternion.Euler(0,0,0);
                elapsedTime += Time.deltaTime;
                percentageComplete = elapsedTime / duration;
                transform.position = Vector3.Lerp(DefaultPos.position,FpsCamera.position,Mathf.SmoothStep(0,1,percentageComplete));
                //Camera.focalLength = Mathf.Clamp(percentageComplete,20,50);
                Camera.focalLength = 50;
            }
            else if(Final && percentageComplete > .4)
            {
                CanShoot = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    
    Transform followtarget; 
    public Vector3 offset = new Vector3(0, 5, -8);

    public void CharSpawned(Transform chacha)
    {
        followtarget = chacha; 
    }

    void LateUpdate()
    {
        if(followtarget != null)
        {
            transform.position = followtarget.position + offset; 
            transform.LookAt(followtarget);
        }
    }

}

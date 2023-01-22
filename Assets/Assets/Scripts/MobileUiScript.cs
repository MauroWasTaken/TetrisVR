using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileUiScript : MonoBehaviour
{
    void Start()
    {
       
        if (SystemInfo.deviceType != DeviceType.Handheld)
        {
            Destroy(this.gameObject);
        }

    }
}

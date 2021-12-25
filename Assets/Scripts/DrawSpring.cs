using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSpring : MonoBehaviour
{
    public float size = 0;

    private void Start()
    {
        this.size = GetComponentInChildren<MeshRenderer>().transform.localScale.x / 2f;
    }

    void Update()
    {
        foreach (var spring in GetComponents<SpringJoint>())
        {
            if(spring.connectedBody.transform.name.Contains("Anchor")) continue;

            var otherPosition = spring.connectedBody.position;
            var ownPosition = this.transform.position;
            
            Vector3 vectorToTarget = otherPosition - ownPosition;
            vectorToTarget.Normalize();
            Debug.DrawLine(ownPosition + vectorToTarget * this.size, otherPosition - vectorToTarget * spring.connectedBody.GetComponentInChildren<SpringJoint>().transform.localScale.x / 2f, Color.HSVToRGB(0, 0, 0.1f));
        }
    }
}

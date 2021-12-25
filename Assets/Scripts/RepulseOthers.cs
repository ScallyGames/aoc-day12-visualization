using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RepulseOthers : MonoBehaviour
{
    [SerializeField] private float targetDistance = 10;
    private new Rigidbody rigidbody;

    private void Start()
    {
        this.rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var others = GameObject.FindObjectsOfType<RepulseOthers>();

        foreach (var other in others)
        {
            if(other.gameObject == this.gameObject) continue;

            var vectorToObject = (this.transform.position - other.transform.position);
            this.rigidbody.AddForce(vectorToObject.normalized * targetDistance / vectorToObject.magnitude);
        }
    }
}

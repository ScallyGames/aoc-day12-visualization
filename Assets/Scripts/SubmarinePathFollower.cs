using System;
using System.Collections.Generic;
using UnityEngine;

public class SubmarinePathFollower : MonoBehaviour
{
    public float speed = 10;
    public List<Cave> path;
    private int nextIndex = 0;


    private void Update()
    {
        if (path == null) return;

        if (nextIndex >= path.Count)
        {
            Destroy(this.gameObject);
            // GameObject.FindObjectOfType<SubmarineSpawner>().SpawnNext();
            return;
        }

        if (nextIndex == 0)
        {
            this.transform.position = path[0].GameObject.transform.position;
            nextIndex++;
            return;
        }

        var targetPosition = path[nextIndex].GameObject.transform.position;
        var position = this.transform.position;
        var vectorToTarget = targetPosition - position;
        
        position = position + vectorToTarget.normalized * Math.Min(vectorToTarget.magnitude, speed * Time.deltaTime);

        this.transform.right = vectorToTarget.normalized;

        if ((targetPosition - position).sqrMagnitude < 0.001f) nextIndex++;
        
        this.transform.position = position;
    }
}

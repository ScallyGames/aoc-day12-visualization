using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CaveSystemGenerator : MonoBehaviour
{
    [SerializeField] private GameObject caveBigPrefab;
    [SerializeField] private GameObject caveSmallPrefab;
    [SerializeField] private Rigidbody startAnchor;
    [SerializeField] private Rigidbody endAnchor;

    string[] lines = new []
    {
        // "start-A",
        // "start-b",
        // "A-c",
        // "A-b",
        // "b-d",
        // "A-end",
        // "b-end",
        "zi-end",
        "XR-start",
        "zk-zi",
        "TS-zk",
        "zw-vl",
        "zk-zw",
        "end-po",
        "ws-zw",
        "TS-ws",
        "po-TS",
        "po-YH",
        "po-xk",
        "zi-ws",
        "zk-end",
        "zi-XR",
        "XR-zk",
        "vl-TS",
        "start-zw",
        "vl-start",
        "XR-zw",
        "XR-vl",
        "XR-ws",
    };

    public Dictionary<string, Cave> caves = new Dictionary<string, Cave>();
    
    
    void Start()
    {
        foreach(var line in lines)
        {
            var connection = line.Split('-');

            for(int i = 0; i < connection.Length; i++)
            {
                if(!caves.ContainsKey(connection[i]))
                {
                    Cave newCave = new Cave(connection[i]);
                    caves.Add(connection[i], newCave);
                    GameObject newCaveObject =
                        Instantiate(newCave.IsLargeCave ? caveBigPrefab : caveSmallPrefab);
                    newCaveObject.transform.position = Random.insideUnitCircle.normalized * 2;
                    newCaveObject.GetComponentInChildren<TextMeshPro>().text = newCave.Name;
                    newCave.GameObject = newCaveObject;

                    if (newCave.Name == "start")
                    {
                        SpringJoint spring = newCaveObject.AddComponent<SpringJoint>();
                        spring.connectedBody = startAnchor;
                        spring.autoConfigureConnectedAnchor = false;
                        spring.connectedAnchor = Vector3.zero;
                        spring.damper = 0.9f;
                        spring.spring = 200;
                    }
                    
                    if (newCave.Name == "end")
                    {
                        SpringJoint spring = newCaveObject.AddComponent<SpringJoint>();
                        spring.connectedBody = endAnchor;
                        spring.autoConfigureConnectedAnchor = false;
                        spring.connectedAnchor = Vector3.zero;
                        spring.damper = 0.9f;
                        spring.spring = 200;
                    }
                }
            }

            for(int i = 0; i < connection.Length; i++)
            {
                caves[connection[i]].AdjacentCaves.Add(caves[connection[connection.Length - i - 1]]);
                SpringJoint spring = caves[connection[i]].GameObject.AddComponent<SpringJoint>();
                spring.connectedBody = caves[connection[connection.Length - i - 1]].GameObject.GetComponent<Rigidbody>();
                spring.damper = 0.9f;
                spring.minDistance = 1.2f;
                spring.maxDistance = 6;
                spring.autoConfigureConnectedAnchor = false;
                spring.connectedAnchor = Vector3.zero;
            }
        }
    }
}

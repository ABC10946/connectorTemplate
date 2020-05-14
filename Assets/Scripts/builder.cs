using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class builder : MonoBehaviour
{
    [SerializeField]
    private GameObject node;

    private int lastNodeId;

    // Start is called before the first frame update
    void Start()
    {
        lastNodeId = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.Space)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                Debug.Log(hit.collider.tag);
                if (hit.collider.tag != "Node") {
                    GameObject iNode = Instantiate(node, hit.point, Quaternion.identity).gameObject;
                    iNode.GetComponent<NodeData>().nodeId = lastNodeId++;
                }
            }
        }
    }
}

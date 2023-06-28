using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private NavMeshAgent agent;
    private Camera mycamera;
    private Ray ray;
    public bool iswinner = false;
    public bool islose = false;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mycamera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        ray = mycamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(1))        // Player Control
        {
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Agent"))
        {
            islose = true;
        }
        if (collision.gameObject.CompareTag("Escape"))
        {
            iswinner = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform reciever;

    private bool isPlayerOverlapping = false;

    void Update()
    {
        if (isPlayerOverlapping)
        {
            Vector3 portalToPlayer = player.position - transform.position;
            float dot = Vector3.Dot(transform.forward.normalized, portalToPlayer.normalized);
            Debug.Log(dot);
            if (dot < 0.0f)
            {
                float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
                rotationDiff += 180.0f;
                player.Rotate(Vector3.up, rotationDiff);
                Vector3 posOffset = Quaternion.Euler(0.0f, rotationDiff, 0.0f) * portalToPlayer;
                player.position = reciever.position + posOffset;

                isPlayerOverlapping = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("coœ wesz³o: " + other.tag);
        if (other.CompareTag("MainCamera"))
        {
            isPlayerOverlapping = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            isPlayerOverlapping = false;
        }
    }
}

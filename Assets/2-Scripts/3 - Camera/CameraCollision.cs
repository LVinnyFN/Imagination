using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{

    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;
    private float distance;
    private Vector3 dollyDir;

    void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit))
        {
            if (!hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.CompareTag("Weapon") && !hit.collider.gameObject.CompareTag("HitPoint") && !hit.collider.gameObject.CompareTag("Drop"))
            {
                distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            }
        }
        else distance = maxDistance;

        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }
}

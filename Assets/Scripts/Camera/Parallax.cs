using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxSpeed = 1f;

    float xPosition;
    float length;

    Camera cam;

    private void Start()
    {
        cam = Camera.main;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    private void FixedUpdate()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxSpeed);
        float distanceToMove = cam.transform.position.x * parallaxSpeed;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if(distanceMoved > xPosition + length)
            xPosition = xPosition + length;
        else if(distanceMoved < xPosition - length)
            xPosition = xPosition - length;
    }
}

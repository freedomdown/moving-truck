using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowMover : MonoBehaviour
{
    public float SnapPoint = 0.1f;
    public float MoveSpeed = 1f;
    public float distance = 400f;
    private bool Moved = false;
    private Vector3 TargetPosition;

    // Start is called before the first frame update
    void Start()
    {
        TargetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != TargetPosition)
        {
            Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * MoveSpeed);
            transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * MoveSpeed);

            if (Vector3.Distance(transform.position, TargetPosition) <= SnapPoint)//snap to target point if we are close enough
                transform.position = TargetPosition;
        }
    }

    [ContextMenu("Toggle")]
    public void Toggle()
    {
        if (Moved)
            TargetPosition = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
        else
            TargetPosition = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
        Moved = !Moved;
    }
}

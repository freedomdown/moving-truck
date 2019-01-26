using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class GridMovement : MonoBehaviour
{
    public bool GravityActive = true;
    public float Gravity = 20f;
    public float GridSpacing = 1f;
    private Vector2 Axis = new Vector2(0f, -1f);
    private Rigidbody2D body;

    [Header("RayCast")]
    public Transform OnTopOfMe;//what obj is on top of this one, if any
    public float RayCastSize = 1f;//width of raycast
    public float RayCastDistance = 2f;//distance to go for ray cast
    public float RayCastOffset = 0f;//lateral offset for ray cast

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0f;
        body.freezeRotation = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //align to grid
        if (transform.position.x % GridSpacing != 0)//if not already aligned
        {
            int gridNum = (int)(transform.position.x / GridSpacing);
            transform.position = new Vector3(gridNum * GridSpacing, transform.position.y, transform.position.z);
        }

        //then fall if able
        if (GravityActive)
        {
            body.AddForce(Axis * (Gravity * body.mass));//add force to push body down
        }

        //find object directly "up" of us, if any
        if (OnTopOfMe == null)
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position + (Vector3.right * RayCastOffset), new Vector2(RayCastSize, 0.2f), 0f, Vector2.up, RayCastDistance);
            if (hit.collider != null)
                OnTopOfMe = hit.transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        //Gizmos.DrawWireCube(transform.position, new Vector2(RayCastSize, 0.2f));
        Gizmos.DrawWireCube((transform.position + (Vector3.up * RayCastDistance)) + (Vector3.right * RayCastOffset), new Vector2(RayCastSize, 0.2f));
    }

    public void Fall()
    {
        GravityActive = true;
    }
}

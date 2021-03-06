﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class GridMovement : MonoBehaviour
{
    public bool GravityActive = false;//set to true when the item is ready to drop
    public float Gravity = 20f;//force of gravity
    public float GridSpacing = 1f;
    private Vector2 Axis = new Vector2(0f, -1f);//direction of gravity

    private Rigidbody2D body;
    private BoxCollider2D col;
    private Durability dur;

    [Header("RayCast")]
    public Transform OnTopOfMe;//what obj is on top of this one, if any
    public float RayCastSize = 1f;//width of raycast
    public float RayCastDistance = 2f;//distance to go for ray cast
    public float RayCastOffset = 0f;//lateral offset for ray cast

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0f;//disable unity's control of gravity
        body.freezeRotation = true;//don't let teh physics system rotate things when they collide

        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;

        dur = GetComponent<Durability>();
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
            body.AddForce(Axis * (Gravity * body.mass));//add force to push body down

        //find object directly "up" of us, if any
        if (OnTopOfMe == null && GravityActive)
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position + (Vector3.right * RayCastOffset), new Vector2(RayCastSize, 0.2f), 0f, Vector2.up, RayCastDistance, LayerMask.GetMask("Items"));
            if (hit.collider != null && hit.collider.GetComponent<GridMovement>().GravityActive)
                OnTopOfMe = hit.transform;
            if (OnTopOfMe == transform)
                OnTopOfMe = null;//make sure we haven't detected ourselves
        }
        else
        {
            if (dur != null)
            {
                float weightOnTop = GetWeight(null);//get the total weight on top of me
                weightOnTop -= dur.Weight;//remove own weight from total
                if (weightOnTop >= dur.Strength)//is weight high enough to break me
                    dur.Break();
            }
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
        GravityActive = true;//turn gravity back on
        if (GetComponent<ItemClick>() != null)
            Destroy(GetComponent<ItemClick>());
        col.isTrigger = false;
        col.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Items");
    }

    public float GetWeight(GameObject caller)
    {
        if (GravityActive == false)
            return 0f;

        if (caller == gameObject)
            return 0f;

        float topWeight = 0f;

        if (OnTopOfMe != null)
        {
            //get weight
            topWeight = OnTopOfMe.GetComponent<GridMovement>().GetWeight(gameObject);
        }

        //then add your own weight to the total weight and pass it along
        if (dur != null)
            return dur.Weight + topWeight; //return my weight + weight from obj on top of me
        else
            return 1f + topWeight;
    }
}

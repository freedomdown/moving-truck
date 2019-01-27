using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class Durability : MonoBehaviour
{
    public bool IsFragile = false;
    public float Weight = 1f;

    public void Break()
    {
        //too much weight
        //Destroy self?
    }
}

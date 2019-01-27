using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class Durability : MonoBehaviour
{
    public float Strength = 4f;
    public float Weight = 1f;
    public AudioSource breakSnd;


    public void Break()
    {
        //too much weight
        //Destroy self?
        Destroy(gameObject);
        Debug.Log("Break!");
        breakSnd.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class Durability : MonoBehaviour
{
    public float Strength = 4f;
    public float Weight = 1f;
    public float DestructionMagnitutde = 0.02f;
    public AudioSource breakSnd;
    private bool breaking = false;


    public void Break()
    {
        //too much weight
        if (!breaking) {
            //Destroy self?
            breakSnd.Play();
            breaking = true;
            Destroy(gameObject, breakSnd.clip.length);
        }
    }

    private void Update() {
        if (breaking) {
            SpriteRenderer sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
            Vector3 offset = Random.insideUnitCircle * DestructionMagnitutde;
            sprite.transform.position = sprite.transform.position + offset;
            sprite.transform.rotation = Quaternion.Slerp(sprite.transform.rotation, Random.rotation, DestructionMagnitutde);
        }
    }
}

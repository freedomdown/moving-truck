using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By: Ryan Dallaire
namespace RyanDStandard
{
    namespace Physics
    {
        [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
        public class CustomGravity2D : MonoBehaviour
        {
            [Header("Planetary Gravity stuff")]
            public bool PlanetaryGravity = false;
            public bool RotateToMatchPlanet = true;
            public Transform planet;
            [Header("Rest of Gravity stuff")]
            public bool GravityActive = true;
            public bool InvertGravity = false;
            public float Gravity = 30f;
            public Vector2 Axis = new Vector2(0f, -1f);
            private Rigidbody2D body;

            // Use this for initialization
            void Start()
            {
                body = GetComponent<Rigidbody2D>();
                body.gravityScale = 0f;
                if (PlanetaryGravity && planet != null && RotateToMatchPlanet)
                {
                    PlanetRotation();
                    body.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }

            // Update is called once per frame
            void FixedUpdate()
            {
                if (GravityActive)
                {
                    if (!PlanetaryGravity)
                    {
                        if (!InvertGravity)
                            body.AddForce(Axis * (Gravity * body.mass));
                        else
                            body.AddForce(Axis * (-Gravity * body.mass));
                    }
                    else
                    {
                        if (planet != null)
                        {
                            var heading = transform.position - planet.position;
                            var distance = heading.magnitude;
                            var direction = heading / distance;

                            //PlanetRotation();
                            if (RotateToMatchPlanet)
                                transform.up = direction;
                            if (!InvertGravity)
                                body.AddForce(-direction * (Gravity * body.mass));
                            else
                                body.AddForce(-direction * (-Gravity * body.mass));
                        }
                    }
                }
            }
            public void ResetToEarthGravity()
            {
                Gravity = 30f;
                Axis = new Vector2(0f, -1f);
            }

            private void PlanetRotation()
            {
                var heading = transform.position - planet.position;
                var distance = heading.magnitude;
                var direction = heading / distance;

                //if ((transform.up - direction).magnitude > 0.1f)
                //    transform.up = Vector3.Slerp(transform.up, direction, 0.1f);//seems to cause issues
                transform.up = direction;
            }
        }
    }
}
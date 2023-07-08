using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class massx : MonoBehaviour
{

    [SerializeField] Transform pivot;

    Transform mass;
    LineRenderer massLR;
    Rigidbody2D massRB;


    float angle;        //IN RADIANS
    float sine;
    float cosine;

    Vector2 gravity;

    Vector2 direction;
    float distance;
    float force_x;
    float force_y;
    int direction_x;
    Vector2 tensionForce;


    float length = 5.5f;
    //float T = 3.01f;
    //float T = 1.23f;     //pi*sqrt(length/g)/2
    [SerializeField] float PI_mult;
    float pi_half = Mathf.PI;




    private void Awake()
    {
        mass = transform;
        massLR = mass.GetComponent<LineRenderer>();
        massRB = mass.GetComponent<Rigidbody2D>();
        massLR.positionCount = 2;
        massLR.SetPosition(0, mass.position);
        massLR.SetPosition(1, pivot.position);
        gravity = Physics2D.gravity;
    }


    private void FixedUpdate()
    {
        distance = Vector2.Distance(mass.position, pivot.position);

        if (distance >= length-0.5f)
        {
            pi_half = Mathf.PI / PI_mult;

            direction = pivot.position - mass.position;
            angle = Vector2.Angle(direction, Vector2.up) * Mathf.PI / 180;       //RADIANS
            sine = Mathf.Sin(angle);
            cosine = Mathf.Cos(angle);

            if (mass.position.x < pivot.position.x) direction_x = 1;
            else direction_x = -1;
            float tension = -gravity.y * cosine;
            force_y = tension * pi_half;
            force_x = force_y * sine * direction_x;
            tensionForce = new Vector2(force_x, force_y);

            massRB.AddForce(tensionForce, ForceMode2D.Force);
        }

    }



    private void LateUpdate()
    {
        massLR.SetPosition(0, mass.position);
    }


}

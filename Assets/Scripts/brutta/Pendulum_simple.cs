using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum_simple : MonoBehaviour
{

    [Header("Objects")]
    [SerializeField] Transform mass;
    [SerializeField] Transform pivot;

    [Header("Components")]
    Rigidbody2D massRB;
    LineRenderer massLR;


    [SerializeField] float ropeLength;

    [Header("Forces")]
    Vector2 gravityForce;
    //Vector2 externalForce;
    //float externalForce_x;
    //float externalForce_y;

    float tension;
    Vector2 tensionForce;
    Vector2 accelerationForce;

    Vector2 direction;              //FROM OBJECT TO PIVOT
    float distance;               //OBJECT - PIVOT
    float angle;
    float cosine;
    float sine;
    bool goingUp;
    int x_verse;

    [SerializeField] float frictionConstant;
    //float friction;
    Vector2 frictionForce;






    private void Awake()
    {
        gravityForce = Physics2D.gravity;
        massRB = mass.GetComponent<Rigidbody2D>();
        massLR = mass.GetComponent<LineRenderer>();
        massLR.positionCount = 2;
        goingUp = false;

        Time.timeScale = .2f;
    }



    private void FixedUpdate()
    {

        //CALCULATE FORCES
        direction = (pivot.position - mass.position).normalized;
        angle = Vector2.Angle(direction, Vector2.up);
        cosine = Mathf.Cos(angle * Mathf.PI / 180f);
        sine = Mathf.Sin(angle * Mathf.PI / 180f);
        /*

        if (massRB.velocity.x * tensionForce.x < 0) externalForce_x = -massRB.velocity.x;   //IF EXTERNAL X VELOCITY AND X TENSION FORCE HAVE OPPOSITE SIGN
        if (massRB.velocity.y < 0) externalForce_y = -massRB.velocity.y;
        //externalForce = new Vector2(externalForce_x / cosine, externalForce_y * cosine);
        externalForce = new Vector2(externalForce_x, externalForce_y);
        */
        distance = Vector2.Distance(mass.position, pivot.position);



        frictionForce = -frictionConstant * massRB.velocity;


        //tension = gravityForce.magnitude * Mathf.Cos(angle*Mathf.PI/180f);
        //tensionForce = direction * tension;


        //if (massRB.velocity.x < 0 && tensionForce.x > 0) tensionForce.x = -massRB.velocity.x;
        //if (massRB.velocity.y < 0) tensionForce.y = -massRB.velocity.y;


        //APPLY FORCES
        //massRB.AddForce(tensionForce, ForceMode2D.Force);
        //massRB.AddForce(frictionForce, ForceMode2D.Force);

        if (massRB.velocity.y > 0) goingUp = true;
        else goingUp = false;

        if ( (goingUp && mass.position.x > pivot.position.x) || (!goingUp && mass.position.x < pivot.position.x) ) x_verse = 1;
        else x_verse = -1;
        Debug.Log("VELOCITY");
        Debug.Log(massRB.velocity);
        Debug.Log(mass.position.x);
        Debug.Log(pivot.position.x);
        float totalX = -gravityForce.y * cosine * x_verse;
        float totalY = gravityForce.y - gravityForce.y * cosine * cosine;
        massRB.velocity = new Vector2(totalX, totalY);// * Mathf.Max((1 - frictionConstant * timer), 0);

    }


    private void LateUpdate()
    {
        massLR.SetPosition(0, transform.position);
        massLR.SetPosition(1, pivot.position);
    }

}

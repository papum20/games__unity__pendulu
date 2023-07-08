using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
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
    [SerializeField] float K;       //HOOKE'S CONSTANT
    float X;                        //LENGTH CHANGE
    Vector2 accelerationForce;

    Vector2 direction;              //FROM OBJECT TO PIVOT
    float distance;               //OBJECT - PIVOT
    //float angle;
    //float cosine;
    //float sine;

    [SerializeField] float frictionConstant;
    //float friction;
    Vector2 frictionForce;



    


    private void Awake()
    {
        gravityForce = Physics2D.gravity;
        massRB = mass.GetComponent<Rigidbody2D>();
        massLR = mass.GetComponent<LineRenderer>();
        massLR.positionCount = 2;
        //Time.timeScale = .1f;
    }



    private void FixedUpdate()
    {
        //CALCULATE FORCES
        direction = (pivot.position - mass.position).normalized;
        //angle = Vector2.Angle(direction, Vector2.up);
        //cosine = Mathf.Cos(angle * Mathf.PI / 180f);
        //sine = Mathf.Sin(angle * Mathf.PI / 180f);
        /*

        if (massRB.velocity.x * tensionForce.x < 0) externalForce_x = -massRB.velocity.x;   //IF EXTERNAL X VELOCITY AND X TENSION FORCE HAVE OPPOSITE SIGN
        if (massRB.velocity.y < 0) externalForce_y = -massRB.velocity.y;
        //externalForce = new Vector2(externalForce_x / cosine, externalForce_y * cosine);
        externalForce = new Vector2(externalForce_x, externalForce_y);
        */
        distance = Vector2.Distance(mass.position, pivot.position);
        X = Mathf.Max(distance - ropeLength, 0);               //x ONLY IF >0 ELSE 0
        //X = distance - ropeLength;
        tension = K * X;                                            //HOOKE'S LAW
        tensionForce = tension * direction;


        frictionForce = -frictionConstant * massRB.velocity;


        //tension = gravityForce.magnitude * Mathf.Cos(angle*Mathf.PI/180f);
        //tensionForce = direction * tension;


        //if (massRB.velocity.x < 0 && tensionForce.x > 0) tensionForce.x = -massRB.velocity.x;
        //if (massRB.velocity.y < 0) tensionForce.y = -massRB.velocity.y;


        //APPLY FORCES
        massRB.AddForce(tensionForce, ForceMode2D.Force);
        //massRB.AddForce(frictionForce, ForceMode2D.Force);

    }


    private void LateUpdate()
    {
        massLR.SetPosition(0, transform.position);
        massLR.SetPosition(1, pivot.position);
    }

}

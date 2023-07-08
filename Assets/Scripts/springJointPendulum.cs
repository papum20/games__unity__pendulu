using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class springJointPendulum : MonoBehaviour
{

    [SerializeField] Transform pivot;
    Transform mass;

    [Header("Components")]
    Rigidbody2D massRB;
    SpringJoint2D joint;
    LineRenderer massLR;

    [Header("Specifications")]
    [SerializeField] float ropeLength;

    [Header("Global")]
    [SerializeField] float frictionConstant;    //0.05
    float dampingTolerance = 95f / 100f;
    float maxDamping_Distance = 9f;
    float dflt_damping = 0.1f;

    Vector2 startPosition;
    float jointBreakForce = 1000f;
    float jointMinBreak = 15f;
    float breakTimer = 25f;

    [Header("Variables")]
    float distance;
    Vector2 frictionForce;
    bool touching;
    bool broken;







    private void Awake()
    {
        mass = transform;
        massRB = mass.GetComponent<Rigidbody2D>();
        joint = mass.GetComponent<SpringJoint2D>();
        massLR = mass.GetComponent<LineRenderer>();

        massLR.positionCount = 2;
        massLR.SetPosition(1, pivot.position);

        joint.autoConfigureConnectedAnchor = false;
        joint.autoConfigureDistance = false;
        joint.connectedAnchor = pivot.position;
        joint.distance = 6f;
        joint.breakForce = jointBreakForce;

        startPosition = new Vector2(-5.5f, 8);
        broken = false;
    }

    private void Update()
    {
        distance = Vector2.Distance(mass.position, pivot.position);

        //JOIN/DISJOIN JOINT
        if (joint != null)
        {
            if (touching == true || distance < ropeLength * dampingTolerance)
                joint.enabled = false;
            else
            {
                joint.enabled = true;
                if (ropeLength - distance > maxDamping_Distance) joint.dampingRatio = 1;
                else joint.dampingRatio = dflt_damping;
            }
        }
        else if (broken == false)            //IF JOINT BROKE
        {
            massLR.positionCount = 1;
            broken = true;
        }



        //SLOWLY BECOMES MORE LIKELY TO BREAK
        if (mass.gameObject.activeSelf && joint != null && joint.breakForce > jointMinBreak)
            joint.breakForce -= jointBreakForce / breakTimer * Time.deltaTime;
        
    }


    private void FixedUpdate()
    {
        //FRICTION FORCE
        frictionForce = -massRB.velocity * frictionConstant;
        massRB.AddForce(frictionForce, ForceMode2D.Force);
    }

    private void LateUpdate()
    {
        Draw_Line();
    }





    void Draw_Line()
    {
        massLR.SetPosition(0, mass.position);
    }


    public void Move(Vector2 newPosition)
    {
        massRB.velocity = Vector2.zero;
        mass.position = newPosition;
        if(joint != null)
            joint.enabled = false;
        touching = true;
    }

    public void Throw(Vector2 newForce)
    {
        massRB.AddForce(newForce, ForceMode2D.Impulse);
        touching = false;
    }

    public void RestartMass()
    {
        massRB.velocity = Vector2.zero;
        mass.position = startPosition;

        if (joint == null)
        {
            gameObject.AddComponent(typeof(SpringJoint2D));
            joint = mass.GetComponent<SpringJoint2D>();
            joint.autoConfigureConnectedAnchor = false;
            joint.autoConfigureDistance = false;
            joint.connectedAnchor = pivot.position;
            joint.distance = 6f;
            joint.breakForce = jointBreakForce;
        }

        massLR.positionCount = 2;
        massLR.SetPosition(1, pivot.position);

        broken = false;
    }
}

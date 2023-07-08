using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    [Header("Objects")]
    Camera mainCamera;
    [SerializeField] Transform mass;

    [Header("Components")]

    [Header("Global")]
    [SerializeField] LayerMask whatIsMass;
    [SerializeField] float forceDivider;                //FORCE TO APPLY TO MASS; = 1
    [SerializeField] float touchPrecision;              //TIMES THE MASS RADIUS, =DISTANCE FROM MASS TO TOUCH IT; =1

    [Header("Variables")]
    Transform massTouched;
    Transform lastMassTouched;
    Vector2 touchPosition;
    Vector2 worldTouchPosition;
    Vector2 lastWorldTouchPosition;
    Vector2 lastTouchDifference;
    bool touching;





    private void Awake()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        touching = false;
    }



    private void Update()
    {

        if(Input.GetMouseButton(0))
        {
            lastMassTouched = massTouched;
            massTouched = null;

            lastTouchDifference = worldTouchPosition - lastWorldTouchPosition;
            lastWorldTouchPosition = worldTouchPosition;
            touchPosition = Input.mousePosition;
            worldTouchPosition = mainCamera.ScreenToWorldPoint(touchPosition);
            Ray touchRay = mainCamera.ScreenPointToRay(touchPosition);

            //RaycastHit2D hitInfo = Physics2D.Raycast(worldTouchPosition, touchRay.direction, whatIsMass);
            RaycastHit2D hitInfo = Physics2D.CircleCast(worldTouchPosition, mass.localScale.x/2 * touchPrecision, touchRay.direction, whatIsMass);
            if (hitInfo.collider != null)
            {
                massTouched = hitInfo.collider.transform;
                Move_Mass(massTouched);
            }
            else if(touching)
                AddForce_Mass(lastMassTouched);
        }

        else if (Input.touchCount > 0)
        {
            lastTouchDifference = worldTouchPosition - lastWorldTouchPosition;
            lastWorldTouchPosition = worldTouchPosition;
            lastMassTouched = massTouched;
            massTouched = null;

            RaycastHit2D hitInfo;
            for (int i = 0; i < Input.touchCount; i++)
            {
                touchPosition = Input.GetTouch(i).position;
                worldTouchPosition = mainCamera.ScreenToWorldPoint(touchPosition);
                Ray touchRay = mainCamera.ScreenPointToRay(touchPosition);

                hitInfo = Physics2D.Raycast(worldTouchPosition, touchRay.direction, whatIsMass);
                if (hitInfo.collider != null)
                {
                    massTouched = hitInfo.collider.transform;
                    break;
                }
            }
          
            if (massTouched != null)
                Move_Mass(massTouched);
            else if (touching == true)
                AddForce_Mass(lastMassTouched);
        }


        else if (touching == true)
            AddForce_Mass(massTouched);

    }










    void Move_Mass(Transform massTouched)
    {
        if (Time.timeScale != 0f)
        {
            massTouched.GetComponent<springJointPendulum>().Move(worldTouchPosition);

            touching = true;
        }
    }


    void AddForce_Mass(Transform massTouched)
    {
        if (Time.timeScale != 0f)
        {
            Vector2 newForce = lastTouchDifference / Time.deltaTime;
            massTouched.GetComponent<springJointPendulum>().Throw(newForce / forceDivider);

            touching = false;
        }
    }
    
}

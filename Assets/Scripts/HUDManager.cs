using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class HUDManager : MonoBehaviour
{
    
    [Header("Objects")]
    [SerializeField] GameObject hud;
    [SerializeField] GameObject menu;
    [SerializeField] GameManager gameManager;

    [SerializeField] Transform pivot;
    [SerializeField] GameObject mass;
    Rigidbody2D massRB;
    LineRenderer massLR;
    SpringJoint2D joint;

    [Header("Variables")]
    bool hasStarted;

    [Header("Graphic Raycast")]
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;





    private void Awake()
    {
        massRB = mass.GetComponent<Rigidbody2D>();
        massLR = mass.GetComponent<LineRenderer>();
        joint = mass.GetComponent<SpringJoint2D>();

        hasStarted = false;

        //GRAPHIC RAYCAST
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
    }



    private void Update()
    {
        //TOUCH TO START, DON'T TOUCH PAUSE BUTTON
        if (hasStarted == false)
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            List<RaycastResult> results = new List<RaycastResult>();

            if (Input.GetMouseButton(0))
            {
                m_PointerEventData.position = Input.mousePosition;

                m_Raycaster.Raycast(m_PointerEventData, results);

                if (results.Count == 0 || results[0].gameObject.tag != "pause")
                {
                    hasStarted = true;
                    gameManager.Start_Function();
                }
            }
            else if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    m_PointerEventData.position = Input.GetTouch(i).position;

                    m_Raycaster.Raycast(m_PointerEventData, results);

                    if (results.Count == 0 || results[0].gameObject.tag != "pause")
                    {
                        hasStarted = true;
                        gameManager.Start_Function();
                    }
                }
            }
        }

    }







    #region BUTTONS

    public void PauseButton_Function()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
        hud.SetActive(false);
    }

    public void RestartButton_Function()
    {
        mass.GetComponent<springJointPendulum>().RestartMass();
    }

    #endregion

}

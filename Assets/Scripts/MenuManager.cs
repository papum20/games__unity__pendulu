using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MenuManager : MonoBehaviour
{

    [Header("Objects")]
    Camera mainCamera;

    [SerializeField] GameObject hud;
    [SerializeField] GameObject menu;

    [SerializeField] Transform mass;

    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject colorsPanel;
    [SerializeField] RawImage colorsWheel;

    [Header("Components")]
    [SerializeField] SpriteRenderer massSR;
    Texture colorsTexture;

    [Header("Graphic Raycast")]
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    [Header("Resources")]
    [SerializeField] Texture2D roundCursor;
    [SerializeField] Texture2D baseCursor;

    Vector2 roundCursor_offset;

    [Header("Variables")]
    Vector2Int colorsRectSize;          //IMAGE SIZE IN PIXELS
    Vector2Int textureSize;             //TEXTURE SIZE IN PIXELS
    Vector2Int colorsPosition;          //IMAGE POSITION IN PIXELS
    Vector2Int m_posInTexture;          //MousePositionInTexture
    Vector2Int t_posInTexture;          //TouchPositionInTexture






    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        colorsTexture = colorsWheel.texture;

        colorsPanel.SetActive(false);
        menu.SetActive(false);

        roundCursor_offset = new Vector2(roundCursor.width/2, roundCursor.height/2);

        //GRAPHIC RAYCAST
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
    }






    private void Update()
    {

        //VARIABLES FOR CHANGE COLOR FUNCTION

        Vector2Int colorsRectSize = Vector2Int.RoundToInt(colorsWheel.rectTransform.rect.size) * Screen.width / 1280;        //IMAGE SIZE IN PIXELS
        Vector2Int textureSize = new Vector2Int(colorsTexture.width, colorsTexture.height);                                  //TEXTURE SIZE IN PIXELS
        Vector2Int colorsPosition = Vector2Int.RoundToInt(colorsWheel.transform.position);                                   //IMAGE POSITION IN PIXELS

        //MAKE BOTTOM-LEFT CORNER OF IMAGE AND SCREEN MATCH
        m_posInTexture = Vector2Int.RoundToInt(Input.mousePosition) - colorsPosition + colorsRectSize / 2;   //MousePositionInTexture
        //USE A PROPORTION TO MAKE BOTTOM-LEFT CORNER OF IMAGE AND TEXTURE (POSITIONED IN BOTTOM-LEFT CORNER OF SCREEN) MATCH
        m_posInTexture = m_posInTexture * textureSize.x / colorsRectSize.x;


        //CHANGE MASS COLOR

        m_PointerEventData = new PointerEventData(m_EventSystem);
        List<RaycastResult> results_mouse = new List<RaycastResult>();
        List<RaycastResult> results_touch = new List<RaycastResult>();

        //GET MOUSE INPUT
        m_PointerEventData.position = Input.mousePosition;
        m_Raycaster.Raycast(m_PointerEventData, results_mouse);

        //GET TOUCH INPUT
        for (int i = 0; i < Input.touchCount; i++)
        {
            m_PointerEventData.position = Input.GetTouch(i).position;
            m_Raycaster.Raycast(m_PointerEventData, results_touch);

            if (results_touch[0].gameObject.tag == "color")
            {
                t_posInTexture = Vector2Int.RoundToInt(Input.GetTouch(i).position) - colorsPosition + colorsRectSize / 2;   //TouchPositionInTexture
                t_posInTexture = t_posInTexture * textureSize.x / colorsRectSize.x;
                break;
            }
        }

        //CHECK IF TOUCHED COLOR WHEEL
        if (results_mouse.Count > 0 && results_mouse[0].gameObject.tag == "color" && (Vector2Int.Distance(m_posInTexture, textureSize / 2) <= textureSize.x / 2))
        {
            Cursor.SetCursor(roundCursor, roundCursor_offset, CursorMode.Auto);
            if (Input.GetMouseButton(0))
                ChangeColor(m_posInTexture);
        }

        else if (results_touch.Count > 0 && results_touch[0].gameObject.tag == "color" && (Vector2Int.Distance(t_posInTexture, textureSize / 2) <= textureSize.x / 2))
        {
            ChangeColor(t_posInTexture);
        }
        else
        {
            Cursor.SetCursor(baseCursor, Vector2.zero, CursorMode.Auto);
        }
        


    }






    #region FUNCTIONS

    public void ChangeColor(Vector2Int posInTexture)
    {
        if (colorsPanel.activeInHierarchy == true)
        {
            Color newColor = (colorsWheel.texture as Texture2D).GetPixel(posInTexture.x, posInTexture.y);
            massSR.color = newColor;
        }

    }

    #endregion



    #region BUTTONS

    public void QuitButton_Function()
    {
        Application.Quit();
    }

    public void ReturnButton_Function()
    {
        hud.SetActive(true);
        Time.timeScale = 1;
        menu.SetActive(false);
    }

    public void ColorButton_Function()
    {
        colorsPanel.SetActive(true);
        menuPanel.SetActive(false);

        //MOVE PANEL ON OPPOSITE SIDE OF SCREEN OF MASS
        if (mass.position.x * (colorsPanel.transform.position.x - Screen.width/2) > 0)
            colorsPanel.transform.position = new Vector2(Screen.width - colorsPanel.transform.position.x, colorsPanel.transform.position.y);
            
    }

    public void ConfirmButton_Function()
    {
        menuPanel.SetActive(true);
        colorsPanel.SetActive(false);
    }

    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("Objects")]
    [SerializeField] GameObject mass;

    [SerializeField] GameObject menu;
    [SerializeField] Text startWriting;
    [SerializeField] GameObject restartButton;

    [Header("Variables")]
    int fontMin = 40;
    int fontMax = 70;
    float fontPeriod = 0.8f;    //FROM FONT_MIN TO FONT_MAX
    float timer;

    [Header("Resources")]
    [SerializeField] Texture2D baseCursor;




    private void Awake()
    {
        mass.SetActive(false);
        restartButton.SetActive(false);
        startWriting.gameObject.SetActive(true);

        Cursor.SetCursor(baseCursor, Vector2.zero, CursorMode.Auto);

        timer = 0f;  
    }


    private void Update()
    {
        if(startWriting.gameObject.activeInHierarchy == true)
            StartTextSizing();
    }








    void StartTextSizing()
    {
        float timeExpression = Mathf.Abs( Mathf.Sin(timer / fontPeriod * Mathf.PI/2) );
        startWriting.fontSize = (int)Mathf.Lerp(fontMin, fontMax, timeExpression);

        timer += Time.deltaTime;
    }





    public void Start_Function()
    {
        if (menu.activeInHierarchy == false)
        {
            mass.SetActive(true);
            restartButton.SetActive(true);
            startWriting.gameObject.SetActive(false);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomScript : MonoBehaviour {

    //private float xEnd = -500, yEnd = 340;
    Vector3 position, scale;
    int bomb;
    private GameObject hookController;
    private GameObject boomObject;
    private bool isDirectionChange;
    private float preX;
    private float preY;


    // Use this for initialization
    void Start()
    {
        preX = position.x;
        preY = position.y;

        isDirectionChange = false;
        position = transform.localPosition;
        scale = transform.localScale;

        hookController = GameObject.Find("HookController");
        boomObject = GameObject.Find("BoomObject");

        //Debug.Log("Toa do luc dau: x=" + position.x + " y=" + position.y);
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseBoomNum();


    }
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "boomObject")
        {
            bomb = PlayerPrefs.GetInt("Bomb");
            bomb++;
            PlayerPrefs.SetInt("Bomb", bomb);
            GamePlayScript.instance.SetNumberBoom();
            Destroy(gameObject, 1f);
        }
    }
    private void IncreaseBoomNum() 
    {
        if (transform.localScale.x < 0.1f) 
        {
            bomb = PlayerPrefs.GetInt("Bomb");
            bomb++;
            PlayerPrefs.SetInt("Bomb", bomb);
            GamePlayScript.instance.SetNumberBoom();
            Destroy(gameObject, 0f);
        }
    }
}

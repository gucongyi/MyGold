using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScoreScript : MonoBehaviour {

    //private float xEnd = -500, yEnd = 340;
    Vector3 position, scale;
    public Text txtScore;
    public static int score;
    public GameObject endFly;

    private GameObject textScore;
    private Vector2 textScoreRectTrans;
    private GameObject buttonTouch;
    private Vector2 buttonTouchRectTrans;
    private float distance;

    // Use this for initialization
    void Start () {
        position = transform.localPosition;
        scale = transform.localScale;

        textScore = GameObject.Find("TextScore");
        textScoreRectTrans = textScore.GetComponent<RectTransform>().position;
        distance = Vector2.Distance(transform.GetComponent<RectTransform>().position, textScoreRectTrans);

        //Debug.Log("Toa do luc dau: x=" + position.x + " y=" + position.y);
    }

    // Update is called once per frame
    void Update () {
        txtScore.text = "$"+ score.ToString();
        TextMove();
        
        
	}
    void TextMove()
    {
        float x = position.x;
        float y = position.y;

        float xScale = scale.x;
        float yScale = scale.y;

        Vector2 textPosition = Vector2.MoveTowards(transform.GetComponent<RectTransform>().position, textScoreRectTrans, distance * Time.deltaTime);

        transform.GetComponent<RectTransform>().position = (Vector3)textPosition;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "scoreObject")
        {
            //Debug.Log("cham vao score object");
            GamePlayScript.instance.ScoreZoomEffect();
            GamePlayScript.instance.SetScoreText();
            Destroy(gameObject, 0.1f);
        }
    }

}

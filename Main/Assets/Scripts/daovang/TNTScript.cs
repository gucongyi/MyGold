using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTScript : MonoBehaviour {

    public GameObject fire;
    public float speed;
    private Vector3 vector3;
    //public GameObject onePiece;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Hook")
        {
            fire.SetActive(true);
            GamePlayScript.instance.PlaySound(4);
            LuoiCauScript.instance.cameraOut = true;
            GameObject.Find("HookController").GetComponent<DayCauScript>().typeAction = TypeAction.KeoCau;
            GameObject.Find("Hook").GetComponent<LuoiCauScript>().velocity = -GameObject.Find("Hook").GetComponent<LuoiCauScript>().velocity * 2;
            GameObject.Find("Hook").GetComponent<LuoiCauScript>().speed -= this.speed;


            //vector3 = gameObject.transform.position;
            //Debug.Log("x=" + vector3.x + " y=" + vector3.y);
            //Instantiate(onePiece, vector3, Quaternion.identity);

            vector3 = gameObject.transform.position;
            GameObject TNTFire = (GameObject)Instantiate(Resources.Load("TNTFire"), vector3, Quaternion.identity);
            TNTFire.transform.localScale = GameObject.Find("Map").transform.localScale;

            Destroy(gameObject);
            Destroy(fire, 0.5f);
        }
    }
       
}

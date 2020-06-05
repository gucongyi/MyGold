using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelController : MonoBehaviour {

    public Text textTarget, textLevel;
    float timeDelay;
	void Start () {

        textTarget.text = "$" + CGameManager.instance.GetScoreTarget(CGameManager.instance.levelCurrent);
        textLevel.text = "LEVEL " + CGameManager.instance.levelCurrent;
	}
	
	// Update is called once per frame
	void Update () {
        timeDelay += UnityEngine.Time.deltaTime * 10;
        if(timeDelay > 25)
        {
            int level = CGameManager.instance.levelCurrent;
            SceneManager.LoadScene("Level");
            //SceneManager.LoadScene("Level_virtual");
        }
	}
}

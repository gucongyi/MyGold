using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GamePlayScript : MonoBehaviour {
    public static GamePlayScript instance;
	public Text timeText, levelText, targetText, scoreText;
    public Text boomText;
    public Text scoreVictoryText, scoreFailText;
	public int score, scoreTarget;
	private int time;
    private float countDown;
    public GameObject panelMenu, panelVictory, panelFail, soundOnButton, soundMuteButton, musicOnButton, musicMuteButton;
    public Animator animPanelMenu, animPanelDark, animPanelPlay;
    public Button restartGame, restartFailPanel;
	//public GameObject []levelsVang;
	public int level;
	public bool endgame = false;

    public GameObject scoreTextFly, boomFly, boomObject, buttonNextLevel;
    public Transform canvas;


    public int numberBoom;
    public string itemSeclected;
    private GameObject itemDestroy, fireObject;

    private bool nextLevel = false;

    public AudioSource audioMusic;
    public AudioSource audioSound;
    public AudioClip pressButton, explosive, lowValue, normalValue, highValue, last10S, pull, lose, win;

    public Collider2D quayTraiC, quayPhaiC, hookC;
    public GameObject Test;
    public GameObject Bone, Mouse1, Mouse2, Mouse3, Mouse4, Mouse5, DGreen, DollarBag, DPink, DViolet, DYellow, GiftBag, GoldL, GoldM, GoldS, SkullCap, StoneL, StoneM, StoneS, TNT;
    public Sprite pBg1, pBg2, pBg3, pBg4, mapBg1, mapBg2, mapBg3, mapBg4;
    public AudioClip bgm1, bgm2;
    private Dictionary<Company.Cfg.Object, GameObject> mapObjDic;
    private Dictionary<string, Sprite> mapSpriteDic;
    private Dictionary<string, AudioClip> mapBGMDic;
    private Company.Cfg.LevelDefine curLevelC;

    bool victory, fail, isPause;
    // Use this for initialization
    void Start () {
        //读配置数据
        var levelC = CGameManager.configExcel.Level;
        //手动设置当前关卡，测试使用
        CGameManager.instance.levelCurrent = 31;
        curLevelC = levelC.Find((elem) => elem.ID == CGameManager.instance.levelCurrent);
        //整理地图字典
        GenerateMapObjDic();
        //更换当前等级基础数据
        UpdateBaseMapInfo();
        //生成地图物体
        GenerateMapObj();

        //忽略钩子和虚拟墙的碰撞
        Physics2D.IgnoreCollision(quayTraiC, hookC, true);
        Physics2D.IgnoreCollision(quayPhaiC, hookC, true);

        score = PlayerPrefs.GetInt("MaxDollar");
        scoreText.text = "$" + score;
        MakeInstance();

		level = 0;
		this.StartCoroutine("Do");
        
        levelText.text = "LEVEL " + CGameManager.instance.levelCurrent;
        //scoreTarget = CGameManager.instance.GetScoreTarget(CGameManager.instance.levelCurrent);
   
        SoundControl();
        MusicControl();
        SetButtonMusic();
        SetButtonSound();
        SetNumberBoom();
    }

    private void UpdateBaseMapInfo() 
    {
        int curTargetScore = curLevelC.TargetScore;
        int curTime = curLevelC.Time;
        string curBGM = curLevelC.BGM;
        string curPersonBg = curLevelC.PersonBg;
        string curMapBg = curLevelC.MapBg;

        countDown = curTime;
        targetText.text = "$" + curTargetScore.ToString();
        scoreTarget = curTargetScore;
        audioMusic.clip = mapBGMDic[curBGM];
        audioMusic.Play();
        GameObject.Find("Bg_top").GetComponent<Image>().sprite = mapSpriteDic[curPersonBg];
        GameObject.Find("Bg_bottom").GetComponent<Image>().sprite = mapSpriteDic[curMapBg];
    }

    private void GenerateMapObjDic() 
    {
        mapObjDic = new Dictionary<Company.Cfg.Object, GameObject>();
        //Bone, Mouse1, Mouse2, Mouse3, Mouse4, Mouse5, DGreen, DollarBag, DPink, DViolet, DYellow, GiftBag, GoldL, GoldM, GoldS, SkullCap, StoneL, StoneM, StoneS, TNT;
        mapObjDic.Add(Company.Cfg.Object.Bone, Bone);
        mapObjDic.Add(Company.Cfg.Object.Mouse1, Mouse1);
        mapObjDic.Add(Company.Cfg.Object.Mouse2, Mouse2);
        mapObjDic.Add(Company.Cfg.Object.Mouse3, Mouse3);
        mapObjDic.Add(Company.Cfg.Object.Mouse4, Mouse4);
        mapObjDic.Add(Company.Cfg.Object.Mouse5, Mouse5);
        mapObjDic.Add(Company.Cfg.Object.DGreen, DGreen);
        mapObjDic.Add(Company.Cfg.Object.DollarBag, DollarBag);
        mapObjDic.Add(Company.Cfg.Object.DPink, DPink);
        mapObjDic.Add(Company.Cfg.Object.DViolet, DViolet);
        mapObjDic.Add(Company.Cfg.Object.DYellow, DYellow);
        mapObjDic.Add(Company.Cfg.Object.GiftBag, GiftBag);
        mapObjDic.Add(Company.Cfg.Object.GoldL, GoldL);
        mapObjDic.Add(Company.Cfg.Object.GoldM, GoldM);
        mapObjDic.Add(Company.Cfg.Object.GoldS, GoldS);
        mapObjDic.Add(Company.Cfg.Object.SkullCap, SkullCap);
        mapObjDic.Add(Company.Cfg.Object.StoneL, StoneL);
        mapObjDic.Add(Company.Cfg.Object.StoneM, StoneM);
        mapObjDic.Add(Company.Cfg.Object.StoneS, StoneS);
        mapObjDic.Add(Company.Cfg.Object.TNT, TNT);
        //整理图片字典
        mapSpriteDic = new Dictionary<string,Sprite>();
        //pBg1, pBg2, pBg3, pBg4, mapBg1, mapBg2, mapBg3, mapBg4;
        mapSpriteDic.Add("pBg1", pBg1);
        mapSpriteDic.Add("pBg2", pBg2);
        mapSpriteDic.Add("pBg3", pBg3);
        mapSpriteDic.Add("pBg4", pBg4);
        mapSpriteDic.Add("mapBg1", mapBg1);
        mapSpriteDic.Add("mapBg2", mapBg2);
        mapSpriteDic.Add("mapBg3", mapBg3);
        mapSpriteDic.Add("mapBg4", mapBg4);
        //整理音乐字典
        mapBGMDic = new Dictionary<string, AudioClip>();
        mapBGMDic.Add("bgm1", bgm1);
        mapBGMDic.Add("bgm2", bgm2);
    }

    //根据配置生成当前关卡的地图对象
    private void GenerateMapObj() 
    {
        //CGameManager.instance.levelCurrent : 当前关卡
        //CGameManager.instance.configExcel
        for (int i = 0; i < curLevelC.ObjPos.Count; i++) 
        {
            //var mapObj = curLevelC.ObjPos[i].Obj;
            //坐标转换
            float posX = ((curLevelC.ObjPos[i].X - 500) / 65f) / 3.5f;
            float posY = (curLevelC.ObjPos[i].Y * 17 / 1000f - 17f) / 3.5f;
            if (GameObject.Find("MapVirtual") == null) 
            {
                Instantiate(mapObjDic[curLevelC.ObjPos[i].Obj], new Vector3(posX, posY, 0f), mapObjDic[curLevelC.ObjPos[i].Obj].transform.rotation, GameObject.Find("Map").transform);
            }
        }
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            PauseGame();
        }

        
        //if (score >= scoreTarget)
        //{
        //    buttonNextLevel.SetActive(true);
        //}
        countDown -= UnityEngine.Time.deltaTime;
        time = (int)countDown;
        if (time > 0)
        {
            timeText.text = time.ToString();
            if(time < 10)
            {
                if(victory || fail)
                {

                }
                else
                {
                    PlaySound(6);
                }
                
            }
        }
        else
        {
            if(score >= scoreTarget)
            {
                if (!victory)
                {
                    Victory();
                }
                
            }
            else
            {
                if (!fail)
                {
                    Fail();
                }

            }
        }
        
        
        //Debug.Log("Thoi gian con lai la: " + cd);
    }
    private void OnApplicationPause(bool pause)
    {

        //isPause = pause;
        //Debug.Log("Tam dung game: " + isPause);
        if (pause && !isPause)
        {
            if (victory && fail)
            {

            }
            else
            {
                PauseGame();
            }

        }
    }
    public void PlaySound(int i)
    {
        switch (i)
        {
            case 1:
                audioSound.PlayOneShot(lowValue);
                break;
            case 2:
                audioSound.PlayOneShot(normalValue);
                break;
            case 3:
                audioSound.PlayOneShot(highValue);
                break;
            case 4:
                audioSound.PlayOneShot(explosive);
                break;
            case 5:
                if (!audioSound.isPlaying)
                {
                    if (victory || fail)
                    {

                    }
                    else
                    {
                        audioSound.PlayOneShot(pull);
                    }
                    
                }   
                break;
            case 6:
                if (!audioSound.isPlaying)
                {
                    audioSound.PlayOneShot(last10S);
                }              
                break;
            case 7:
                audioSound.PlayOneShot(lose);
                break;
            case 8:
                audioSound.PlayOneShot(win);
                break;
            case 9:
                audioSound.PlayOneShot(pressButton);
                break;
        }
    }
    public void PauseGame()
    {
        isPause = true;
        audioSound.enabled = false;
        animPanelDark.SetBool("In", true);
        animPanelMenu.SetBool("In", true);
        animPanelMenu.SetBool("Out", false);
        Time.timeScale = 0;
        restartGame.onClick.RemoveAllListeners();
        restartGame.onClick.AddListener(() => RestartGame());
 
    }

    public void ResumeGame()
    {
        isPause = false;
        audioSound.enabled = true;
        PlaySound(9);

        UnityEngine.Time.timeScale = 1;
        animPanelDark.SetBool("In", false);
        animPanelMenu.SetBool("Out", true);
        //StartCoroutine(MenuPanelOnTop());
    }

    public void Victory()
    {
        audioMusic.enabled = false;
        PlaySound(8);
        victory = true;
        UnityEngine.Time.timeScale = 0;
        panelVictory.SetActive(true);
        scoreVictoryText.text = "$" + score;
        PlayerPrefs.SetInt("MaxLevel", CGameManager.instance.levelCurrent);
        PlayerPrefs.SetInt("MaxDollar", score);
    } 
    public void Fail()
    {
        audioMusic.enabled = false;
        PlaySound(7);
        fail = true;
        UnityEngine.Time.timeScale = 0;
        panelFail.SetActive(true);
        scoreFailText.text = "$" + score;

        restartFailPanel.onClick.RemoveAllListeners();
        restartFailPanel.onClick.AddListener(() => RestartGame());
    }
    public void RestartGame()
    {
        UnityEngine.Time.timeScale = 1;
        CGameManager.instance.powerCurrent = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void NextLevel()
    {
        CGameManager.instance.DisableItems();
        SceneManager.LoadScene("Shop"); 
        CGameManager.instance.levelCurrent++;
    }
    public void NextLevelAndSave()
    {
        PlayerPrefs.SetInt("MaxLevel", CGameManager.instance.levelCurrent);
        PlayerPrefs.SetInt("MaxDollar", score);
        CGameManager.instance.DisableItems();
        SceneManager.LoadScene("Shop");
        CGameManager.instance.levelCurrent++;
    }
    //IEnumerator ShowMenuPanel()
    //{
    //    yield return new WaitForSeconds(1f);
    //    panelMenu.SetActive(true);
    //}
    public void BackToMenu()
    {
        //SceneManager.LoadScene("MainMenu");
        SceneManager.LoadScene("MainMenu");
    }
    public void Boom()
    {
        OngGiaScript.instance.DropBomb();
        itemDestroy = GameObject.Find("Target").transform.GetChild(0).gameObject;
        Vector3 vector3 = itemDestroy.transform.position;
        Debug.LogError(vector3);

        PlaySound(4);
        //Debug.Log("Cai dang duoc keo la co tag la: " + itemDestroy.tag);
        if(itemDestroy.tag == "Gold")
        {
            fireObject = Instantiate(Resources.Load("GoldFire"), vector3, Quaternion.identity) as GameObject;
            fireObject.transform.localScale = GameObject.Find("Map").transform.localScale * itemDestroy.transform.localScale.x;
        }
        else if(itemDestroy.tag == "Stone")
        {
            fireObject = Instantiate(Resources.Load("StoneFire"), vector3, Quaternion.identity) as GameObject;
            fireObject.transform.localScale = GameObject.Find("Map").transform.localScale * itemDestroy.transform.localScale.x;
        }
        else
        {
            fireObject = Instantiate(Resources.Load("OtherFire"), vector3, Quaternion.identity) as GameObject;
            fireObject.transform.localScale = GameObject.Find("Map").transform.localScale;
        }
        
        Destroy(itemDestroy);
        numberBoom = PlayerPrefs.GetInt("Bomb");
        numberBoom--;
        PlayerPrefs.SetInt("Bomb", numberBoom);
        SetNumberBoom();
        if(GameObject.Find("HookController").GetComponent<DayCauScript>().typeAction == TypeAction.KeoCau)
        {
            LuoiCauScript.instance.speed = 4;
        }
        //GameObject.Find("HookController").GetComponent<DayCauScript>().typeAction = TypeAction.Nghi;
    }
    public void Power()
    {
        CGameManager.instance.powerCurrent = true;
        OngGiaScript.instance.Happy();
    }
    public void SetNumberBoom()
    {
        if (PlayerPrefs.GetInt("Bomb") > 0)
        {
            boomObject.SetActive(true);
            boomText.text = PlayerPrefs.GetInt("Bomb").ToString();
        }
        else
        {
            boomObject.SetActive(false);
        }
    }
	public IEnumerator Do ()
	{
		bool add = true;
		while(add){
			yield return new WaitForSeconds (1);
			if(time > 0) {
				time --;
			}
			if(time <= 0 && !endgame) {
                //endGame();
                //StopCoroutine("Do");
            }
		}
	}

    //void endGame() {
    //	endgame = true;
    //	menuEndGame.SetActive(true);
    //	level ++;
    //}

    //void startGame()
    //{
    //    //menuEndGame.SetActive(false);
    //    endgame = false;
    //    time = 60;
    //    score = 0;
    //    for (int i = 0; i < levelsVang.Length; i++)
    //    {
    //        if (level == i)
    //        {
    //            levelsVang[i].SetActive(true);
    //        }
    //        else
    //        {
    //            levelsVang[i].SetActive(false);
    //        }
    //    }
    //}
    public void CreateScoreFly(int score)
    {
        Vector3 vector3 = scoreTextFly.transform.position;
        Instantiate(scoreTextFly, vector3, Quaternion.identity).transform.SetParent(canvas, false);
        TextScoreScript.score = score;
    }

    public void ScoreZoomEffect()
    {
        animPanelPlay.SetBool("Zoom", true);
        StartCoroutine(ScoreZoomOut());
    }
    IEnumerator ScoreZoomOut()
    {
        yield return new WaitForSeconds(1f);
        animPanelPlay.SetBool("Zoom", false);
    }
    public void CreateBoomFly()
    {
        Vector3 vector3 = GameObject.Find("HookController").transform.position;
        Instantiate(boomFly, vector3, Quaternion.identity);
    }
	// Update is called once per frame
	
    public void SetScoreText()
    {
        scoreText.text = "$" + score.ToString();
    }
    //public void replay() {
    //	startGame();
    //}

    //Music and Sound Control
    void SoundControl()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            audioSound.enabled = true;
        }
        else
        {
            audioSound.enabled = false;
        }
    }
    void MusicControl()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            audioMusic.enabled = true;
        }
        else
        {
            audioMusic.enabled = false;
        }
    }
    public void SetOnMusic()
    {
        PlayerPrefs.SetInt("Music", 1);
        SetButtonMusic();
        MusicControl();
    }
    public void SetOnSound()
    {
        PlayerPrefs.SetInt("Sound", 1);
        SetButtonSound();
        SoundControl();
    }
    public void SetMuteMusic()
    {
        PlayerPrefs.SetInt("Music", 0);
        SetButtonMusic();
        MusicControl();
    }
    public void SetMuteSound()
    {
        PlayerPrefs.SetInt("Sound", 0);
        SetButtonSound();
        SoundControl();
    }
    private void SetButtonSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            soundOnButton.SetActive(true);
            soundMuteButton.SetActive(false);
        }
        else
        {
            soundOnButton.SetActive(false);
            soundMuteButton.SetActive(true);
        }
    }
    private void SetButtonMusic()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            musicOnButton.SetActive(true);
            musicMuteButton.SetActive(false);
        }
        else
        {
            musicMuteButton.SetActive(true);
            musicOnButton.SetActive(false);
        }
    }
}

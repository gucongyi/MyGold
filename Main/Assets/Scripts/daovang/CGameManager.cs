using UnityEngine;
using System.Collections;
using Company.Cfg;
using System;

public enum TypeAction { Nghi, ThaCau, KeoCau };
public enum EnumFishAction { Boi, CanCau, DopMoi, NhayVaoGio };
public enum EnumStateGame { Play, Pause, Win, Lose, Menu };

public delegate void OnStateChangeHandler();

public class CGameManager : MonoBehaviour
{
    public static CGameManager instance;
    public static Config configExcel;

    public int music = 1, sound = 1;
    private const string MUSIC = "Music", SOUND = "Sound";

    //public int[] scoreLevels;
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;
    public string keyLevelNow = "levelNow";
    public string keyLevelMax = "levelMax";
    public string keyNumberLevel = "numberLevel";

    //public int[] scoreTarget = new int[60];

    //	private static CGameManager _instance = null;    
    public event OnStateChangeHandler OnStateChange;
    public EnumStateGame gameState { get; private set; }
    public int score { get; private set; }
    public int level { get; private set; }
    public int maxScore { get; private set; }
    public int timePlay { get; private set; }
    public int typeLuoiCau { get; private set; }

    public int levelCurrent;
    public int scoreCurrent;

    public bool power, bookStone, clover, diamond, clock, powerCurrent;
    //public static CGameManager Instance { get; private set; }


    private void Awake()
    {
        IsGameStartedForTheFirstTime();
       
        MakeSingleInstance();
        music = GetMusic();
        sound = GetSound();

        //SetScoreTarget();
        int maxLevel = GetMaxLevel();
        if (maxLevel == 0)
        {
            levelCurrent = 1;
        }
        else
        {
            levelCurrent = maxLevel;
        }

        var _path = Application.dataPath + "/Bundles/TabtoyData/tabtoyData.bytes";
        //加载二进制表格数据
        StartCoroutine(wwwLoad(_path, ParseExcel));
    }
    private void ParseExcel(byte[] bytes) 
    {
        Debug.Log("xxxx2");
        //数据表格解析
        ParseExcelData parseExcelData = new ParseExcelData();
        configExcel = parseExcelData.Init(bytes);
        DisableItems();
    }
    void MakeSingleInstance()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    //	protected CGameManager() {}
    //
    //	// Singleton pattern implementation
    //	public static CGameManager Instance { 
    //		get {
    //			if (_instance == null) {
    //				_instance = new CGameManager(); 
    //			}  
    //			return _instance;
    //		} 
    //	}
    public int GetScoreTarget(int level)
    {
        var levelC = configExcel.Level;
        var curLevelC = levelC.Find((elem) => elem.ID == level);
        return curLevelC.TargetScore;
    }
    //void SetScoreTarget()
    //{
    //    scoreTarget[0] = 0;
    //    scoreTarget[1] = 800;
    //    scoreTarget[2] = 2050;
    //    scoreTarget[3] = 3795;
    //    scoreTarget[4] = 5500;
    //    scoreTarget[5] = 7050;
    //    scoreTarget[6] = 8575;
    //    scoreTarget[7] = 10195;
    //    scoreTarget[8] = 12270;
    //    scoreTarget[9] = 14095;
    //    scoreTarget[10] = 16995;
    //    scoreTarget[11] = 18695;
    //    scoreTarget[12] = 20404;
    //    scoreTarget[13] = 22107;
    //    scoreTarget[14] = 25657;
    //    scoreTarget[15] = 27447;
    //    scoreTarget[16] = 29447;
    //    scoreTarget[17] = 31647;
    //    scoreTarget[18] = 33750;
    //    scoreTarget[19] = 31600;
    //    scoreTarget[20] = 35800;
    //    scoreTarget[21] = 38100;
    //    scoreTarget[22] = 40000;
    //    scoreTarget[23] = 42300;
    //    scoreTarget[24] = 44700;
    //    scoreTarget[25] = 47200;
    //    scoreTarget[26] = 49600;
    //    scoreTarget[27] = 52200;
    //    scoreTarget[28] = 54900;
    //    scoreTarget[29] = 57700;
    //    scoreTarget[30] = 60200;
    //    scoreTarget[31] = 62800;
    //    scoreTarget[32] = 65700;
    //    scoreTarget[33] = 68500;
    //    scoreTarget[34] = 71500;
    //    scoreTarget[35] = 74510;
    //    scoreTarget[36] = 77460;
    //    scoreTarget[37] = 80430;
    //    scoreTarget[38] = 83430;
    //    scoreTarget[39] = 86630;
    //    scoreTarget[40] = 90330;
    //}
    void Update()
    {

    }
    public void DisableItems()
    {
        //power, bookStone, clover, diamond, clock;
        power = false;
        bookStone = false;
        clover = false;
        diamond = false;
        clock = false;
        powerCurrent = false;
    }
    public void SetGameState(EnumStateGame gameState)
    {
        this.gameState = gameState;
        if (OnStateChange != null)
        {
            OnStateChange();
        }
    }

    public void SetScore(int newScore)
    {
        this.score = newScore;
    }


    public void SetTimePlay(int newTime)
    {
        this.timePlay = newTime;
    }

    public void SetTypeLuoiCau(int type)
    {
        this.typeLuoiCau = type;
    }

    void IsGameStartedForTheFirstTime()
    {
        if (!PlayerPrefs.HasKey("IsGameStartedForTheFirstTime"))
        {
            PlayerPrefs.SetInt("Bomb", 0);
            PlayerPrefs.SetInt("MaxLevel", 0);
            PlayerPrefs.SetInt("MaxDollar", 0);
            PlayerPrefs.SetInt(MUSIC, 1);
            PlayerPrefs.SetInt(SOUND, 1);

            PlayerPrefs.SetInt("IsGameStartedForTheFirstTime", 0);

        }
    }
    // get set max level
    public void SetMaxLevel(int maxLevel)
    {
        PlayerPrefs.SetInt("MaxLevel", maxLevel);
    }
    public int GetMaxLevel()
    {
        return PlayerPrefs.GetInt("MaxLevel");
    }

    // get set max Score
    public void SetMaxScore(int maxScore)
    {
        PlayerPrefs.SetInt("MaxDollar", maxScore);
    }
    public int GetMaxScore()
    {
        return PlayerPrefs.GetInt("MaxDollar");
    }

    // get set music background
    public void SetMusic(int music)
    {
        PlayerPrefs.SetInt(MUSIC, music);
    }
    public int GetMusic()
    {
        return PlayerPrefs.GetInt(MUSIC);
    }
    // get set sound effect
    public void SetSound(int sound)
    {
        PlayerPrefs.SetInt(SOUND, sound);
    }
    public int GetSound()
    {
        return PlayerPrefs.GetInt(SOUND);
    }


    IEnumerator wwwLoad(string _path, Action<byte[]> action)
    {

//#if UNITY_EDITOR || UNITY_IOS
//        _path = "file://" + _path;
//#endif
        Debug.Log(_path);
        WWW www = new WWW(_path);

        yield return www;

        action(www.bytes);
    }
}

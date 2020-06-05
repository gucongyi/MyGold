using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVirtualTotal : MonoBehaviour
{
    private Company.Cfg.LevelDefine curLevelC;
    public GameObject Bone, Mouse1, Mouse2, Mouse3, Mouse4, Mouse5, DGreen, DollarBag, DPink, DViolet, DYellow, GiftBag, GoldL, GoldM, GoldS, SkullCap, StoneL, StoneM, StoneS, TNT;
    private Dictionary<Company.Cfg.Object, GameObject> mapObjDic;

    private float flagTime;
    // Start is called before the first frame update
    void Start()
    {
        flagTime = 0f;
        StartCoroutine(DelayGenerateMapObj());

        //SpliceChildOutStr();
        //CalTotalValue();
    }
    // Update is called once per frame
    void Update()
    {
        flagTime += Time.deltaTime;
        if ((int)flagTime % 4 == 3)
        {
            flagTime = 0f;
            SpliceChildOutStr();
            CalTotalValue();
        }
    }
    private IEnumerator DelayGenerateMapObj() 
    {
        yield return new WaitForSeconds(2f);
        if (true)
        {
            //读配置数据
            var levelC = CGameManager.configExcel.Level;
            //手动设置当前关卡，测试使用
            CGameManager.instance.levelCurrent = 80;
            curLevelC = levelC.Find((elem) => elem.ID == CGameManager.instance.levelCurrent);
            //整理地图字典
            GenerateMapObjDic();
            //生成地图物体
            GenerateMapObj();
        }
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
    }
    /// <summary>
    /// 计算地图总价值
    /// </summary>
    private void CalTotalValue() 
    {
        int totalValue = 0;
        foreach (Transform elem in transform)
        {
            var vangScript = elem.GetComponent<VangScript>();
            if (vangScript != null) 
            {
                totalValue += vangScript.score;
            }
            var chuotScript = elem.GetComponent<ChuotScript>();
            if (chuotScript != null)
            {
                totalValue += chuotScript.score;
            }
        }
        Debug.Log("当前地图总价值：" + totalValue.ToString());
    }
    /// <summary>
    /// 拼接地图对象位置信息字符串
    /// </summary>
    private void SpliceChildOutStr() 
    {
        string totalOutStr = "";
        int i = 0;
        foreach (Transform elem in transform) 
        {
            var mapVirtual = elem.GetComponent<MapVirtual>();
            string elemStr = mapVirtual.outPosInfo;
            if (i > 0)
            {
                totalOutStr += "|" + elemStr;
            }
            else 
            {
                totalOutStr += elemStr;
            }
            i++;
        }
        Debug.Log(totalOutStr);
    }
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
            if (GameObject.Find("MapVirtual") != null)
            {
                Instantiate(mapObjDic[curLevelC.ObjPos[i].Obj], new Vector3(posX, posY, 0f), mapObjDic[curLevelC.ObjPos[i].Obj].transform.rotation, GameObject.Find("MapVirtual").transform);
            }
        }
        CalTotalValue();
    }
}

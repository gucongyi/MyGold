using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVirtual : MonoBehaviour
{
    public string mapObjName;
    public string outPosInfo;
    void Start()
    {
        if (transform.parent.name != "MapVirtual") {
            Destroy(transform.GetComponent<MapVirtual>());
        }
        CalMapPos();
    }
    void Update()
    {
        
    }
    private void CalMapPos() 
    {
        float localPosX = transform.localPosition.x;
        float localPosY = transform.localPosition.y;
        //不需要乘以map的缩放倍数
        float outPosX = localPosX * 65 + 500;
        float outPosY = (localPosY + 17) * 1000 / 17f;
        if (outPosX < 0)
        {
            outPosX = 0f;
        }
        if (outPosY > 1000f)
        {
            outPosY = 1000f;
        }
        outPosInfo = "对象:" + mapObjName + " X:" + outPosX.ToString("0.0") + " Y:" + outPosY.ToString("0.0");
    }
}

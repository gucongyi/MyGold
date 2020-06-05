﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerScript : MonoBehaviour {

    public GameObject buttonBuy, buttonNBuy;
    public Text textPrice;
    int price;
    // Use this for initialization
    void Start()
    {
        SetPrice();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("So lan nhan: " + press);
    }

    void SetPrice()
    {
        price = Random.Range(10, 70);
        textPrice.text = "$" + price;
    }
    public void Buy()
    {
        int dollar = PlayerPrefs.GetInt("MaxDollar");
        PlayerPrefs.SetInt("MaxDollar", dollar - price);
        ShopController.instance.SetTextMoney();
        //Debug.Log("Da vao day");
            CGameManager.instance.power = true;
            //Destroy(gameObject);
        buttonNBuy.SetActive(true);
        buttonBuy.SetActive(false);
    }

}
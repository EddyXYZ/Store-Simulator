using System;
using UnityEngine;

[System.Serializable] public class StockInfo
{
    [SerializeField] private string name;
    [SerializeField] private EStockTypes stockType;
    [SerializeField] private float price;
    [SerializeField] private StockObject stockObject;

    public string GetName()
    {
        return name;
    }

    public EStockTypes GetStockType()
    {
        return stockType;
    }

    public float GetPrice()
    {
        return price;
    }
}

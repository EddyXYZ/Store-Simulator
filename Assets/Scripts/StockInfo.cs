using System;
using UnityEngine;

[System.Serializable] public class StockInfo
{
    public enum StockTypes
    {
        bigDrink, fruit, largeFruit, chips
    }
    [SerializeField] private string name;
    [SerializeField] private StockTypes stockType;
    [SerializeField] private float price;
    [SerializeField] private StockObject stockObject;

    public string GetName()
    {
        return name;
    }

    public StockTypes GetStockType()
    {
        return stockType;
    }

    public float GetPrice()
    {
        return price;
    }
}

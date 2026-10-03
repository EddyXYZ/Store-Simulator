using System.Collections.Generic;
using UnityEngine;

public class StockInfoController : MonoBehaviour
{
    [SerializeField] private List<StockInfo> foodInfo, produceInfo;
    public static StockInfoController instance;
    private List<StockInfo> allStock = new();

    // Consolidates stock-object information changes by having each StockObject.Start() replaces info with the controller's version
    public StockInfo GetInfo(string stockName)
    {
        StockInfo infoToReturn = null;

        for(int i = 0; i < allStock.Count; i++)
        {
            if(allStock[i].GetName() == stockName)
            {
                infoToReturn = allStock[i];
            }
        }

        return infoToReturn;
    }

    public StockInfo.StockTypes GetStockType(string stockName) => GetInfo(stockName).GetStockType(); // What does => do?

    public float GetPrice(string stockName) => GetInfo(stockName).GetPrice();

    // Unity calls Awake() once when the object is created, before any object's Start() runs
    private void Awake()
    {   
        instance = this; // Helps Unity find "which" StockInfoController is stored in memory 
        allStock.AddRange(foodInfo);
        allStock.AddRange(produceInfo);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

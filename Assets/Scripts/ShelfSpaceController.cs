using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShelfSpaceController : MonoBehaviour
{
    [SerializeField] private List<ShelfSection> shelfSections;
    private Dictionary<EStockTypes, ShelfSection> shelfDict = new(); 
    
    private void Awake()
    {
        foreach(ShelfSection section in shelfSections)
        {
            shelfDict[section.GetStockType()] = section;
        }
    }

    public void PlaceStock(StockObject stockObjectToPlace)
    {
        /* 
            * PlaceStock() uses StockInfoController to get stockObjectToPlace's EStockTypes

            * Using stockObjectToPlace's EStockTypes, PlaceStock() can use shelfDict to access 
                the specific shelf section stockObjectToPlace will be placed in
        */

        EStockTypes objectType = StockInfoController.instance.GetStockType(stockObjectToPlace.GetStockName());;
        List<Transform> allowedAmount = shelfDict[objectType].GetPlacementPoints();
        List<StockObject> currentAmount = shelfDict[objectType].GetAmountInShelf();
        TMP_Text label = shelfDict[objectType].GetLabel();

        // Check if there is no more room left
        if(currentAmount.Count >= allowedAmount.Count)
        {
            return;
        }

        // Set the stock object's position
        stockObjectToPlace.GetStockRB().isKinematic = true;
        stockObjectToPlace.SetPlaced(true);
        stockObjectToPlace.GetCollider().enabled = false;
        stockObjectToPlace.transform.SetParent(allowedAmount[currentAmount.Count]); 

        // Add the stock object to its respective List<StockObject>
        currentAmount.Add(stockObjectToPlace);
        label.text = "$" + StockInfoController.instance.GetInfo(currentAmount[0].GetStockName()).GetPrice();
    }

    public StockObject GetStock(Collider hitCollider)
    {
        /*
            1. Get the specific section, that the latest stock object was add to, using its designated BoxCollider
            2. Remove the latest stock object that was added in its section
            3. Label should reflect if there are no more stock objects in a section
        */

        List<StockObject> currentAmount = new(); // Get the current amount in the specific section using its BoxCollider
        TMP_Text stockObjectLabel = null; 

        foreach(ShelfSection section in shelfSections)
        {
            if(section.GetBoxCollider() == hitCollider)
            {
                currentAmount = section.GetAmountInShelf();
                stockObjectLabel = section.GetLabel();
                break;
            }
        }

        if(currentAmount.Count == 0 || currentAmount == null) // Check if there isn't anything in that shelf or if the collider returns something else (default = null)
        {
            return null; // Do nothing if the stock is empty
        }

        int latestStockObjectIndex = currentAmount.Count - 1; // Get the index of the latest stock object
        StockObject latest = currentAmount[latestStockObjectIndex];   // Get the latest stock object that was added to its designated BoxCollider
        currentAmount.RemoveAt(latestStockObjectIndex);               // Remove that latest stock object using its index

        // Clear the label once the last stock object has been taken out
        if(currentAmount.Count == 0)
        {
            stockObjectLabel.text = "Empty";
        }

        return latest;
    }
}

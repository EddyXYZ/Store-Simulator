using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShelfSpaceController : MonoBehaviour
{
    [SerializeField] BoxCollider chipsCollider;
    [SerializeField] BoxCollider bigDrinksCollider;
    [SerializeField] BoxCollider fruitsCollider;
    [SerializeField] BoxCollider largeFruitsCollider;
    [SerializeField] TMP_Text chipsLabel;
    [SerializeField] TMP_Text bigDrinksLabel;
    [SerializeField] TMP_Text fruitsLabel;
    [SerializeField] TMP_Text largeFruitsLabel;
    [SerializeField] List<StockObject> bigDrinksInShelf;
    [SerializeField] List<StockObject> largeFruitsInShelf;
    [SerializeField] List<StockObject> fruitsInShelf;
    [SerializeField] List<StockObject> chipsInShelf;
    [SerializeField] List<Transform> chipPoints;
    [SerializeField] List<Transform> bigDrinkPoints;
    [SerializeField] List<Transform> fruitsPoints;
    [SerializeField] List<Transform> largeFruitsPoints;
    
    // Get the specific List<Transform> type depending on the stock object's type
    private List<Transform> GetPointsFor(StockInfo.StockTypes type)
    {
        switch(type)
        {
            case StockInfo.StockTypes.chips:      return chipPoints;
            case StockInfo.StockTypes.bigDrink:   return bigDrinkPoints;
            case StockInfo.StockTypes.fruit:      return fruitsPoints;
            case StockInfo.StockTypes.largeFruit: return largeFruitsPoints;
            default:                              return null;
        }
    }

    // Get the specific List<StockObject> type depending on the stock object's type
    private List<StockObject> GetListFor(StockObject stockObject)
    {
        switch(StockInfoController.instance.GetStockType(stockObject.GetStockName()))
        {
            case StockInfo.StockTypes.chips:
                return chipsInShelf;
            case StockInfo.StockTypes.bigDrink:
                return bigDrinksInShelf;
            case StockInfo.StockTypes.fruit:
                return fruitsInShelf;
            case StockInfo.StockTypes.largeFruit:
                return largeFruitsInShelf;
            default: return null;
        }
    }

    // Get the specific List<StockObject> type depending on the stock object's collider
    private List<StockObject> GetListFor(Collider c)
    {
        if(c == bigDrinksCollider)   return bigDrinksInShelf;
        if(c == largeFruitsCollider) return largeFruitsInShelf;
        if(c == fruitsCollider)      return fruitsInShelf;
        if(c == chipsCollider)       return chipsInShelf;
        return null; // Hit something else other than the 4 colliders
    }

    // Get the specific TMP_Text type depending on the stock object's collider
    private TMP_Text GetLabelFor(Collider c)
    {
        if(c == bigDrinksCollider)   return bigDrinksLabel;
        if(c == largeFruitsCollider) return largeFruitsLabel;
        if(c == fruitsCollider)      return fruitsLabel;
        if(c == chipsCollider)       return chipsLabel;
        return null;
    }
    public void PlaceStock(StockObject objectToPlace)
    {
        StockInfo.StockTypes objectType = StockInfoController.instance.GetStockType(objectToPlace.GetStockName());
        List<Transform> allowedAmount = GetPointsFor(objectType);
        List<StockObject> currentAmount = GetListFor(objectToPlace);

        // Check if there is no more room left
        if(currentAmount.Count >= allowedAmount.Count)
        {
            return;
        }

        // Set the stock object's position
        objectToPlace.GetStockRB().isKinematic = true;
        objectToPlace.SetPlaced(true);
        objectToPlace.GetCollider().enabled = false;
        objectToPlace.transform.SetParent(allowedAmount[currentAmount.Count]); 

        // Add the stock object to its respective List<StockObject>
        switch(StockInfoController.instance.GetStockType(objectToPlace.GetStockName()))
        {
            case StockInfo.StockTypes.chips: 
                chipsInShelf.Add(objectToPlace);
                chipsLabel.text = "$" + StockInfoController.instance.GetInfo(chipsInShelf[0].GetStockName()).GetPrice(); 
                break;
            case StockInfo.StockTypes.bigDrink: 
                bigDrinksInShelf.Add(objectToPlace);
                bigDrinksLabel.text = "$" + StockInfoController.instance.GetInfo(bigDrinksInShelf[0].GetStockName()).GetPrice(); 
                break;
            case StockInfo.StockTypes.fruit: 
                fruitsInShelf.Add(objectToPlace);
                fruitsLabel.text = "$" + StockInfoController.instance.GetInfo(fruitsInShelf[0].GetStockName()).GetPrice(); 
                break;
            case StockInfo.StockTypes.largeFruit: 
                largeFruitsInShelf.Add(objectToPlace);
                largeFruitsLabel.text = "$" + StockInfoController.instance.GetInfo(largeFruitsInShelf[0].GetStockName()).GetPrice(); 
                break;
        }
    }

    public StockObject GetStock(Collider hitCollider)
    {
        /*
            1. Get the specific shelf, that the latest stock object was add to, using its designated BoxCollider
            2. Remove that latest stock object 
        */

        List<StockObject> currentAmount = GetListFor(hitCollider); // Get the current amount in the specific shelf using its BoxCollider

        if(currentAmount.Count == 0 || currentAmount == null) // Check if there isn't anything in that shelf or if the collider returns something else (default = null)
        {
            return null; // Do nothing if the stock is empty
        }

        int latestStockObjectIndex = currentAmount.Count - 1; // Get the index of the latest stock object
        StockObject latest = currentAmount[latestStockObjectIndex];   // Get the latest stock object that was added to its designated BoxCollider
        currentAmount.RemoveAt(latestStockObjectIndex);               // Remove that latest stock object using its index

        // Clear the label once the last stock object has been taken out
        TMP_Text stockObjectLabel = GetLabelFor(hitCollider);

        if(currentAmount.Count == 0)
        {
            stockObjectLabel.text = "Empty";
        }

        return latest;
    }
}

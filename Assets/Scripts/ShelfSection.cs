using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable] public class ShelfSection
{
    [SerializeField] private EStockTypes type;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private TMP_Text label;
    [SerializeField] private List<StockObject> amountInShelf;
    [SerializeField] private List<Transform> placementPoints;

    public EStockTypes GetStockType() => type;
    public TMP_Text GetLabel() => label;
    public BoxCollider GetBoxCollider() => boxCollider;
    public List<StockObject> GetAmountInShelf() => amountInShelf;
    public List<Transform> GetPlacementPoints() => placementPoints;
}

using System;
using NUnit.Framework;
using UnityEngine;

public class StockObject : MonoBehaviour
{
    [SerializeField] private string stockName;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody stockRB;
    [SerializeField] private Collider collider;
    private bool isPlaced;

    public string GetStockName()
    {
        return stockName;
    }
    public Rigidbody GetStockRB()
    {
        return stockRB;
    }

    public bool GetPlaced()
    {
        return isPlaced;
    }

    public void SetPlaced(bool value)
    {
        isPlaced = value;
    }

    public Collider GetCollider()
    {
        return collider;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlaced)
        {
            // Move in world space so the placement point's scale doesn't change the speed
            transform.position = Vector3.MoveTowards(transform.position, transform.parent.position, moveSpeed * Time.deltaTime);
            // Slerp makes the initial movement fast and then slow down
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, moveSpeed * Time.deltaTime);
        }
    }

    public void Pickup()
    {
        stockRB.isKinematic = true;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        isPlaced = false;
        collider.enabled = false;
    }

    public void Release()
    {
        stockRB.isKinematic = false;
        collider.enabled = true;
    }
}

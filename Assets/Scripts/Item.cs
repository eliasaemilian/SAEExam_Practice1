using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Item")]
public class Item : ScriptableObject
{
    public string ItemName;
    public int Weight;
    public int Value;
    public string Description;
    public Category Category;
}

public enum Category
{
    Cat1,
    Cat2
}
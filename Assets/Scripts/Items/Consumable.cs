using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public enum ItemType
    {
        HealtPotion,

        Fireball,

        ScrollOfConfusion
    }
    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Get.AddItem(this);
    }

    private ItemType type;

    public ItemType Type
    {
        get { return type; }
    }
}

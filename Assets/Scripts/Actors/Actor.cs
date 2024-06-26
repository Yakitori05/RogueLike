using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Mathematics;
using System;

public class Actor : MonoBehaviour
{
    private AdamMilVisibility algorithm;
    public List<Vector3Int> FieldOfView = new List<Vector3Int>();
    public int FieldOfViewRange = 8;

    [Header("Powers")]
    [SerializeField] private int maxHP = 30;
    [SerializeField] private int hp = 30;
    [SerializeField] private int defense;
    [SerializeField] private int power;
    [SerializeField] private int level = 1;
    [SerializeField] private int xp;
    [SerializeField] private int xptonextlevel = 50;


    public int MaxHP => maxHP;
    public int HP => hp;
    public int Defense => defense;
     public int Power => power;
    public int Level => level;
    public int Xp => xp;
    public int Xptonextlevel => xptonextlevel;

    private void Start()
    {
        algorithm = new AdamMilVisibility();
        UpdateFieldOfView();

        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hp, maxHP);
            UIManager.Instance.UpdateLevel(level);
            UIManager.Instance.UpdateXp(xp);
        }
    }

    private void Die()
    {
        if (GetComponent<Player>())
        {
            UIManager.Instance.AddMessages("You died", Color.red);
        }
        else
        {
            UIManager.Instance.AddMessages($"{name}is dead", Color.red);
            GameManager.Get.RemoveEnemy(this);
        }

        //*GameObject gravestone = 
            GameManager.Get.CreateActor("Dead", transform.position);
        //*gravestone.name = $"Remains of {name}";

        Destroy(gameObject);
    }

    public void DoDamage(int hp, Actor enemy)
    {
        hp -= hp;

        if (hp < 0)
        {
            hp = 0;
        }

        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hp, maxHP);
        }

        if (hp == 0)
        {
            Die();
            enemy.Addxp(xp);
        }
    }

    public void Move(Vector3 direction)
    {
        if (MapManager.Get.IsWalkable(transform.position + direction))
        {
            transform.position += direction;
        }
    }

    public void UpdateFieldOfView()
    {
        var pos = MapManager.Get.FloorMap.WorldToCell(transform.position);

        FieldOfView.Clear();
        algorithm.Compute(pos, FieldOfViewRange, FieldOfView);

        if (GetComponent<Player>())
        {
            MapManager.Get.UpdateFogMap(FieldOfView);
        }
    }

    public void Health (int hp)
    {
        int previoushp = hp;
        hp += hp;
        
        if (hp > maxHP)
        {
            hp = maxHP;
        }

        int amounthealed = hp - previoushp;

        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hp, maxHP);
            UIManager.Instance.AddMessages($"You have been healed by {amounthealed} hp", Color.green);
        }
    }

    public void Addxp(int xp)
    {
        this.xp += xp;

        while (this.xp >= xptonextlevel)
        {
            this.xp -= xptonextlevel;
            Levelup();
        }

        if (GetComponent<Player>())
        {
            UIManager.Get.UpdateXp(this.xp);
        }
    }

    public void Levelup()
    {
        level++;
        xptonextlevel = (int)Math.Round((double)xptonextlevel, MidpointRounding.ToEven);
        maxHP += 15;
        defense += 2;
        power += 2;
    }
}

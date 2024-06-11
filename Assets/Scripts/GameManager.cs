using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Get { get => instance; }

    private List<Actor> enemies = new List<Actor>();
    public List<Consumable> Items { get; private set; } = new List<Consumable>();
    public List<Actor> Enemies { get => enemies; }
    public List<Ladder> Ladders = new List<Ladder>();
    public Actor player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddEnemy (Actor enemy)
    {
        enemies.Add(enemy);
    }
    public void SetPlayer(Actor player)
    {
        this.player = player;
    }
    public Actor GetPlayer() 
    { 
        return player; 
    }
    public Actor GetActorAtLocation(Vector3 location)
    {
        if (player != null && player.transform.position == location)
        {
            return player;
        }
        foreach (Actor enemy in enemies) 
        {
            if (enemy.transform.position == location)
            {
                return enemy;
            }
        }
        return null;
    }
    public Consumable GetItemAtLocation (Vector3 location)
    {
        foreach (var item in Items)
        {
            if (item != null && item.transform.position == location) 
            { 
                return item; 
            }
        }
        return null;
    }

    public Ladder GetLadderAtLocation (Vector3 location)
    {
        foreach (var ladder in Ladders)
        {
            if (ladder.transform.position == location)
            {
                return ladder;
            }
        }
        return null;
    }
    public Actor CreateActor(string type, Vector2 position)
    {
        GameObject actorPrefab = Resources.Load<GameObject>($"Prefabs/{type}");
        if (actorPrefab != null )
        {
            GameObject actorObject = Instantiate(actorPrefab , position, Quaternion.identity);
            Actor actorComponent = actorObject.GetComponent<Actor>();
            if (actorComponent != null)
            {
                if (type == "Player")
                {
                    SetPlayer(actorComponent);
                }
                else
                {
                    AddEnemy(actorComponent);
                }
                return actorComponent;
            }
        }
        Debug.LogError($"Actor type '{type}' could not be created");
        return null;
    }
    public void StartEnemyTurn()
    {
        foreach (Actor enemyActor in enemies) 
        {
            Enemy enemyComponent = enemyActor.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.RunAI();
            }
            else
            {
                Debug.LogError("Enemy component not found on actor");
            }
        }
    }
    public void RemoveEnemy (Actor enemy)
    {
        if (enemies.Contains(enemy))
        {
            Enemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }
    }
    public GameObject CreateItem (string name, Vector2 position)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>($"prefabs/{name}"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
        AddItem(item.GetComponent<Consumable>());
        item.name = name;
        return item;
    }
    public void AddItem (Consumable item)
    {
        Items.Add(item);
    }

    public void AddLadder(Ladder ladder)
    {
        Ladders.Add(ladder);
    }
    public void RemoveItem(Consumable item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
            Destroy(item.gameObject);
        }
    }
    public List <Actor> GetNearbyEnemies (Vector3 location)
    {
        List<Actor> nearbyEnemies = new List<Actor>();

        foreach (Actor enemy in Enemies)
        {
            if (Vector3.Distance(enemy.transform.position, location) < 5)
            {
                nearbyEnemies.Add(enemy);
            }
        }
        return nearbyEnemies;
    }

    public void ClearLevel()
    {
        foreach(var enemy in Enemies)
        {
            Destroy(enemy.gameObject);
        }
        Enemies.Clear();

        foreach(var item in Items)
        {
            Destroy(item.gameObject);
        }
        Items.Clear();

        foreach (var ladder in Ladders)
        {
            Destroy(ladder.gameObject);
        }
        Ladders.Clear();

        //*foreach (var tombstone in Tombstones)
        //*{
        //*    Destroy(tombstone.gameObject);
        //*}
        //*Tombstones.Clear();
    }
}
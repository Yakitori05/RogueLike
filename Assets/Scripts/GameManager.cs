using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Get { get => instance; }

    private List<Actor> enemies = new List<Actor>();
    public List<Actor> Enemies { get => enemies; }
    private Actor player;

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
}
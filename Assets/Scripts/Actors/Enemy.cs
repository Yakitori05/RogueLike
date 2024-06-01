using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor), typeof(AStar))]

public class Enemy : MonoBehaviour
{
    public Actor target {  get; set; }
    public bool IsFighting { get; private set; } = false;
    private AStar Algorithm;
    private int confused = 0;
    private void Start()
    {
        Actor actor = GetComponent<Actor>();
        GameManager.Get.AddEnemy(actor);
        Algorithm = GetComponent<AStar>();
    }
    public void MoveOnPath (Vector3Int targetPosition)
    {
        Vector3Int gridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        Vector2 direction = Algorithm.Compute((Vector2Int)gridPosition, (Vector2Int)targetPosition);
        Action.Move(GetComponent<Actor>(), direction);
    }
    public void RunAI()
    {
        if (target == null || target.Equals(null))
        {
            target = GameManager.Get.GetPlayer();
        }

        if(target == null)
        {
            return;
        }

        if (confused > 0)
        {
            confused--;
            UIManager.Instance.AddMessages($"{name} is canfuesd and can't move", Color.cyan);
            return;
        }

        Vector3Int targetGridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        if (IsFighting || GetComponent<Actor>().FieldOfView.Contains(targetGridPosition))
        {
            if (!IsFighting)
            {
                IsFighting = true;
            }

            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget < 1.5f)
            {
                Action.Hit(GetComponent<Actor>(), target);
            }
            else
            {
                MoveOnPath(targetGridPosition);
            }
        }
    }

    public void Confuse()
    {
        confused = 8;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor), typeof(AStar))]

public class Enemy : MonoBehaviour
{
    public Actor target {  get; set; }
    public bool IsFighting { get; private set; } = false;
    private AStar Algorithm;
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
        if (target == null)
        {
            target = GameManager.Get.GetPlayer();
        }

        Vector3Int targetGridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        Vector3Int gridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        if (IsFighting || GetComponent<Actor>().FieldOfView.Contains(gridPosition))
        {
            if (!IsFighting)
            {
                IsFighting = true;
            }
            MoveOnPath(targetGridPosition);
        }
    }
}

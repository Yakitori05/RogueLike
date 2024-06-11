using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    static public void MoveOrHit (Actor actor, Vector2 direction)
    {
        Actor target = GameManager.Get.GetActorAtLocation(actor.transform.position + (Vector3)direction);

        if (actor == null)
        {
            Move(actor, direction);
        }
        else
        {
            Hit(actor, target);
        }
    }
    static public void Move(Actor actor, Vector2 direction)
    {
        if (MapManager.Get.IsWalkable(actor.transform.position + (Vector3)direction))
        {
            actor.Move(direction);
            actor.UpdateFieldOfView();
        }
    }
    static public void Hit (Actor actor, Actor target)
    {
        int damage = actor.Power - target.Defense;

        if (damage > 0)
        {
            target.DoDamage(damage, actor);
            UIManager.Instance.AddMessages($"{actor.name} hits {target.name} for {damage} damage", actor.GetComponent<Player>() ? Color.white : Color.red);
        }
        else
        {
            UIManager.Instance.AddMessages($"{actor.name} hits {target.name} and does no damage", actor.GetComponent<Player>() ? Color.white : Color.red);
        }
    }
    static private void EndTurn (Actor actor)
    {
        if (actor.GetComponent<Player>() != null)
        {
            GameManager.Get.StartEnemyTurn();
        }
    }
}

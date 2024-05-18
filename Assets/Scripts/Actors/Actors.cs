using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actors : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static private void EndTurn(Actor actor)
    {
        if (actor.GetComponent<Player>() != null)
        {
            GameManager.Get.StartEnemyTurn();
        }
    }
    static public void Move (Actor actor, Vector2 direction)
    {
        Actor target = GameManager.Get.GetActorAtLocation(actor.transform.position + (Vector3)direction);
        if (target != null)
        {
            actor.Move(direction);
            actor.UpdateFieldOfView();
        }
        EndTurn(actor);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public bool up;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Get.AddLadder(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

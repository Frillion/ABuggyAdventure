using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyUpdate : MonoBehaviour
{
    IChase Chaser;

    private void Start()
    {
        Chaser = gameObject.GetComponent<IChase>();
    }

    // Update is called once per frame
    void Update()
    {
        Chaser.StartPath();
    }
}

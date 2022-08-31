using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public EnemyManager[] enemyManager;
    [SerializeField] Transform[] spawnLocation;
    [SerializeField] MyDoorController door;
    

    public void Play()
    {
        
        SpawnEnemy();
        LockDoor();

    }
    private void SpawnEnemy()
    {
             
            for(int i = 0; i < enemyManager.Length; i++)
        {
            enemyManager[i].enabled = true;
            enemyManager[i].navMeshAgent.enabled = true;
            enemyManager[i].navMeshAgent.Warp(spawnLocation[i].position);
        } 

    }
    private void LockDoor()
    {
        if (door.doorOpen)
        {
            door.PlayAnimation();
            door.isLocked = true;
        }
        else door.isLocked = true;
        
    }
       
}

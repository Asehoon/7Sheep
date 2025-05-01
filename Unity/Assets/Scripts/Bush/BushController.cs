using System.Collections.Generic;
using UnityEngine;

public class BushController : MonoBehaviour
{
    public static BushController Instance { get; private set; }

    public bool IsPlayerInBush { get; private set; } = false;
    public bool IsEnemyInBush => enemiesInBush.Count > 0;

    private List<BushTriggerHandler> enemiesInBush = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void SetPlayerInBush(bool isInBush)
    {
        IsPlayerInBush = isInBush;

        foreach (var enemy in enemiesInBush) //만약에 플레이어가 들어와있는데 enemy가 있다면
        {
            if (isInBush) //전부 visible
                enemy.SetLayer("Visible");
            else //만약에 플레이가 나간상태라면 다시 invisible로 바꾸기.
                enemy.SetLayer("Invisible");
        }
    }

    public void RegisterEnemyInBush(BushTriggerHandler enemy)
    {
        if (!enemiesInBush.Contains(enemy)) //포함되어있지않다면
        {
            enemiesInBush.Add(enemy); //추가하기
            if (!IsPlayerInBush) 
                enemy.SetLayer("Invisible");
        }
    }

    public void UnregisterEnemyFromBush(BushTriggerHandler enemy)
    {
        //enemy가 밖으로 나갈때
        if (enemiesInBush.Contains(enemy))
        {
            enemiesInBush.Remove(enemy);
            enemy.SetLayer("Visible");
        }
    }
}

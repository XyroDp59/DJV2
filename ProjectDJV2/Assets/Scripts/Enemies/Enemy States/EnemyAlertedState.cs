using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlertedState : MonoBehaviour, IEnemyState
{
    [SerializeField] float Speed;
    [SerializeField] private float alertedDuration;
    private float alertedTimer;

    public void OnInitialize(Enemy enemy)
    {
        ResetAlertedTimer();
        enemy.SetSpeed(Speed);
    }

    public void ResetAlertedTimer()
    {
        Debug.Log("IS ALERTED");
        alertedTimer = alertedDuration;
    }

    public void Behave(Enemy enemy)
    {
        enemy.SetDestination(enemy.lastKnownPlayerPos);

        alertedTimer -= Time.deltaTime;
        if(alertedTimer < 0)
        {
            enemy.GoToDefaultState();
            Debug.Log("BACK TO DEFAULT");
        }
    }

    public void OnDestinationFoundAction(Enemy enemy)
    {
        Debug.Log("Alerted : arrived to destination (did i kill you ?)");
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlertedState : MonoBehaviour, IEnemyState
{
    [SerializeField] float Speed;
    [SerializeField] private float alertedDuration;
    Enemy e;
    private float alertedTimer;

    public void FirstInitialize(EnemyData data)
    {
        Speed = data.alertedSpeed;
        alertedDuration = data.alertedDuration;
    }

    public void OnInitialize(Enemy enemy)
    {
        ResetAlertedTimer();
        enemy.SetSpeed(Speed);
        e = enemy;
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerController p) && alertedTimer > 0)
        {
            alertedTimer = 0;
            e.GoToDefaultState();
        }
    }
}

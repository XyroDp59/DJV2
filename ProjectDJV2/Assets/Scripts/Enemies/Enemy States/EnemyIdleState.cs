using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : MonoBehaviour, IEnemyState
{
    float precision = 0.1f;
    private float Speed = 1f;
    Vector3 initialPosition;
    Vector3 lookAtPosition;

    private void Start()
    {
        initialPosition = transform.position;
        lookAtPosition = initialPosition + transform.forward;
    }

    public void OnInitialize(Enemy enemy)
    {
        if (enemy.debugger) Debug.Log("IS IDLE");
        enemy.SetSpeed(Speed);
    }

    public void Behave(Enemy enemy)
    {
        if(Vector3.Distance(initialPosition, transform.position) > precision)
        {
            enemy.SetDestination(initialPosition);
        }
        else 
        {
            enemy.SetDestination(lookAtPosition);
            enemy.SetSpeed(0f);
        }
    }

    public void OnDestinationFoundAction(Enemy enemy)
    {
        if (enemy.debugger) Debug.Log("This is really unlikely !");
    }

}


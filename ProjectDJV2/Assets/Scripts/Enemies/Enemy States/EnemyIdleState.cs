using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : MonoBehaviour, IEnemyState
{
    public float precision = 0.1f;
    [SerializeField] private float Speed = 1f;
    [SerializeField] Vector3 initialPosition;
    [SerializeField] Vector3 lookAtPosition;

    private void Start()
    {
        initialPosition = transform.position;
        lookAtPosition = initialPosition + transform.forward;
    }

    public void OnInitialize(Enemy enemy)
    {
        Debug.Log("IS IDLE");
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
        Debug.Log("This is really unlikely !");
    }

}


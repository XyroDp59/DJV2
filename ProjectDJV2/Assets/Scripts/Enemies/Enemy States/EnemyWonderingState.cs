using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWonderingState : MonoBehaviour, IEnemyState
{
    [SerializeField] float Speed;
    [SerializeField] float radius;
    [SerializeField] float waitDuration = 1f;
    WaitForSeconds waitCooldown;

    public void OnInitialize(Enemy enemy)
    {
        Debug.Log("IS WONDERING");
        enemy.SetSpeed(Speed);
    }

    public void Behave(Enemy enemy)
    {
        //nothing to add here ?
    }

    public void OnDestinationFoundAction(Enemy enemy)
    {
        StartCoroutine(WaitAndWander(enemy));
    }

    IEnumerator WaitAndWander(Enemy enemy)
    {
        if (waitCooldown == null) { waitCooldown = new WaitForSeconds(waitDuration); }
        enemy.SetSpeed(0f);
        yield return waitCooldown;

        enemy.SetSpeed(Speed);
        Vector3 newDest = enemy.lastKnownPlayerPos + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
        enemy.SetDestination(newDest);
    }
}


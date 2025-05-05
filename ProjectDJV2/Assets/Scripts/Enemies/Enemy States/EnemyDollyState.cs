using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDollyState : MonoBehaviour, IEnemyState
{
    float Speed;
    [SerializeField] List<Transform> targetPath = new List<Transform>();
    int currentTargetID = 0;
    float waitDuration = 1f;
    WaitForSeconds waitCooldown;

    public void FirstInitialize(EnemyData data)
    {
        Speed = data.dollySpeed;
        waitDuration = data.dollyWaitDuration;
    }

    public void OnInitialize(Enemy enemy)
    {
        if (enemy.debugger) Debug.Log("IS DOLLY");
        enemy.SetSpeed(Speed);
        enemy.SetDestination(targetPath[0].position);
    }

    public void Behave(Enemy enemy)
    {
        // Agent is doing stuff, nothing to add here.
    }

    public void OnDestinationFoundAction(Enemy enemy)
    {
        StartCoroutine(WaitOnDestinationChange(enemy));
    }

    IEnumerator WaitOnDestinationChange(Enemy enemy)
    {
        currentTargetID = (currentTargetID + 1) % targetPath.Count;
        enemy.SetDestination(targetPath[currentTargetID].position);
        if (enemy.debugger) Debug.Log("path dest id = " + currentTargetID);

        if (waitCooldown == null) { waitCooldown = new WaitForSeconds(waitDuration); }
        enemy.SetSpeed(0f);
        yield return waitCooldown;
        enemy.SetSpeed(Speed);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEnemyState
{
    public void OnInitialize(Enemy enemy);
    public void Behave(Enemy enemy);
    public void OnDestinationFoundAction(Enemy enemy);
}

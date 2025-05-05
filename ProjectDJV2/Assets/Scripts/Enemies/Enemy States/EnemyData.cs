using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    [Header("Senses")]
    [Header("vision")]
    public float angleFOV = 45;
    public float visionRange = 5f;
    public float visualInterest = 2f;

    [Header("Audition")]
    public float auditionRange = 10f;
    public float hearPower = 1f;
    public float auditionFactor = 0.25f;

    [Header("dolly state")]
    public float dollySpeed = 2f;
    public float dollyWaitDuration = 1f;

    [Header("wondering state")]
    public float wonderSpeed = 1f;
    public float wonderRadius = 3f;
    public float wonderWaitDuration = 1f;
    public float wonderTotalDuration = 5f;


    [Header("alerted state")]
    public float alertedSpeed = 2.5f;
    public float alertedDuration = 1f;
}

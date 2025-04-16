using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{

    [Header("Path")]
    [SerializeField] List<Transform> targetPath = new List<Transform>();
    int currentTargetID = 1;
    [SerializeField] float pathPrecision = 0.1f;
    NavMeshAgent agent;

    [Header("Inspection")]
    // if interest <= 0 : the enemy follow its path without caring
    // else if interest < maxInterestThreshold : the enemy is wondering what's happening around PointOfInterest
    // else : the Enemy goes rapidly searching around PointOfInterest
    float interest;
    [SerializeField] float maxInterestThreshold;
    Transform PointOfInterest;
    [SerializeField] float angularSpeed;
    [SerializeField] float inspectionRadius = 2f;
    [SerializeField] float walkingSpeed = 3f;
    [SerializeField] float runningSpeed = 3f;



    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        transform.position = targetPath[0].position;
        agent.destination = targetPath[currentTargetID].position;
    }

    void Update()
    {
        if (Vector3.Distance(agent.destination, transform.position) < pathPrecision) return;
        
        if (PointOfInterest != null)
        {
            if(interest > maxInterestThreshold) //interet maximal
            {

            }
            else if(interest > 0) //interet modéré
            {
                Vector3 randomOffset = inspectionRadius * new Vector3(Random.value, 0, Random.value).normalized;
                StartCoroutine(InspectLocationAndChangeDestination(PointOfInterest.position + randomOffset, 2f, 2));
            }
            else //se désinteresse
            {
                interest = 0;
                PointOfInterest = null;
                StartCoroutine(InspectLocationAndChangeDestination(targetPath[currentTargetID].position, 2f, 2));
            }
        }
        else 
        {
            currentTargetID += 1;
            currentTargetID %= (targetPath.Count - 1);
            StartCoroutine(InspectLocationAndChangeDestination(targetPath[currentTargetID].position, 2f, 2));
        }
    }

    public void TriggerInterest(Transform t, float interest)
    {
        if(interest > this.interest)
        PointOfInterest = t;
        this.interest = interest;
        agent.destination = t.position;

        agent.speed = interest < maxInterestThreshold ? walkingSpeed : runningSpeed;
    }


    #region look around

    IEnumerator InspectLocationAndChangeDestination(Vector3 newDestination, float inspectionDuration, float leftRightNumber)
    {
        agent.isStopped = true;

        int sign = 1;
        StartCoroutine(InspectionLook(sign, inspectionDuration / 2));
        yield return new WaitForSeconds(inspectionDuration / 2);

        for (float n = 1; n < leftRightNumber -1; n++)
        {
            sign *= -1;
            StartCoroutine(InspectionLook(sign, inspectionDuration));
            yield return new WaitForSeconds(inspectionDuration);
        }
        sign *= -1;
        StartCoroutine(InspectionLook(sign, inspectionDuration / 2));
        yield return new WaitForSeconds(inspectionDuration / 2);

        agent.isStopped = false;
        agent.destination = newDestination;
    }

    IEnumerator InspectionLook(int lookClockwise, float lookDuration)
    {
        for (float t = 0; t < lookDuration; t += Time.deltaTime)
        {
            yield return null;
            transform.Rotate(lookClockwise * angularSpeed * Vector3.up * Time.deltaTime);
        }
    }
    #endregion
}

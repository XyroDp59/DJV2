using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;

    [Header("Inspection")]
    UnityEvent<Enemy> OnDestinationFound = new UnityEvent<Enemy>();
    [SerializeField] float attention = 3f;
    [SerializeField] Image attentionBarFill;
    [SerializeField] Gradient attentionBarGradient;
    float interest = 0f;

    public Vector3 lastKnownPlayerPos { get; private set; }

    [Header("Senses")]
    [SerializeField] float auditionRange;
    [SerializeField] float hearPower;

    [Header("States")]
    IEnemyState currentState;
    IEnemyState defaultState;

    [SerializeField] EnemyDollyState idleState;
    [SerializeField] EnemyWonderingState wonderState;
    [SerializeField] EnemyAlertedState alertedState;



    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        defaultState = idleState;
        GoToDefaultState();
    }


    void Update()
    {
        currentState.Behave(this);

        if (Vector3.Distance(agent.destination, agent.transform.position) < 0.5)
        {
            OnDestinationFound.Invoke(this);
            Debug.Log("Destination found !");
        }


        // ============ TRANSITION BACK TO DEFAULT ===============
        if(currentState != (IEnemyState)alertedState)
        {
            interest = Mathf.Clamp(interest - Time.deltaTime, 0, attention);
            SetAttentionBar(interest / attention);
        }
        if (interest <= 0) GoToDefaultState();
    }

    #region public state method
    private void GoToState(IEnemyState nextState)
    {
        if(nextState == currentState) return;

        OnDestinationFound.RemoveAllListeners();
        currentState = nextState;
        OnDestinationFound.AddListener(currentState.OnDestinationFoundAction);

        currentState.OnInitialize(this);
    }

    public void GoToDefaultState()
    {
        GoToState(defaultState);
        interest = 0;
    }

    public void TriggerInterest(Vector3 objectPos, float quantity)
    {
        if(currentState != (IEnemyState)alertedState)
        {
            interest += (1 + quantity) * Time.deltaTime;
            Debug.Log("interest : " + Mathf.Round(interest * 100) / 100 + " / " + attention);
        }
        else alertedState.ResetAlertedTimer();
        lastKnownPlayerPos = objectPos;

        // ============ OTHERS TRANSITION ===============
        if (currentState == defaultState || currentState == (IEnemyState)wonderState)
        {
            if (interest >= attention) GoToState(alertedState);
            else if (interest > 0) GoToState(wonderState);
        }
    }

    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }
    public void SetDestination(Vector3 dest)
    {
        agent.destination = dest;
    }

    public void StopAgent(bool shouldStop)
    {
       agent.isStopped = shouldStop;
    }

    private void SetAttentionBar(float percent)
    {
        attentionBarFill.rectTransform.anchorMax = new Vector2(percent, 1);
        attentionBarFill.color = attentionBarGradient.Evaluate(percent);
    }

    #endregion

    #region Senses

    public void HearPlayerNoises(Vector3 noisePos, float volume)
    {
        bool isInRange = Vector3.Distance(noisePos, transform.position) < auditionRange;
        bool isLoudEnough = volume > hearPower;

        if (isInRange && isLoudEnough)
        {
            TriggerInterest(transform.position, 1);
        }
    }
    #endregion

    /*
    #region look around

    IEnumerator InspectLocationAndChangeDestination(Vector3 newDestination, float inspectionDuration, float leftRightNumber)
    {
        Debug.Log(newDestination);
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
        
        yield return null;
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
    */
}

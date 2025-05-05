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

    [Header("States")]
    IEnemyState currentState;
    IEnemyState defaultState;

    [SerializeField] EnemyDollyState idleState;
    [SerializeField] EnemyWonderingState wonderState;
    [SerializeField] EnemyAlertedState alertedState;
    [SerializeField] EnemyData data;

    public bool debugger = false;

    public EnemyData GetData()
    {
        return data;
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        defaultState = idleState;
        idleState.FirstInitialize(data);
        wonderState.FirstInitialize(data);
        alertedState.FirstInitialize(data);
        GoToDefaultState();
    }


    void Update()
    {
        currentState.Behave(this);

        if (Vector3.Distance(agent.destination, agent.transform.position) < 0.5)
        {
            OnDestinationFound.Invoke(this);
            if(debugger) Debug.Log("Destination found !");
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
            interest = Mathf.Clamp(interest + (1 + quantity) * Time.deltaTime,0,attention);
            if (debugger) Debug.Log("interest : " + Mathf.Round(interest * 100) / 100 + " / " + attention);
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
}

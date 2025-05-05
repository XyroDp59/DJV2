using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySenses : MonoBehaviour
{
    [SerializeField] Enemy enemyBody;

    [Header("vision")]
    float angleFOV;
    float visionRange;
    float visualInterest = 2f;

    [Header("Audition")]
    float auditionRange;
    float hearPower;
    float auditionFactor;

    private void Awake()
    {
        EnemyData data = enemyBody.GetData();
        angleFOV = data.angleFOV;
        visionRange = data.visionRange;
        visualInterest = data.visualInterest;
        auditionRange = data.auditionRange;
        hearPower = data.hearPower;
        auditionFactor = data.auditionFactor;
    }
    private void OnDrawGizmos()
    {
        float a = Mathf.Deg2Rad * angleFOV;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward + transform.right * Mathf.Tan(a / 2)).normalized * visionRange);
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward - transform.right * Mathf.Tan(a / 2)).normalized * visionRange);
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerController p;
        if(other.TryGetComponent(out p))
        {
            // Debug angle entre joueur et enemy
            //Debug.Log("angle : " + Mathf.Abs(Vector3.Angle((p.transform.position - enemyBody.transform.position), enemyBody.transform.forward)));

            RaycastHit hit;
            Physics.Raycast(enemyBody.transform.position, (p.transform.position - enemyBody.transform.position), out hit, visionRange);
            if (hit.collider == null) return;
            bool isInRange = (hit.collider.gameObject == p.gameObject);
            bool isInFOV = Mathf.Abs(Vector3.Angle((p.transform.position - enemyBody.transform.position), enemyBody.transform.forward)) < angleFOV;

            if (isInRange && isInFOV)
            {
                enemyBody.TriggerInterest(other.transform.position, visualInterest);
            }
        }


        SoundEmitter s;
        if (other.TryGetComponent(out s))
        {
            Debug.Log("hear noises");
            bool isInRange = Vector3.Distance(s.transform.position, enemyBody.transform.position) < auditionRange;
            float auditionInterest = s.GetSoundVolume() - hearPower;

            Debug.Log("-----> " + auditionInterest);

            if (isInRange && auditionInterest > 0)
            {
                Debug.Log("AAAH");
                enemyBody.TriggerInterest(other.transform.position, auditionFactor * auditionInterest);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] Enemy enemyBody;

    [SerializeField] float angleFOV;
    [SerializeField] float visionRange;

    private void OnDrawGizmos()
    {
        float a = Mathf.Deg2Rad * angleFOV;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward + transform.right * Mathf.Tan(a / 2)).normalized * visionRange);
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward - transform.right * Mathf.Tan(a / 2)).normalized * visionRange);
    }

    private void OnTriggerStay(Collider other)
    {
        Player p;
        if(other.TryGetComponent(out p))
        {
            // Debug angle entre joueur et enemy
            //Debug.Log("angle : " + Mathf.Abs(Vector3.Angle((p.transform.position - enemyBody.transform.position), enemyBody.transform.forward)));

            bool isInRange = Vector3.Distance(p.transform.position, enemyBody.transform.position) < visionRange;
            bool isInFOV = Mathf.Abs(Vector3.Angle((p.transform.position - enemyBody.transform.position), enemyBody.transform.forward)) < angleFOV;

            if (isInRange && isInFOV)
            {
                enemyBody.TriggerInterest(other.transform.position, 2);
            }
        }
    }
}

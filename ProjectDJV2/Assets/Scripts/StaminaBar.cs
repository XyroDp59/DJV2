using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Image fill;
    

    // Update is called once per frame
    void Update()
    {
        fill.rectTransform.anchorMax = new Vector2(player.GetStamina(), 1f);
    }
}

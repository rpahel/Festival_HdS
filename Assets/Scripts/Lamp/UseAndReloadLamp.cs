using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseAndReloadLamp : MonoBehaviour
{
    public Slider lampStaminaBar;
    public Light lamp;
    public float speedStaminaDown;
    public float maxValueStamina;
    private float valueStamina;
    private bool isPressed = false;

    private void Start()
    {
        lampStaminaBar.maxValue = maxValueStamina;
        lampStaminaBar.value = lampStaminaBar.maxValue;
    }

    private void Update()
    {
        SwitchOnAndOff();
        StaminaManager();
    }

    private void SwitchOnAndOff()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isPressed = !isPressed;
        }
        if (isPressed == true)
        {
            lamp.intensity = 0;
        }
        else
        {
            lamp.intensity = 20;
        }
    }
    private void StaminaManager()
    {
        if (lamp.intensity > 0)
        {
            lampStaminaBar.value -= speedStaminaDown * Time.deltaTime;
            valueStamina = lampStaminaBar.value;
        }
        else
        {
            lampStaminaBar.value = valueStamina;
        }
    }
}

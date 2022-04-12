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
    public float valueReload;

    private float valueStamina;
    private bool isPressed = false;
    private bool canBePressed = true;

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
        if (canBePressed)
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
                lamp.intensity = 3;
            }
        }
        if(lampStaminaBar.value <= 0)
        {
            canBePressed = false;
            lamp.intensity = 0;
        }
        else
        {
            canBePressed = true;
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

        if (lampStaminaBar.value <= lampStaminaBar.maxValue / 6 && Camera.main.fieldOfView >= 4)
        {
            if (!isPressed)
            {
                Camera.main.fieldOfView -= Time.deltaTime; // calcul to be determined with the value of the stamina
            }
        }
        else if (lampStaminaBar.value > lampStaminaBar.maxValue / 6 && Camera.main.fieldOfView <= 10.85f)
        {
            Camera.main.fieldOfView = 10.85f;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("ReloadLampStamina"))
        {
            valueStamina += valueReload;
            lampStaminaBar.value += valueReload;
            Destroy(hit.gameObject);
        }
    }
}

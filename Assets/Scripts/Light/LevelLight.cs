using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLight : MonoBehaviour
{
    public bool isOn;
    public bool isFlickering;

    private void Awake()
    {
        GetComponent<Animator>().SetBool("isOn", isOn);
        GetComponent<Animator>().SetBool("isFlickering", isFlickering);
    }
}

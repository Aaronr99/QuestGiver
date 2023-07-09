using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMusic : MonoBehaviour
{
    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("Audio").Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

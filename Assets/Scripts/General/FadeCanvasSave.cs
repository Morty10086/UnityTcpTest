using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCanvasSave : MonoBehaviour
{
    private static FadeCanvasSave instance;
    public static FadeCanvasSave Instance=>instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
}

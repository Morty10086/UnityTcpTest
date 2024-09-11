using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginButtonSetNormal : MonoBehaviour
{

    private Image buttonImage;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        buttonImage.SetNativeSize();
    }
}

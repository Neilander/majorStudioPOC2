using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endControl : MonoBehaviour
{
    public startControl control;
    // Start is called before the first frame update
    void Start()
    {
        control.doNext();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

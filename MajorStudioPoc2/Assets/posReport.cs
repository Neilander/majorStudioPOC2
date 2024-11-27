using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class posReport : MonoBehaviour
{
    public bool ifLeft;

    // Start is called before the first frame update
    void Start()
    {
        animalManager.Instance.reportLeftRightPos(transform, ifLeft);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

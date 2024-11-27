using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basePosReport : MonoBehaviour
{
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        animalManager.Instance.reportBasePos(transform, index);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

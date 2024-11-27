using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class coolDownText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        animalManager.Instance.reportText(text, index);
        Debug.Log("为什么没触发？");
        gameObject.SetActive(false);
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class bananaLeft : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        animalManager.Instance.reportBananaText(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeLeft(int n)
    {
        text.text = n.ToString();
    }
}

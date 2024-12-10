using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class joker : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void doContinue()
    {
        text.text = "Haha, guess what? No continue!";
    }

    public void doOption()
    {
        text.text = "Haha, guess what? No option!";
    }
}

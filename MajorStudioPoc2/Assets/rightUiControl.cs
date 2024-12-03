using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rightUiControl : MonoBehaviour
{
    public List<Button> btns;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract()
    {
        foreach (Button btn in btns)
        {
            btn.interactable = true;
        }
    }

    public void downInteract()
    {
        foreach (Button btn in btns)
        {
            btn.interactable = false;
        }
    }
}

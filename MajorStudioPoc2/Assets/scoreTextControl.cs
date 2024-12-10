using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scoreTextControl : MonoBehaviour
{
    public Vector2 moveAmount;
    public moveUiControl move;
    public float waitTime;
    public TextMeshProUGUI Scoretext;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startScore(string text)
    {
        Scoretext.text = text;
        move.MoveTo(GetComponent<RectTransform>().anchoredPosition + moveAmount);
    }

    public void vanish()
    {
        Invoke("lalaDestroy", waitTime);
    }

    void lalaDestroy()
    {
        Destroy(gameObject);
    }
}

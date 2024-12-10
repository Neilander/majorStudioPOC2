using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textGenerator : MonoBehaviour
{
    public List<Vector2> textPos;
    public GameObject textPrefab;
    // Start is called before the first frame update
    void Start()
    {
        animalManager.Instance.registerCanvas(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

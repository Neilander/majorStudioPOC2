using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bananaThrower : MonoBehaviour
{
    public GameObject bananaPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 获取鼠标点击位置
            Vector3 mousePosition = Input.mousePosition;

            // 将鼠标屏幕坐标转换为世界坐标
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));

            // 调用 throwBanana
            throwBanana(new Vector3(worldPosition.x, worldPosition.y, 0));
        }
    }

    void throwBanana(Vector3 pos)
    {
        bananaScript banana =  Instantiate(bananaPrefab, new Vector3(transform.position.x, transform.position.y,0), Quaternion.identity).GetComponent<bananaScript>();
        banana.ThrowObject(pos);
    }
}

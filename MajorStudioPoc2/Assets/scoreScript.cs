using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scoreScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    private int score; // 当前分数

    public int tarScore;
    // Start is called before the first frame update
    void Start()
    {
        score = 0; // 初始分数为 0
        UpdateScoreDisplay(); // 更新显示
        animalManager.Instance.reportScoreText(this);
    }

    // 修改分数的方法
    public void ChangeScore(int delta)
    {
        score += delta; // 改变分数
        UpdateScoreDisplay(); // 更新显示
    }

    // 更新分数显示
    private void UpdateScoreDisplay()
    {
        if (text != null)
        {
            text.text = score.ToString(); // 更新 Text 显示
        }
    }

    public bool ifWin()
    {
        return score >= tarScore;
    }
}

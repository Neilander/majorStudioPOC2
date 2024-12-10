using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class startControl : MonoBehaviour
{
    public List<Sprite> spriteList;
    public List<string> stringList;

    public Image displayImage;
    public TextMeshProUGUI displayText;

    public float fadeSpeed = 1.0f;

    private Coroutine fadeCoroutine;
    private int curIndex = 0;
    public int loadIndex;

    [Header("测试用")]
    public int toIndex;
    public bool testBtn = false;

    // Start is called before the first frame update
    void Start()
    {
        if (spriteList.Count > 0 && stringList.Count > 0)
        {
            // Set initial values
            displayImage.sprite = spriteList[0];
            displayText.text = stringList[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        if (testBtn)
        {
            testBtn = false;
            TriggerChange(toIndex);
        }
    }

    public void doNext()
    {
        if (fadeCoroutine != null)
            return;
        if ((curIndex + 1) < spriteList.Count)
        {
            curIndex += 1;
            TriggerChange(curIndex);
        }
        else
        {
            LoadSceneByIndex(loadIndex);
        }
    }

    public void TriggerChange(int index)
    {
        if (index < 0 || index >= spriteList.Count || index >= stringList.Count)
        {
            Debug.LogWarning("Index out of bounds!");
            return;
        }

        // Stop any ongoing fade coroutine
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Start the fade transition
        fadeCoroutine = StartCoroutine(FadeTransition(index));
    }

    private IEnumerator FadeTransition(int targetIndex)
    {
        // Gradually decrease opacity to 0
        for (float alpha = 1.0f; alpha >= 0.0f; alpha -= Time.deltaTime * fadeSpeed)
        {
            SetUIAlpha(alpha);
            yield return null;
        }

        SetUIAlpha(0);

        // Update content
        displayImage.sprite = spriteList[targetIndex];
        displayText.text = stringList[targetIndex];

        // Gradually increase opacity back to 1
        for (float alpha = 0.0f; alpha <= 1.0f; alpha += Time.deltaTime * fadeSpeed)
        {
            SetUIAlpha(alpha);
            yield return null;
        }

        // End the coroutine
        fadeCoroutine = null;
        SetUIAlpha(1);
    }

    // Helper method to set the alpha value of both UI elements
    private void SetUIAlpha(float alpha)
    {
        if (displayImage != null)
        {
            Color imgColor = displayImage.color;
            imgColor.a = alpha;
            displayImage.color = imgColor;
        }

        if (displayText != null)
        {
            Color textColor = displayText.color;
            textColor.a = alpha;
            displayText.color = textColor;
        }
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogWarning($"Scene index {sceneIndex} is out of range!");
        }
    }
}

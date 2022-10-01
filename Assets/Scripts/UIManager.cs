using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Canvas ui;
    private TextMeshProUGUI text;
    private Image panel;
    private Queue<string[]> queue = new Queue<string[]>();
    private bool isDisplayingText;
    
    [SerializeField] private int fadeDuration = 10;

    void Start()
    {
        ui = FindObjectOfType<Canvas>();
        text = ui.GetComponentInChildren<TextMeshProUGUI>();
        panel = ui.GetComponentInChildren<Image>();
    }

    public void DisplayText(string displayText, int duration)
    {
        if (isDisplayingText)
        {
            string[] toQueue = new string[2];
            toQueue[0] = displayText;
            toQueue[1] = duration.ToString();
            queue.Enqueue(toQueue);
        }
        else
        {
            StartCoroutine(FadeInUI(displayText, duration));
        }
    }

    private IEnumerator FadeInUI(string displayText, int duration, bool onlyFadeText = false)
    {
        text.text = displayText;
        isDisplayingText = true;
        if (!onlyFadeText)
        {
            for (int i = 0; i < fadeDuration; i++)
            {
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b,
                    panel.color.a + 100f / 255 / fadeDuration);
                yield return new WaitForSeconds(1f / 60);
            }
        }
        for (int i = 0; i < fadeDuration; i++)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 
                text.color.a + 1f / fadeDuration);
            yield return new WaitForSeconds(1f / 60);
        }
        StartCoroutine(StayTimer(duration));
    }

    private IEnumerator FadeOutUI(bool onlyFadeText = false)
    {
        for (int i = 0; i < fadeDuration; i++)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 
                text.color.a - 1f / fadeDuration);
            if (!onlyFadeText)
            {
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b,
                    panel.color.a - 100f / 255 / fadeDuration);
            }

            yield return new WaitForSeconds(1f / 60);
        }

        if (!onlyFadeText)
        {
            isDisplayingText = false;
        }
    }

    private IEnumerator StayTimer(int duration)
    {
        if (duration == 0)
        {
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(duration + 2 * fadeDuration / 60);
        }

        if (queue.Count == 0)
        {
            StartCoroutine(FadeOutUI());
        }
        else
        {
            StartCoroutine(FadeOutUI(true));
            yield return new WaitForSeconds(fadeDuration / 60f);
            string[] fromQueue = queue.Dequeue();
            StartCoroutine(FadeInUI(fromQueue[0], Int32.Parse(fromQueue[1]), true));
        }
    }
}

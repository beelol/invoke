using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITools
{
    public static IEnumerator TypeText(Text outputText, string inputText, float delay)
    {
        foreach (char character in inputText)
        {
            outputText.text += character;
            yield return new WaitForSeconds(delay);
        }
    }

    public static IEnumerator UnTypeText(Text outputText, float delay)
    {
        for (int i = outputText.text.Length - 1; i >= 0; i--)
        {
            outputText.text = outputText.text.Remove(i);
            yield return new WaitForSeconds(delay);
        }
    }

    public static void Translate(RectTransform rect, Vector2 position, float time)
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, position, time);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreRewardIndicator : MonoBehaviour
{
    public string Text
    {
        get
        {
            return textField.text;
        }
        set
        {
            textField.text = value;
        }
    }

    public Color Color
    {
        get
        {
            return textField.color;
        }
        set
        {
            textField.color = value;
        }
    }

    private Text textField;
    private Vector3 startPosition;
    private void Awake()
    {
        textField = GetComponent<Text>();
    }

    private void Start()
    {
        startPosition = transform.position;
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        while (textField.color.a > 0)
        {
            // Position
            var fromBottomToTopInOneSecond = Camera.main.WorldToScreenPoint(Vector3.zero) * Time.deltaTime;
            fromBottomToTopInOneSecond.x = 0;
            fromBottomToTopInOneSecond.z = 0;

            if (transform.position.y - startPosition.y < 16) // 16 pixels
            {
                transform.position += fromBottomToTopInOneSecond * 0.4f; // 0.4th of the screen in one second
            }
            else
            {
                // Alpha
                var textFieldColor = textField.color;
                textField.color = new Color(textFieldColor.r, textFieldColor.g, textFieldColor.b, textFieldColor.a - 1f * Time.deltaTime);
            }

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}

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

    private Text textField;
    private Vector3 startPosition;
    private void Awake()
    {
        textField = GetComponent<Text>();
        startPosition = transform.position;
    }

    private void Start()
    {
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

            transform.position += fromBottomToTopInOneSecond * 0.2f; // 0.2th of the screen in one second

            // Alpha
            var textFieldColor = textField.color;
            textField.color = new Color(textFieldColor.r, textFieldColor.g, textFieldColor.b, textFieldColor.a - 1f * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}

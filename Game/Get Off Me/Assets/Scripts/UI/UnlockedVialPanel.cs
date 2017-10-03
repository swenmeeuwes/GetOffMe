using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class UnlockedVialPanel : MonoBehaviour {

    public static UnlockedVialPanel Instance;
    public Text title;
    public Image image;
    public Text positiveText;
    public Text negativeText;
    public Text description;

    private Animator animator;

    // Use this for initialization
    void Awake() {
        if (Instance != null)
            Debug.LogWarning("Another UnlockedVialPanel was already instantiated!");

        Instance = this;
    }
    void Start() {
        animator = GetComponent<Animator>();

        gameObject.SetActive(false);
    }
    public void ShowUnlockedVial(VialData vial) {
        title.text = vial.name;
        image.sprite = vial.sprite;
        positiveText.text = vial.positiveEffect.Replace("\\n", "\n");
        negativeText.text = vial.negativeEffect.Replace("\\n", "\n");
        description.text = vial.description;
        gameObject.SetActive(true);

        animator.SetTrigger("SlideIn");
    }
    public void NextPopup() {
        animator.SetTrigger("SlideOut");
        GameManager.Instance.GameOverNextPanel();
    }
}

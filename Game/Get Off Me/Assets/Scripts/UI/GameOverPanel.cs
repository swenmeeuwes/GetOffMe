using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GameOverPanel : MonoBehaviour {
    public static GameOverPanel Instance;

    [SerializeField]
    private GameObject newPersonalBestPanel;

    private GameObject gameOverPanel;
    private Animator animator;

    private void Awake()
    {
        if (Instance != null)
            Debug.LogWarning("Game over panel is already instantiated.");

        Instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        gameObject.SetActive(false);        
    }

    public void Show()
    {
        gameObject.SetActive(true);

        var isNewPersonalBest = ScoreManager.Instance.Score >= ScoreManager.Instance.Highscore + 1;
        newPersonalBestPanel.SetActive(isNewPersonalBest);

        if (PlayerPrefs.GetInt(PlayerPrefsLiterals.SHOW_TUTORIAL.ToString(), 1) == 1 && ScoreManager.Instance.Score > 100)
            PlayerPrefs.SetInt(PlayerPrefsLiterals.SHOW_TUTORIAL.ToString(), 0);

        ScoreManager.Instance.SubmitHighscore(true);

        animator.SetTrigger("SlideIn");
    }
}

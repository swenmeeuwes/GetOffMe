using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyModifierMenu : MonoBehaviour {
    [SerializeField]
    private Text modifierNameTextField;
    [SerializeField]
    private Text statusTextField;
    [SerializeField][Tooltip("Should contain 3 containers from left to right")]
    private Image[] imageContainers; // Left, Middle, Right // Sorted!
    [SerializeField]
    private VialContext vialContext;
    [SerializeField]
    private Text positiveTextField;
    [SerializeField]
    private Text negativeTextField;
    [SerializeField]
    private Text unlockTextField;

    private SaveGameModel saveGameModel;
    private DifficultyModifier selectedModifier;
    private int selectedModifierIndex;
    private Sprite[] vialSprites;

    private void Start()
    {
        saveGameModel = GameManager.Instance.SaveGame;

        vialSprites = new Sprite[vialContext.data.Count];
        for (int i = 0; i < vialContext.data.Count; i++)
        {
            vialSprites[i] = vialContext.data.Where(vial => vial.type == saveGameModel.DifficultyModifiers[i].Type).First().sprite;
        }

        selectedModifierIndex = 0;
        UpdateSelection();
    }

    public void PreviousModifier()
    {
        selectedModifierIndex = PrecedingIndex;

        UpdateSelection();
    }

    public void NextModifier()
    {
        selectedModifierIndex = SucceedingIndex;

        UpdateSelection();
    }
    
    public void ToggleSelected()
    {
        if (!selectedModifier.Unlocked)
            return;

        selectedModifier.Enabled = !selectedModifier.Enabled;
        UpdateSelection();
        GameManager.Instance.Save();
    }

    private void UpdateSelection()
    {
        selectedModifier = saveGameModel.DifficultyModifiers[selectedModifierIndex];

        var modifierContext = vialContext.data.Where(modifier => modifier.type == selectedModifier.Type).First();
        var unlocked = selectedModifier.Unlocked; // UnlockConditionResolver.ConditionsAreMet(modifierContext);

        // Set name
        modifierNameTextField.text = unlocked ? modifierContext.name : "???";

        // Set positive, negative and unlock text
        positiveTextField.text = unlocked ? modifierContext.positiveEffect : "???";
        negativeTextField.text = unlocked ? modifierContext.negativeEffect : "???";

        var progression = UnlockConditionResolver.GetProgression(modifierContext);
        unlockTextField.text = modifierContext.unlockCondition
            .Replace("{{GOAL}}", modifierContext.unlockConditionValue.ToString())
            .Replace("{{CURRENT}}", (modifierContext.unlockConditionValue * progression).ToString())
            .Replace("{{PROGRESSION}}", Mathf.Clamp(Mathf.RoundToInt(progression * 100), 0, 100).ToString());

        // Set image containers
        imageContainers[0].sprite = vialSprites[PrecedingIndex]; // LEFT
        imageContainers[1].sprite = vialSprites[selectedModifierIndex]; // MIDDLE
        imageContainers[2].sprite = vialSprites[SucceedingIndex]; // RIGHT

        // Visualize modifier state in vial
        imageContainers[0].color = ResolveContainerColor(saveGameModel.DifficultyModifiers[PrecedingIndex]);
        imageContainers[1].color = ResolveContainerColor(selectedModifier);
        imageContainers[2].color = ResolveContainerColor(saveGameModel.DifficultyModifiers[SucceedingIndex]);

        // Visualize progression towards vial if the vial is not unlocked
        imageContainers[1].transform.Find("ProgressionText").GetComponent<Text>().text = !unlocked ? Mathf.Clamp(Mathf.RoundToInt(progression * 100), 0, 100) + "%" : "";

        if (selectedModifier.Enabled)
        {
            modifierNameTextField.color = new Color(0, 1, 0);
            //statusTextField.text = "Disable";
        }
        else
        {
            modifierNameTextField.color = new Color(1, 0, 0);
            //statusTextField.text = "Enable";
        }
    }

    private Color ResolveContainerColor(DifficultyModifier difficultyModifier)
    {
        if (!difficultyModifier.Unlocked)
            return Color.black;

        if (!difficultyModifier.Enabled)
            return Color.gray;

        return Color.white;
    }

    private int PrecedingIndex
    {
        get
        {
            if (selectedModifierIndex > 0)
                return selectedModifierIndex - 1;
            return saveGameModel.DifficultyModifiers.Length - 1;
        }
    }

    private int SucceedingIndex
    {
        get
        {
            if (selectedModifierIndex < saveGameModel.DifficultyModifiers.Length - 1)
                return selectedModifierIndex + 1;
            return 0;
        }
    }
}

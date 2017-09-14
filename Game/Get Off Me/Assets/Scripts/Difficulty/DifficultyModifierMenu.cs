using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyModifierMenu : MonoBehaviour {
    // TEMP
    [SerializeField]
    private Sprite[] vialSprites;
    //

    [SerializeField]
    private Text modifierNameTextField;
    [SerializeField]
    private Text statusTextField;
    [SerializeField]
    private Image[] imageContainers; // Left, Middle, Right // Sorted!

    private SaveGameModel saveGameModel;
    private DifficultyModifier selectedModifier;
    private int selectedModifierIndex;
    private void Start()
    {
        saveGameModel = GameManager.Instance.SaveGame;

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
        selectedModifier.Enabled = !selectedModifier.Enabled;
        UpdateSelection();
        GameManager.Instance.Save();
    }

    private void UpdateSelection()
    {
        selectedModifier = saveGameModel.DifficultyModifiers[selectedModifierIndex];
        modifierNameTextField.text = selectedModifier.Type.ToString();

        // Set image containers
        imageContainers[0].sprite = vialSprites[PrecedingIndex]; // LEFT
        imageContainers[1].sprite = vialSprites[selectedModifierIndex]; // MIDDLE
        imageContainers[2].sprite = vialSprites[SucceedingIndex]; // RIGHT

        // Visualize modifier state in vail
        imageContainers[0].color = saveGameModel.DifficultyModifiers[PrecedingIndex].Enabled ? Color.white : Color.gray;
        imageContainers[1].color = selectedModifier.Enabled ? Color.white : Color.gray;
        imageContainers[2].color = saveGameModel.DifficultyModifiers[SucceedingIndex].Enabled ? Color.white : Color.gray;

        if (selectedModifier.Enabled)
        {
            modifierNameTextField.color = new Color(0, 1, 0);
            statusTextField.text = "Disable";
        }
        else
        {
            modifierNameTextField.color = new Color(1, 0, 0);
            statusTextField.text = "Enable";
        }
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

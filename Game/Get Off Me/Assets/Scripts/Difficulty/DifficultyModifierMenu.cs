using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyModifierMenu : MonoBehaviour {
    [SerializeField]
    private Text modifierNameTextField;

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
        selectedModifierIndex--;
        if (selectedModifierIndex < 0)
            selectedModifierIndex = saveGameModel.DifficultyModifiers.Length - 1;

        UpdateSelection();
    }

    public void NextModifier()
    {
        selectedModifierIndex++;
        if (selectedModifierIndex > saveGameModel.DifficultyModifiers.Length - 1)
            selectedModifierIndex = 0;

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
        modifierNameTextField.text = selectedModifier.Name;

        modifierNameTextField.color = selectedModifier.Enabled ? new Color(0, 1, 0) : new Color(1, 0, 0);
    }
}

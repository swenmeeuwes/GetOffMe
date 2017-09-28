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
        selectedModifier.Enabled = !selectedModifier.Enabled;
        UpdateSelection();
        GameManager.Instance.Save();
    }

    private void UpdateSelection()
    {
        selectedModifier = saveGameModel.DifficultyModifiers[selectedModifierIndex];
        modifierNameTextField.text = selectedModifier.Type.ToString();

        var modifierContext = vialContext.data.Where(modifier => modifier.type == selectedModifier.Type).First();
        var unlocked = UnlockConditionResolver.ConditionsAreMet(modifierContext);

        // Set positive, negative and unlock text
        positiveTextField.text = modifierContext.positiveEffect;
        negativeTextField.text = modifierContext.negativeEffect;
        unlockTextField.text = modifierContext.unlockCondition;

        // Set image containers
        imageContainers[0].sprite = vialSprites[PrecedingIndex]; // LEFT
        imageContainers[1].sprite = vialSprites[selectedModifierIndex]; // MIDDLE
        imageContainers[2].sprite = vialSprites[SucceedingIndex]; // RIGHT

        // Visualize modifier state in vail
        //imageContainers[0].color = saveGameModel.DifficultyModifiers[PrecedingIndex].Enabled ? Color.white : Color.gray;
        //imageContainers[1].color = selectedModifier.Enabled ? Color.white : Color.gray;
        //imageContainers[2].color = saveGameModel.DifficultyModifiers[SucceedingIndex].Enabled ? Color.white : Color.gray;

        imageContainers[0].color = ResolveContainerColor(saveGameModel.DifficultyModifiers[PrecedingIndex]);
        imageContainers[1].color = ResolveContainerColor(selectedModifier);
        imageContainers[2].color = ResolveContainerColor(saveGameModel.DifficultyModifiers[SucceedingIndex]);

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
        var vialData = vialContext.data.Where(vial => difficultyModifier.Type == vial.type).First();
        if (!UnlockConditionResolver.ConditionsAreMet(vialData))
        {
            return Color.black;
        }
        else if (!difficultyModifier.Enabled)
        {
            return Color.gray;
        }

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

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    private VisualElement root;
    private VisualElement healthBar;
    private Label healthLabel;
    private Label levelLabel;
    private Label xpLabel;
    // Start is called before the first frame update
    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        healthBar = root.Q<VisualElement>("HealthBar");
        healthLabel = root.Q<Label>("HealthText");

        if (healthBar == null)
        {
            Debug.LogError("element healthbar not found");
        }
        if (healthLabel == null)
        {
            Debug.LogError("element healthtext not found");
        }

        SetValues(30, 30);
    }

    public void SetValues(int currentHP, int maxHP)
    {
        if (healthBar == null || healthLabel == null) 
        {
            Debug.LogError("element healthtext or healthBar not intialized");
        }

        float percent = (float)currentHP / maxHP * 100;

        healthBar.style.width = new Length(percent, LengthUnit.Percent);
        healthLabel.text = $"{currentHP}/{maxHP} HP";
    }

    public void SetLevel (int level)
    {
        if(levelLabel != null)
        {
            levelLabel.text = $"level: {level}";
        }
    }

    public void SetXp(int xp)
    {
        if (xpLabel != null)
        {
            xpLabel.text = $"xp: {xp}";
        }
    }
}

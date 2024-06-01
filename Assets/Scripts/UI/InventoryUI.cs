using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    public Label[] labels = new Label[8];
    private VisualElement root;
    public int selected;
    private int numItems;
    private static InventoryUI instance;
    public int Selected
    {
        get { return selected; }
    }
    // Start is called before the first frame update
    void Start()
    {
        var UIDocument = GetComponent<UIDocument>();
        root = UIDocument.rootVisualElement;

        for (int i = 0; i < 8; i++)
        {
            labels[i] = root.Q<Label>($"Item{i + 1}");
        }
        Clear();
        root.style.display = DisplayStyle.None;
    }

    public void Clear()
    {
        for (int i = 0; i < labels.Length; i++)
        {
            labels[i].text = string.Empty;
            labels[i].style.backgroundColor = new StyleColor(Color.clear);
        }
        selected = 0;
        numItems = 0;
    }

    // Update is called once per frame
    void UpdateSelected()
    {
        for (int i = 0; i < labels.Length; i++)
        {
            if (i == selected)
            {
                labels[i].style.backgroundColor = new StyleColor(Color.blue);
            }
            else
            {
                labels[i].style.backgroundColor = new StyleColor(Color.clear);
            }
        }
    }

    public void SelectNextItem()
    {
        if (numItems == 0)
        {
            return;
        }
        selected = (selected + 1) % numItems;
        UpdateSelected();
    }

    public void SelectPreviousItem()
    {
        if (numItems == 0)
        {
            return;
        }
        selected = (selected - 1 + numItems) % numItems;
        UpdateSelected();
    }

    public void Show(List<Consumable> list)
    {
        selected = 0;
        numItems = list.Count;
        Clear();

        for (int i = 0; i < list.Count && i < labels.Length; i++)
        {
            if (list[i] != null)
            {
                labels[i].text = list[i].name;
            }
        }
        UpdateSelected();
        root.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        root.style.display = DisplayStyle.None;
    }
}

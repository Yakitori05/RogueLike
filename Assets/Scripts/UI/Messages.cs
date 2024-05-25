using UnityEngine;
using UnityEngine.UIElements;

public class Messages : MonoBehaviour
{
    private Label[] labels = new Label[5];
    private VisualElement root;
    // Start is called before the first frame update
    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        for (int i = 0; i < labels.Length; i++)
        {
            labels[i] = root.Q<Label>($"Label{i + 1}");
        }

        Clear();

        AddMessages("Welcome to the dungeon, Adventurer!", Color.blue);
    }

    public void Clear()
    {
        foreach (var label in labels)
        {
            label.text = string.Empty;
        }
    }

    public void MoveUp()
    {
        for (int i = labels.Length - 1; i > 0; i--)
        {
            labels[i].text = labels[i - 1].text;
            labels[i].style.color = labels[i -1].style.color;
        }

        labels[0].text = string.Empty;
    }

    public void AddMessages(string content, Color color)
    {
        MoveUp();

        labels[0].text = content;
    }
}
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject HealthBar;
    public GameObject Messages;
    public GameObject Inventory;
    public GameObject LevelFloor;

    private InventoryUI inventoryUI { get => Inventory.GetComponent<InventoryUI>(); }
    private HealthBar healthBar;
    private Messages messageController;

    public static UIManager Get { get => Instance; }
    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (healthBar != null)
        {
            healthBar = HealthBar.GetComponent<HealthBar>();
            if (healthBar == null)
            {
                Debug.LogError("element healthbar not found");
            }
        }
        else
        {
            Debug.LogError("healthbar is not assigned in uimanager");
        }

        if (Messages != null)
        {
            messageController = Messages.GetComponent<Messages>();
        }

        if (messageController != null)
        {
            messageController.Clear();
            messageController.AddMessages("Welcome to the dungeon, Adventurer!", Color.blue);
        }
    }

    // Update is called once per frame
    public void UpdateHealth(int current, int max)
    {
        healthBar.SetValues(current, max);
    }

    public void UpdateLevel(int level)
    {
        healthBar.SetLevel(level);
    }

    public void UpdateXp(int xp)
    {
        healthBar.SetXp(xp);
    }

    public void AddMessages(string message, Color color)
    {
        if (messageController != null)
        {
            messageController.AddMessages(message, color);
        }
    }
}

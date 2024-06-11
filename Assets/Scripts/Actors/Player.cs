using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading;

[RequireComponent(typeof(Actor))]
public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    private Actor actorComponent;
    private Inventory inventory;
    private bool inventoryIsOpen = false;
    private bool droppingItem = false;
    private bool usingItem = false;
    private bool isonladder = false;


    private void Awake()
    {
        controls = new Controls();
    }

    private void Start()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);

        actorComponent = GetComponent<Actor>();
        
        if (actorComponent != null)
        {
            GameManager.Get.SetPlayer(actorComponent);
        }
        else
        {
            Debug.LogError("no actor component found");
        }
    }

    private void OnEnable()
    {
        controls.Player.SetCallbacks(this);
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.SetCallbacks(null);
        controls.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context, InventoryUI inventoryUI)
    {
        if (context.performed)
        {
            if (inventoryIsOpen)
            {
                Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
                if (direction.y > 0)
                {
                    inventoryUI.SelectPreviousItem();
                }
                else if (direction.y < 0)
                {
                    inventoryUI.SelectPreviousItem();
                }
            }
            else
            {
                Move();
            }
        }
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        
    }

    private void Move()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
        Debug.Log("roundedDirection");
        Action.MoveOrHit(GetComponent<Actor>(), roundedDirection);
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector3 playerPosition = transform.position;
            Consumable item = GameManager.Get.GetItemAtLocation(playerPosition);

            if (item == null)
            {
                Debug.Log("No items present");
            }
            else if (inventory.IsFull)
            {
                Debug.Log("Inventory is full");
            }
            else
            {
                inventory.AddItem(item);
                item.gameObject.SetActive(false);
                GameManager.Get.RemoveItem(item);
                Debug.Log($"Picked up {item.name}");
            }
        }
    }

    public void OnDrop(InputAction.CallbackContext context, InventoryUI inventoryUI)
    {
        if (context.performed)
        {
            if (!inventoryIsOpen)
            {
                inventoryUI.Show(GameManager.Get.player.GetComponent<Inventory>().Items);
                inventoryIsOpen = true;
                droppingItem = true;
            }
        }
    }

    public void OnSelect(InputAction.CallbackContext context, InventoryUI inventoryUI)
    {
        if (context.performed)
        {
            if (inventoryIsOpen)
            {
                Consumable selectedItem = inventory.Items[inventoryUI.Selected];
                inventory.DropItem(selectedItem);

                if (droppingItem)
                {
                    selectedItem.transform.position = transform.position;
                    GameManager.Get.AddItem(selectedItem);
                    selectedItem.gameObject.SetActive(true);
                }
                else if (usingItem)
                {
                    UseItem(selectedItem);
                    Destroy(selectedItem.gameObject);
                }

                inventoryUI.Hide();
                inventoryIsOpen = false;
                droppingItem=false;
                usingItem=false;
            }
        }
    }

    private void UseItem (Consumable item)
    {
        Debug.Log($"Using item: {item.name}");
    }

    public void OnUse(InputAction.CallbackContext context, InventoryUI inventoryUI)
    {
        if (context.performed)
        {
            if (inventoryIsOpen)
            {
                inventoryUI.Show(GameManager.Get.player.GetComponent<Inventory>().Items);
                inventoryIsOpen = true;
                usingItem = true;
            }
        }
    }

    public void CheckForLadder()
    {
        Vector3 playerPosition = transform.position;
        Ladder ladder = GameManager.Get.GetLadderAtLocation(playerPosition);

        if (ladder != null)
        {
            if (ladder.up)
            {
                MapManager.Get.MoveUp();
            }
            else
            {
                MapManager.Get.MoveDown();
            }
        }
    }

    public void OnLadder(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CheckForLadder();
        }
    }

    void Controls.IPlayerActions.OnMovement(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    void Controls.IPlayerActions.OnDrop(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    void Controls.IPlayerActions.OnSelect(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    void Controls.IPlayerActions.OnUse(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}

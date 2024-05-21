using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreItem
{
    public string Name { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }
    public GameObject GameObject { get; set; }
    //public Button PurchaseButton { get; set; }
    //public TMPro.TextMeshProUGUI Title {  get; set; }
    public bool Purchased {  get; set; }
    public string PlayerPrefsId { get; set; }

    public StoreItem(string name, int price, string description, GameObject gameObject, string id)
    {
        Name = name;
        Price = price;
        Description = description;
        GameObject = gameObject;
        PlayerPrefsId = id;
        
        //PurchaseButton = button;
        //Title = title;
    }
}
public class Store : MonoBehaviour
{
    public PlayerInfo Player;
    private float playerCredits;
    public StoreItem core;
    public StoreItem thruster;
    public StoreItem wings;
    public List<StoreItem> StoreItems;
    public Button PurchaseButton;
    public TextMeshProUGUI title;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI playerBalance;

    public GameObject CoreGameObject;

    public GameObject ThrusterGameObject;

    public GameObject WingsGameObject;

    private int currentShipPartIndex;

    // Start is called before the first frame update
    void Start()
    {
        StoreItems = new List<StoreItem>();

        core = new StoreItem("core", 12, "The main hull of the ship. Provides the player with a 2x bonus for any coins gained.", CoreGameObject, "hasCore");
        thruster = new StoreItem("thruster", 15, "The thrusters of the ship. Provides the player with increased speed.", ThrusterGameObject, "hasThruster");
        wings = new StoreItem("wings", 15, "The wings of the ship. Provides the player with increased jump.", WingsGameObject, "hasWings");
        StoreItems.Add(core);
        StoreItems.Add(thruster);
        StoreItems.Add(wings);

        UpdatePlayerShipParts();

        currentShipPartIndex = 0;
        foreach (StoreItem part in StoreItems)
        {
            part.GameObject.SetActive(false);
        }

        UpdateDisplay();

        playerBalance.text = "Balance: " + Player.GetCoins();
    }

    public void Purchase()
    {
        StoreItem purchase = StoreItems[currentShipPartIndex];
        Debug.Log("Found: " + purchase);
        if (Player.CanAfford(purchase.Price))
        {
            purchase.Purchased = true;
            Player.WithdrawCoins(purchase.Price);
            Player.AddShipPart(purchase);
            Debug.Log("Player has purchased: " + purchase.Name + ", balance remaining: " + Player.GetCoins());
            UpdateDisplay();
            playerBalance.text = "Balance: " + Player.GetCoins();
        }
    }

    public void NextItem()
    {
        StoreItems[currentShipPartIndex].GameObject.SetActive(false);
        currentShipPartIndex++;
        if (currentShipPartIndex == StoreItems.Count)
        {
            currentShipPartIndex = 0;
        }
        UpdateDisplay();
    }

    public void PreviousItem()
    {
        StoreItems[currentShipPartIndex].GameObject.SetActive(false);
        currentShipPartIndex--;
        if (currentShipPartIndex < 0)
        {
            currentShipPartIndex = StoreItems.Count - 1;
        }
        UpdateDisplay();
    }

    private bool PlayerOwnsShipPart(StoreItem shipPart)
    {
        string id = shipPart.PlayerPrefsId;
        if (PlayerPrefs.GetInt(id, 0) == 1)
        {
            return true;
        }
        else return false;
    }

    private void UpdatePlayerShipParts()
    {
        foreach (StoreItem item in  StoreItems)
        {
            if (PlayerPrefs.GetInt(item.PlayerPrefsId, 0) == 1)
            {
                Player.AddShipPart(item);
            }
        }
    }

    public void UpdateDisplay()
    {
        StoreItems[currentShipPartIndex].GameObject.SetActive(true);
        title.text = StoreItems[currentShipPartIndex].Name;
        desc.text = StoreItems[currentShipPartIndex].Description;
        PurchaseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Purchase - " + StoreItems[currentShipPartIndex].Price;
        if (PlayerOwnsShipPart(StoreItems[currentShipPartIndex]))
        {
            Debug.Log("Invalid Purchase Option - Already owns part.");
            PurchaseButton.interactable = false;
            PurchaseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Purchased";
        }
        else if (!Player.CanAfford(StoreItems[currentShipPartIndex].Price))
        {
            Debug.Log("Invalid Purchase Option - Can't afford part.");
            PurchaseButton.interactable = false;
        }
        else
        {
            Debug.Log("Valid purchase option.");
            PurchaseButton.interactable = true;
        }
    }
}

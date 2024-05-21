using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    static int Coins;
    static List<StoreItem> ShipParts = new List<StoreItem>();
    public bool JumpBoost = false;
    public bool ExtraCoins = false;
    public bool SpeedBoost = false;
    public CharacterMovement characterMovement;

    private void Start()
    {

        characterMovement = GetComponent<CharacterMovement>();

        if (PlayerPrefs.GetInt("hasWings", 0) == 1)
        {
            JumpBoost = true;
            if (characterMovement!=null)
            {
                characterMovement.jumpHeight = 7.5f;
            }
        }

        if (PlayerPrefs.GetInt("hasThruster", 0) == 1)
        {
            SpeedBoost = true;
            if (characterMovement != null)
            {
                characterMovement.speed = 7.5f;
            }
        }

        if (PlayerPrefs.GetInt("hasCore", 0) == 1)
        {
            ExtraCoins = true;
            if (characterMovement != null)
            {
                characterMovement.coinGain = 2;
            }
        }

        Coins = PlayerPrefs.GetInt("playerCoins", 0);

        Debug.Log("Player's current balance: " + Coins);
        Debug.Log("Jump Boost: " + JumpBoost);
        Debug.Log("Extra Coins: " + ExtraCoins);
        Debug.Log("Speed Boost: " + SpeedBoost);
    }

    public void SetCoins(int coins)
    {
        Coins = coins;
        PlayerPrefs.SetInt("playerCoins", Coins);
    }

    public void IncreaseCoins()
    {
        Coins++;
    }

    public void WithdrawCoins(int coins)
    {
        Coins -= coins;
        PlayerPrefs.SetInt("playerCoins", Coins);
    }

    public Boolean CanAfford(int price)
    {
        if (Coins - price >= 0)
        {
            return true;
        }
        else return false;
    }

    public int GetCoins()
    {
        return Coins;
    }

    public List<StoreItem> GetShipParts()
    {
        return ShipParts;
    }

    public void AddShipPart(StoreItem item)
    {
        ShipParts.Add(item);
        if (item.Name == "wings")
        {
            PlayerPrefs.SetInt("hasWings", 1);
            JumpBoost = true;
            Debug.Log("Jump Boost status after adding: " + JumpBoost);
        }
        else if (item.Name == "thruster")
        {
            PlayerPrefs.SetInt("hasThruster", 1);
            SpeedBoost = true;
            Debug.Log("Speed Boost status after adding: " + SpeedBoost);
        }
        else if (item.Name == "core")
        {
            PlayerPrefs.SetInt("hasCore", 1);
            ExtraCoins = true;
            Debug.Log("Extra Coin Status after adding: " + ExtraCoins);
        }
    }

    public bool HasJumpBoost()
    {
        return JumpBoost;
    }

    public bool HasExtraCoins()
    {
        return ExtraCoins;
    }

    public bool HasSpeedBoost()
    {
        return SpeedBoost;
    }

    public bool HasFullShip()
    {
        if (HasSpeedBoost() && HasJumpBoost() && HasExtraCoins())
        {
            return true;
        }
        else return false;
    }

    public void Reset()
    {
        ExtraCoins = false;
        JumpBoost = false;
        SpeedBoost = false;
        Coins = 0;
    }
}

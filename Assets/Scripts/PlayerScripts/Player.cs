using UnityEngine;
using System.Collections.Generic;


public class PlayerHandling : MonoBehaviour
{


    public static class Player
    {
        public static int money = 15;


    }
    public static void AddMoney(int addedValue) // Adds money by a specified value
    {
        Player.money += addedValue;
    }

    public static void DecreaseMoney(int decreasedValue) // Decreases money by a specified value
    {
        Player.money -= decreasedValue;
    }
    public static void ResetMoney(int newAmount) // Resets money to a specified amount
    {
        Player.money = newAmount;
    }
}
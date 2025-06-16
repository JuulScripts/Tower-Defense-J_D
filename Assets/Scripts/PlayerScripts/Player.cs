using UnityEngine;
using System.Collections.Generic;


public  class PlayerHandling : MonoBehaviour
{


public static class Player
{
    public static int money = 0;


}
    public static void AddMoney(int addedValue)
    {
        Player.money += addedValue;
    }

    public static void DecreaseMoney(int decreasedValue)
    {
        Player.money -= decreasedValue;
    }
}
using System.Collections.Generic;
using UnityEngine;

public class BurgerBox : MonoBehaviour
{
    public List<BurgerData> contents = new List<BurgerData>();

    public void LoadBurger(List<BurgerData> data)
    {
        contents = new List<BurgerData>(data);
    }
}

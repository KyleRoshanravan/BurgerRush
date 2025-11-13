using UnityEngine;
using System;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }
    public int Money { get; private set; }

    public event Action<int> OnMoneyChanged;

    [SerializeField] int startMoney = 200;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Money = startMoney;
        OnMoneyChanged?.Invoke(Money);
    }

    public void AddMoney(int amount)
    {
        Money += amount;
        OnMoneyChanged?.Invoke(Money);
    }

    public void LoseMoney(int amount)
    {
        Money -= amount;
        OnMoneyChanged?.Invoke(Money);
    }
}

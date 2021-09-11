using UnityEngine;
using UnityEngine.UI;
using System;


public class ResourceController : MonoBehaviour

{


    public Button ResourceButton;

    public Image ResourceImage;

    public Text ResourceDescription;

    public Text ResourceUpgradeCost;

    public Text ResourceUnlockCost;

    public AudioSource upgradeSound;

    public AudioSource unlockedSound;



    private ResourceConfig _config;



    private int _level = 1;

    public bool IsUnlocked { get; private set; }

    private void Start()
    {
        ResourceButton.onClick.AddListener(() =>

        {
            if (IsUnlocked)
            {
                UpgradeLevel();
            }
            else
            {
                UnlockResource();
            }

        });

        ResourceButton.onClick.AddListener(UpgradeLevel);

    }

    public void SetConfig(ResourceConfig config)

    {
        _config = config;
        // ToString("0") berfungsi untuk membuang angka di belakang koma
        ResourceDescription.text = $"{ _config.Name } Lv. { _level }\n+{ GetOutput().ToString("0") }";
        ResourceUnlockCost.text = $"Unlock Cost\n{ _config.UnlockCost }";
        ResourceUpgradeCost.text = $"Upgrade Cost\n{ GetUpgradeCost() }";
        SetUnlocked(_config.UnlockCost == 0);
    }



    public double GetOutput()

    {

        return _config.Output * _level;

    }



    public double GetUpgradeCost()

    {

        //return _config.UpgradeCost * _level;

        return Math.Round((_config.UpgradeCost + (_level * _config.UpgradeCost * (0.4 + _level * 0.5))),0,MidpointRounding.ToEven);

        //Math.Round(value, 2,MidpointRounding.ToEven));

    }



    public double GetUnlockCost()

    {

        return _config.UnlockCost;

    }

    public void UpgradeLevel()
    {
        double upgradeCost = GetUpgradeCost();
        if (GameManager.Instance._totalGold < upgradeCost)
        {
            return;
        }

        GameManager.Instance.AddGold(-upgradeCost);
        _level++;
        ResourceUpgradeCost.text = $"Upgrade Cost\n{ GetUpgradeCost() }";
        ResourceDescription.text = $"{ _config.Name } Lv. { _level }\n+{ GetOutput().ToString("0") }";
        upgradeSound.Play();
    }

    public void UnlockResource()
    {
        double unlockCost = GetUnlockCost();
        if (GameManager.Instance._totalGold < unlockCost)
        {
            return;
        }
        SetUnlocked(true);
        GameManager.Instance._totalGold -= unlockCost;
        GameManager.Instance.ShowNextResource();
        AchievementController.Instance.UnlockAchievement(AchievementType.UnlockResource, _config.Name);
        unlockedSound.Play();
    }

    public void UnlockGold()
    {
        if (GameManager.Instance._totalGold > 1000)
        {
            Console.Out.WriteLine("a");
        }
    }



    public void SetUnlocked(bool unlocked)

    {

        IsUnlocked = unlocked;

        ResourceImage.color = IsUnlocked ? Color.white : Color.grey;

        ResourceUnlockCost.gameObject.SetActive(!unlocked);

        ResourceUpgradeCost.gameObject.SetActive(unlocked);

    }



}
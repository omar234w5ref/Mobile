using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Reward
{
    public string name;
    public Sprite slotImage;
    public int xpNeeded;
    public bool playerSkin;
}

public class RewardSystem : MonoBehaviour
{

    public List<Reward> rewards;
    [SerializeField] private GameObject giftSlot;
    [SerializeField] private GameObject Inhalt;



    private const string xpCountKey = "xpCount";
    [SerializeField] private int xpCount = 0;




    void Start()
    {
        Debug.Log("Rewards count: " + rewards.Count);

        xpCount = PlayerPrefs.GetInt(xpCountKey, 0);

        foreach (var reward in rewards)
        {
            GameObject slot = Instantiate(giftSlot, Inhalt.transform);

            slot.GetComponent<Image>().sprite = reward.slotImage;

            if (reward.xpNeeded > xpCount)
            {

                slot.GetComponent<Button>().enabled = true;
            }
            else
            {
                slot.GetComponent<Button>().enabled = false;
            }

            if(reward.playerSkin == true)
            {
               
            }

        }
    }

    void Update()
    {

    }
}

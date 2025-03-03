using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    public List<ShopSlot> powerUpSlots = new List<ShopSlot>();

    [Header("Robot Friend")]
    [SerializeField]private GameObject RobotPrefab;


    //------------RobotFriend
    bool hasRobot = false;
   public bool hasPoisonTrail = false;

    //----------
    private GameObject player;
    public GameObject robotInstance;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        foreach (ShopSlot shopSlot in powerUpSlots)
        {
            if (shopSlot.Name == "RobotFriend")
            {
                hasRobot = true;
                break;
            }
            if (shopSlot.Name == "PoisonTrail")
            {
                hasPoisonTrail = true;
                break;
            }
        }

        if (hasRobot)
        {
            if (robotInstance == null)
            {
                robotInstance = Instantiate(RobotPrefab, transform.position, Quaternion.identity);
            }
        }
        else
        {
            if (robotInstance != null)
            {
                Destroy(robotInstance);
                robotInstance = null;
            }
        }




        


    }



  
}

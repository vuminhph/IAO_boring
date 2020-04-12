using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHider : MonoBehaviour
{
    
    GameObject[] chestArray;
    List<GameObject> chestList = new List<GameObject>();

    int chestNum; 

    void Awake()
    {
        chestArray = GameObject.FindGameObjectsWithTag("Chest");
        chestNum = chestArray.Length;
        for (int i = 0; i < chestNum; i++)
        {
            chestList.Add(chestArray[i]);
        }
    }

    public bool oneChestRemains = false;

    public void CheckChest()
    {
        if (oneChestRemains == false)
        {
            for (int i = 0; i < chestNum; i++)
            {
                if (chestList[i].GetComponent<ChestController>().opened == true)
                {
                    chestList.RemoveAt(i);
                    chestNum--;
                }
            }
            if (chestNum == 1)
            {
                RevealKey();
                oneChestRemains = true;
            }
        }
    }

    void RevealKey()
    {
        ChestController chest = chestList[0].GetComponent<ChestController>();
        chest.dropDown = ChestController.contains.key;
    }

}

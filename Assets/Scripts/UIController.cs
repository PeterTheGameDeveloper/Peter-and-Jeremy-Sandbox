using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject inventory;
    public GameObject armor;
    public GameObject stats;

    private Dictionary<int, GameObject> storedInventoryItems;

    private void Awake()
    {
        inventory.SetActive(false);
        armor.SetActive(false);
        stats.SetActive(false);
        storedInventoryItems = new Dictionary<int, GameObject>();
    }

    public void UIViewer(string targetObjectName)
    {
        GameObject targetObject = null;
        List<GameObject> uiObjects = new() { armor, inventory, stats };
        foreach (GameObject item in uiObjects)
        {
            if (item.name == targetObjectName)
            {
                targetObject = item;
                break;
            }
        }
        if (targetObject != null)
        {
            uiObjects.Remove(targetObject);
            foreach (GameObject uiObject in uiObjects)
            {
                uiObject.SetActive(false);
            }
            if (targetObject.activeSelf)
                targetObject.SetActive(false);
            else
                targetObject.SetActive(true);
        }
    }
    
    public void AddItemToInventory(GameObject inventoryItem)
    {
        // When a new item is added to the inventory, add it to a dictionary that stores the GameObjects for each inventory slot. Also update the sprite for that inventory slot
        if (storedInventoryItems.Count < 6)
        {
            int nextAvailableSlot = 1;
            while (storedInventoryItems.ContainsKey(nextAvailableSlot))
            {
                nextAvailableSlot++;
            }

            storedInventoryItems.Add(nextAvailableSlot, inventoryItem);

            Transform[] components = inventory.transform.GetComponentsInChildren<Transform>();
            foreach (Transform item in components)
            {
                if (item.name == nextAvailableSlot.ToString())
                {
                    item.GetComponent<Image>().sprite = inventoryItem.transform.GetComponent<SpriteRenderer>().sprite;
                    item.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }
            }
        }
    }

    public void AddItemToArmor(Collision2D itemToAdd)
    {
        Transform[] components = armor.transform.GetComponentsInChildren<Transform>();
        foreach (Transform item in components)
        {
            if (item.name == itemToAdd.transform.tag)
            {
                item.GetComponent<Image>().sprite = itemToAdd.transform.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    public void AddStats(float damage, float knockback, float defense)
    {
        Transform[] components = stats.transform.GetComponentsInChildren<Transform>();
        Dictionary<string, float> componentValues = new Dictionary<string, float> 
        {
            { "AttackDamageText", damage },
            { "KnockbackText", knockback },
            { "DefenseText" , defense }
        };

        foreach (string name in componentValues.Keys)
        {
            foreach (Transform item in components)
            {
                if (name == item.name)
                {
                    TextMeshProUGUI text = item.gameObject.GetComponent<TextMeshProUGUI>();
                    if (text != null)
                    {
                        text.SetText(componentValues[name].ToString());
                    }
                }
            }
        }
    }
}

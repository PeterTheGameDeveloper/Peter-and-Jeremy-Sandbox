using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject inventory;
    public GameObject armor;
    public GameObject stats;
    public GameObject hoverBox;
    public GameObject rightClickMenu;

    private Dictionary<int, GameObject> storedInventoryItems;

    private void Awake()
    {
        inventory.SetActive(false);
        armor.SetActive(false);
        stats.SetActive(false);
        hoverBox.SetActive(false);
        rightClickMenu.SetActive(false);
        storedInventoryItems = new Dictionary<int, GameObject>();
    }

    // General UI
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

    // Armor Management
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

    // Stats Management
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

    // Inventory Management
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

    public void DisplayInventoryItemStats(GameObject inventoryButton)
    {
        string slotName = inventoryButton.name;
        Transform[] hoverBoxTexts;

        foreach (int name in storedInventoryItems.Keys)
        {
            if (name.ToString() == slotName)
            {
                hoverBox.transform.position = inventoryButton.transform.position;
                hoverBox.SetActive(true);
                hoverBoxTexts = hoverBox.transform.GetComponentsInChildren<Transform>();

                foreach (Transform item in hoverBoxTexts)
                {
                    if (item.name == "HoverTitle")
                    {
                        item.GetComponent<TextMeshProUGUI>().text = storedInventoryItems[name].name;
                    }
                    else if (item.name == "HoverDescription")
                    {
                        ItemPickups itemStats = storedInventoryItems[name].GetComponent<ItemPickups>();
                        List<string> descriptionList = new();
                        if (itemStats.damage != 0)
                        {
                            descriptionList.Add("Damage: " + itemStats.damage.ToString());
                        }
                        if (itemStats.defense != 0)
                        {
                            descriptionList.Add("Defense: " + itemStats.defense.ToString());
                        }
                        if (itemStats.knockback != 0)
                        {
                            descriptionList.Add("KnockBack: " + itemStats.knockback.ToString());
                        }
                        string finalDescription = string.Join(Environment.NewLine, descriptionList);
                        item.GetComponent<TextMeshProUGUI>().text = finalDescription;
                    }
                }
            }
        }
    }

    public void StopDisplayInventoryItemStats()
    {
        hoverBox.SetActive(false);
    }

    public void ShowRightClickMenu(GameObject inventoryButton)
    {
        if (Input.GetMouseButtonDown(1))
        {
            rightClickMenu.transform.position = Input.mousePosition;
            rightClickMenu.SetActive(true);

            Transform selectedItem = null;
            Transform[] rightClickMenuOptions;
            Debug.Log("Right clicked an inventory item!");
            foreach (int name in storedInventoryItems.Keys)
            {
                if (name.ToString() == inventoryButton.name)
                {
                    selectedItem = storedInventoryItems[name].transform;
                }
                rightClickMenuOptions = rightClickMenu.transform.GetComponentsInChildren<Transform>();
                foreach (Transform item in rightClickMenuOptions)
                {
                    if (item.name == "Option1" & selectedItem.GetComponent<ItemPickups>())
                    {
                        item.GetComponentInChildren<TextMeshProUGUI>().text = "Equip " + selectedItem.name;
                    }
                    else if (item.name == "Option2")
                    {
                        item.GetComponentInChildren<TextMeshProUGUI>().text = "Examine " + selectedItem.name;
                    }
                    else if (item.name == "Option3")
                    {
                        item.GetComponentInChildren<TextMeshProUGUI>().text = "Drop " + selectedItem.name;
                    }
                }
            }
        }
    }

    public void StopDisplayRightClickMenu()
    {
        rightClickMenu.SetActive(false);
    }
}

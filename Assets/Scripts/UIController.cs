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
    public GameObject examineBox;

    private GameObject[] arrayOfUIObjects;
    private List<bool> activeStates;
    private Dictionary<int, GameObject> storedInventoryItems;
    private Dictionary<string, GameObject> equippedArmor;
    private int buttonNumber;

    private void Awake()
    {
        arrayOfUIObjects = CountUIObjects();
        storedInventoryItems = new Dictionary<int, GameObject>();
        equippedArmor = new();
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
    public void AddItemToArmor()
    {
        Transform[] components = armor.transform.GetComponentsInChildren<Transform>();
        foreach (Transform item in components)
        {
            if (item.name == storedInventoryItems[buttonNumber].tag)
            {
                item.GetComponent<Image>().sprite = storedInventoryItems[buttonNumber].transform.GetComponent<SpriteRenderer>().sprite;
                equippedArmor.Add(storedInventoryItems[buttonNumber].tag, storedInventoryItems[buttonNumber]);
                storedInventoryItems.Remove(buttonNumber);

                Transform[] inventoryComponents = inventory.transform.GetComponentsInChildren<Transform>();
                foreach (Transform inventoryItem in inventoryComponents)
                {
                    if (inventoryItem.name == buttonNumber.ToString())
                    {
                        inventoryItem.GetComponent<Image>().sprite = null;
                        inventoryItem.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
                        StopDisplayRightClickMenu();
                    }
                }
                break;
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

    public void ExamineItem()
    {
        if (storedInventoryItems.ContainsKey(buttonNumber))
        {
            activeStates = new();
            foreach (GameObject gameObject in arrayOfUIObjects)
            {
                activeStates.Add(gameObject.activeSelf);
                gameObject.SetActive(false);

                if (gameObject == examineBox)
                {
                    Transform[] childObjects = gameObject.GetComponentsInChildren<Transform>();
                    int index = Array.FindIndex(childObjects, item => item.name == "ExamineText");
                    childObjects[index].GetComponent<TextMeshProUGUI>().text = storedInventoryItems[buttonNumber].GetComponent<ItemPickups>().examineText;
                    gameObject.SetActive(true);
                }
            }
        }
    }

    public void CloseExamineMenu()
    {
        int uiCount = 0;
        foreach (GameObject item in arrayOfUIObjects)
        {
            item.SetActive(activeStates[uiCount]);
            uiCount++;
        }

        examineBox.SetActive(false);
        rightClickMenu.SetActive(false);
    }

    // Right click menu
    public void ShowRightClickMenu(GameObject inventoryButton)
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (inventoryButton.GetComponent<Image>().sprite != null)
            {
                rightClickMenu.transform.position = Input.mousePosition;
                rightClickMenu.SetActive(true);

                Transform selectedItem = null;
                Transform[] rightClickMenuOptions;

                if (storedInventoryItems.ContainsKey(int.Parse(inventoryButton.name)))
                {
                    selectedItem = storedInventoryItems[int.Parse(inventoryButton.name)].transform;
                    buttonNumber = int.Parse(inventoryButton.name);
                }
                rightClickMenuOptions = rightClickMenu.transform.GetComponentsInChildren<Transform>();
                foreach (Transform item in rightClickMenuOptions)
                {
                    if (item.name == "Option1" && selectedItem.GetComponent<ItemPickups>())
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

    // Helper functions
    private GameObject[] CountUIObjects()
    {
        inventory.SetActive(true);
        armor.SetActive(true);
        stats.SetActive(true);
        hoverBox.SetActive(true);
        rightClickMenu.SetActive(true);
        examineBox.SetActive(true);

        arrayOfUIObjects = GameObject.FindGameObjectsWithTag("UI");

        inventory.SetActive(false);
        armor.SetActive(false);
        stats.SetActive(false);
        hoverBox.SetActive(false);
        rightClickMenu.SetActive(false);
        examineBox.SetActive(false);

        return arrayOfUIObjects;
    }
}

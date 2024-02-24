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

    private void Awake()
    {
        inventory.SetActive(false);
        armor.SetActive(false);
        stats.SetActive(false);
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
    
    public void AddItemToInventory()
    {

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

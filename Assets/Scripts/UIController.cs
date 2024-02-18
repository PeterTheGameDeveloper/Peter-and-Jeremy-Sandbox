using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Animator uiAnimator;
    public void Inventory(bool inventoryOpen)
    {
        uiAnimator.SetBool("InventoryOpen", inventoryOpen);
    }
}

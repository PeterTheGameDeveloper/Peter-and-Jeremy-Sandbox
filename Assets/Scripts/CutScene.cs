using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    public static bool isCutSceneOn;
    public Animator camAnim;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isCutSceneOn = true;
            camAnim.SetBool("CutScene1", true);
            Invoke(nameof(StopCutScene), 3f);
        }


    }

    void StopCutScene()
    {
        isCutSceneOn = false;
        camAnim.SetBool("CutScene1", false);
        Destroy(gameObject);
    }
}

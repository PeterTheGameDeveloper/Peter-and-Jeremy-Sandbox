using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove_Ref : MonoBehaviour
{
    public int sceneBuildIndex; //Could use name of scene, but will break if scene names change

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player") 
        {
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }


    }


}

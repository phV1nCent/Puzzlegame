using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class Box : MonoBehaviour
{
    private  bool isOnGoal = false;
    
     void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Goal"))
        {
            isOnGoal = true;
            Debug.Log("Complete");
            GameManager.Instance.CheckWin();
        }
    }
     void OnTriggerExit2D (Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Goal"))
        {
            isOnGoal = false;
        } 
    }
    public bool IsOnGoal()
    {
        return isOnGoal;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

enum BoxType
{ 
    normalBox,
    redBox,
}

public class Box : MonoBehaviour
{
    private  bool isOnGoal = false;
    [SerializeField] private BoxType boxType;
    public bool canMoveLeft = true;

    void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.gameObject.CompareTag("Goal") && boxType == BoxType.normalBox)
            {
                isOnGoal = true;
                Debug.Log("Complete");
                GameManager.Instance.CheckWin();
            }
            if (collision.gameObject.CompareTag("RedGoal") && boxType == BoxType.redBox)
            {
                isOnGoal = true;
                Debug.Log("Complete");
                GameManager.Instance.CheckWin();
            }
            if (this.boxType == BoxType.normalBox)
            {
                if (collision.gameObject.CompareTag("Box"))
                {
                    Debug.Log(collision.gameObject.name);
                    if (collision.gameObject.name == "Right")
                    {
                        Debug.LogError("Collider with right of another Box");
                        canMoveLeft = false;
                    }
                }
            }
    }
     void OnTriggerExit2D (Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Goal"))
        {
            isOnGoal = false;
        } 
        else if (collision.gameObject.CompareTag("RedGoal"))
        {
            isOnGoal = false;
        }
    }
    public bool IsOnGoal()
    {
        return isOnGoal;
    }
}

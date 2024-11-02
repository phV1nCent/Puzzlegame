using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton parttern ( Viết tắt )
    public Box[] boxes; // mảng chứa box trong scene
    public int totalGoals; // tổng số Goal trong scene

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        boxes = FindObjectsOfType<Box>();   // Tìm box trong scene
        totalGoals = GameObject.FindGameObjectsWithTag("Goal").Length; //Số goal trong scene gắn tag 
    }

    public void CheckWin()
    {
        int boxesOnGoal = 0;
        foreach (Box box in boxes)
        {
            if (box.IsOnGoal())    // Check box vào đúng Goal chưa
            {
                boxesOnGoal++;
            }
        }

        if (boxesOnGoal == totalGoals)  // Số box đặt đúng vị trí trên goal bằng tổng sô goal
        {
            Debug.Log("Level Complete!");
            Invoke("LoadNextLevel", 1f); // Qua màn
        }
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1; // Index của scene hiện tại
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)  // Check xem còn scene ?
        {
            SceneManager.LoadScene(nextSceneIndex); // Load scene tiếp theo
        }
        else
        {
            Debug.Log("Game Complete!");
        }
    }
}


// Cách viết đầy đủ
/*
private GameManager instance;
public GameManager Instance 
{
    get { return instance; }
    private set { instance = value; }
}
*/
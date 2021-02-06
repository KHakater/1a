using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SelectScript : MonoBehaviour
{
    public static int StageNumber;
    // Start is called before the first frame update
    public static int GetStage()
    {
        return StageNumber;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StageNumber = 01;
            SceneManager.LoadScene("playstage");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StageNumber = 02;
            SceneManager.LoadScene("playstage");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StageNumber = 03;
            SceneManager.LoadScene("playstage");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StageNumber = 04;
            SceneManager.LoadScene("playstage");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StageNumber = 05;
            SceneManager.LoadScene("playstage");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            StageNumber = 06;
            SceneManager.LoadScene("playstage");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            StageNumber = 07;
            SceneManager.LoadScene("playstage");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            StageNumber = 08;
            SceneManager.LoadScene("playstage");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            StageNumber = 09;
            SceneManager.LoadScene("playstage");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StageNumber = 10;
            SceneManager.LoadScene("playstage");
        }
    }
}

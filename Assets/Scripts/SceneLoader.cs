using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadLogin(){
        SceneManager.LoadScene(0);
    }

    public void LoadRegister(){
        SceneManager.LoadScene(1);
    }
}

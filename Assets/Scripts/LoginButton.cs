using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Proyecto26;
using UnityEngine.SceneManagement;

public class LoginButton : MonoBehaviour
{
    public TMP_InputField _userName;
    public TMP_InputField _password;
    public TMP_Text _ErrorMessage;
    User user = new User();
    public static string userName;
    public static string password;
    
    public void OnLogin()
    {
        if(string.IsNullOrWhiteSpace(_userName.text))
        {
            _ErrorMessage.text = "USERNAME FIELD CANNOT BE EMPTY!";
            return;
        }else if (string.IsNullOrWhiteSpace(_password.text))
        {
            _ErrorMessage.text = "PASSWORD FIELD CANNOT BE EMPTY!";
            return;
        }

        userName = _userName.text;
        password = _password.text;

        RetrieveFromDatabase();
    }

    public void RetrieveFromDatabase()
    {
        RestClient.Get<User>("https://smart-gloves-app-6b71b-default-rtdb.asia-southeast1.firebasedatabase.app/Users/" + userName + ".json").Then(response
        => 
        {
            if (response != null)
            {
                user = response;
                if(user.Password != password){
                    _ErrorMessage.text = "INCORRECT PASSWORD!";
                    return;
                }
            
                if(user.UserType == "CareTaker"){
                    SceneManager.LoadScene(2);
                }else{
                    SceneManager.LoadScene(3);
                }
                
            }
            else
            {
                _ErrorMessage.text = "USERNAME DOES NOT EXIST! PLEASE REGISTER!";
            }
        }).Catch(error =>
        {
            _ErrorMessage.text = "USERNAME DOES NOT EXIST! PLEASE REGISTER!";
        });
    }
}

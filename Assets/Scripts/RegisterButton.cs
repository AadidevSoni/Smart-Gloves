using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Proyecto26;

public class RegisterButton : MonoBehaviour
{
    public TMP_InputField _userName;
    public TMP_InputField _emailAddress;
    public TMP_InputField _password;
    public TMP_InputField _confirmedPassword;
    public TMP_Dropdown _userType;

    public static string userName;
    public static string emailAddress;
    public static string password;
    public static string confirmedPassword;
    public static string userType;
    public TMP_Text _ErrorMessage;

    public void OnRegister()
    {
        if (string.IsNullOrWhiteSpace(_userName.text))
        {
            _ErrorMessage.text = "USERNAME FIELD CANNOT BE EMPTY!";
            return;
        }else if (string.IsNullOrWhiteSpace(_emailAddress.text))
        {
            _ErrorMessage.text = "EMAIL ADDRESS FIELD CANNOT BE EMPTY!";
            return;
        }else if (string.IsNullOrWhiteSpace(_password.text))
        {
            _ErrorMessage.text = "PASSWORD FIELD CANNOT BE EMPTY!";
            return;
        }else if (string.IsNullOrWhiteSpace(_confirmedPassword.text))
        {
            _ErrorMessage.text = "CONFIRM PASSWORD FIELD CANNOT BE EMPTY!";
            return;
        }

        if(_password.text != _confirmedPassword.text)
        {
            _ErrorMessage.text = "PASSWORD DOESN'T MATCH!";
            return;
        }

        if(!_emailAddress.text.Contains("@gmail.com"))
        {
            _ErrorMessage.text = "INVALID EMAIL ADDRESS!";
            return;
        }

        RestClient.Get<User>("https://smart-gloves-app-6b71b-default-rtdb.asia-southeast1.firebasedatabase.app/Users/" + userName + ".json").Then(response
        => 
        {
            if (response != null)
            {  
                    
            }
            else
            {
                _ErrorMessage.text = "USERNAME ALREADY TAKEN! TRY ANOTHER!";
                return;
            }
        }).Catch(error =>
        {
            _ErrorMessage.text = "USERNAME ALREADY TAKEN! TRY ANOTHER!";
        });

        RestClient.Get<User>("https://smart-gloves-app-6b71b-default-rtdb.asia-southeast1.firebasedatabase.app/Users/" + emailAddress + ".json").Then(response
        => 
        {
            if (response != null)
            {  
                     
            }
            else
            {
                _ErrorMessage.text = "EMAIL ADDRESS ALREADY IN USE! TRY LOGIN!";
                return;
            }
        }).Catch(error =>
        {
            _ErrorMessage.text = "EMAIL ADDRESS ALREADY IN USE! TRY LOGIN!";
        });

        userName = _userName.text;
        emailAddress = _emailAddress.text;
        password = _password.text;
        userType = _userType.options[_userType.value].text;

        PostToDatabase();
    }

    public void PostToDatabase()
    {
        User user = new User();
        RestClient.Put("https://smart-gloves-app-6b71b-default-rtdb.asia-southeast1.firebasedatabase.app/Users/" + userName + ".json", user);
        _ErrorMessage.text = "REGISTERED! GO BACK TO LOGIN WINDOW!";
    }
}

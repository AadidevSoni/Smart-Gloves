using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class User
{
    public string UserName;
    public string EmailAddress;
    public string Password;
    public string UserType;
    
    public User()
    {
        UserName = RegisterButton.userName;
        EmailAddress = RegisterButton.emailAddress;
        Password = RegisterButton.password;
        UserType = RegisterButton.userType;
    }
}

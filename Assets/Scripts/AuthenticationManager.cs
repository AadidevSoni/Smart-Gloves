using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;

public class AuthenticationManager : MonoBehaviour
{
    // Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    // Login variables
    [Header("Login")]
    public TMP_InputField LoginEmail;
    public TMP_InputField LoginPassword;
    public TMP_Text warningLoginText;

    // Register variables
    [Header("Register")]
    public TMP_InputField RegisterUserName;
    public TMP_InputField RegisterEmail;
    public TMP_InputField RegisterPassword;
    public TMP_InputField RegisterConfirmPassword;
    public TMP_Text warningRegisterText;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                warningLoginText.text = "SERVER DOWN! ERROR: " + dependencyStatus;
            }
        });
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    // Button listeners
    public void LoginButton() => StartCoroutine(Login(LoginEmail.text, LoginPassword.text));
    public void RegisterButton() => StartCoroutine(Register(RegisterEmail.text, RegisterPassword.text, RegisterUserName.text));

    public void GoToRegisterScene() => SceneManager.LoadScene(1); // Adjust index as needed
    public void GoToLoginScene() => SceneManager.LoadScene(0);    // Adjust index as needed

    // Login Coroutine
    private IEnumerator Login(string _email, string _password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {   
            warningLoginText.text = "LOGIN FAILED: " + loginTask.Exception;
            FirebaseException firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = errorCode switch
            {
                AuthError.MissingEmail => "Missing Email!",
                AuthError.MissingPassword => "Missing Password!",
                AuthError.WrongPassword => "Wrong Password!",
                AuthError.InvalidEmail => "Invalid Email!",
                AuthError.UserNotFound => "Account does not exist!",
                _ => "Login Failed!"
            };

            warningLoginText.text = message;
        }
        else
        {
            User = loginTask.Result.User;
            warningLoginText.text = "";
            SceneManager.LoadScene(2); 
        }
    }

    // Register Coroutine
    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (string.IsNullOrEmpty(_username))
        {
            warningRegisterText.text = "Missing Username!";
            yield break;
        }
        if (_password != RegisterConfirmPassword.text)
        {
            warningRegisterText.text = "Passwords do not match!";
            yield break;
        }
        if (string.IsNullOrEmpty(_email) || !_email.Contains("@") || !_email.Contains("."))
        {
            warningRegisterText.text = "Please enter a valid email!";
            yield break;
        }

        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {   
            warningRegisterText.text = "REGISTER FAILED: " + registerTask.Exception;
            FirebaseException firebaseEx = registerTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = errorCode switch
            {
                AuthError.MissingEmail => "Missing Email!",
                AuthError.MissingPassword => "Missing Password!",
                AuthError.WeakPassword => "Weak Password!",
                AuthError.EmailAlreadyInUse => "Email already in use!",
                _ => "Register Failed!"
            };

            warningRegisterText.text = message;
        }
        else
        {
            User = registerTask.Result.User;
            if (User != null)
            {
                var profile = new UserProfile { DisplayName = _username };
                var profileTask = User.UpdateUserProfileAsync(profile);
                yield return new WaitUntil(() => profileTask.IsCompleted);

                if (profileTask.Exception != null)
                {
                    warningRegisterText.text = "Username Set Failed!";
                }
                else
                {
                    SceneManager.LoadScene(0); 
                }
            }
        }
    }
}


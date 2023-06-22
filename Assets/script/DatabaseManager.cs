using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Database;
using Firebase;

public class DatabaseManager : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_InputField confirmPassword;
    public TMP_InputField loginusername;
    public TMP_InputField loginpassword;
    public TextMeshProUGUI messageText;
    private string userID;
    private DatabaseReference reference;

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        messageText.text = "";
    }

    public void SignUp()
{
    if (password.text != confirmPassword.text)
    {
        messageText.text = "Passwords do not match";
        return;
    }

    if (password.text.Length < 8)
    {
        messageText.text = "Password should be at least 8 characters long";
        return;
    }

    DatabaseReference userRef = reference.Child("users").Child(username.text);

    userRef.GetValueAsync().ContinueWith(task =>
    {
        if (task.IsFaulted)
        {
            Debug.Log("Error checking for user: " + task.Exception);
            return;
        }

        DataSnapshot snapshot = task.Result;
        if (snapshot.Exists)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                messageText.text = "User already exists!";
            });
            return;
        }

        User newUser = new User(password.text);
        string json = JsonUtility.ToJson(newUser);
        reference.Child("users").Child(username.text).SetRawJsonValueAsync(json).ContinueWith(createUserTask =>
        {
            if (createUserTask.IsFaulted)
            {
                Debug.Log("Error creating user: " + createUserTask.Exception);
                return;
            }

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                username.text = "";
                password.text = "";
                confirmPassword.text = "";
                messageText.text = "User created!";
                SceneManager.LoadScene("scn_login");
            });
        });
    });
}

    public IEnumerator GetPassword(System.Action<string> onCallback)
    {
        var userRef = reference.Child("users").Child(loginusername.text);

        var passwordTask = userRef.GetValueAsync();
        yield return new WaitUntil(() => passwordTask.IsCompleted);

        if (passwordTask.Exception != null)
        {
            Debug.Log("Error retrieving password: " + passwordTask.Exception.Message);
            yield break;
        }

        DataSnapshot snapshot = passwordTask.Result;

        if (snapshot.Exists)
        {
            string storedPassword = snapshot.Child("password").Value.ToString();
            onCallback.Invoke(storedPassword);
        }
        else
        {
            Debug.Log("User not found!");
            onCallback.Invoke(null);
        }
    }

    public void AuthenticatePassword()
{
    if (string.IsNullOrEmpty(loginusername.text) || string.IsNullOrEmpty(loginpassword.text))
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            messageText.text = "Please fill in both username and password";
        });
        return;
    }

    StartCoroutine(GetPassword((string name) =>
    {
        if (name == loginpassword.text)
        {
            Debug.Log("Success");
            loginpassword.text = "";
            loginusername.text = "";

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                SceneManager.LoadScene("scn_lobby");
            });
        }
        else
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                messageText.text = "Incorrect Email or Password";
            });
            loginpassword.text = "";
            loginusername.text = "";
        }
    }));
}
}

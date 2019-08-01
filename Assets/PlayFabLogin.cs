using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{

    public string PlayerID = "TestPlayer1";
    
    public string playerName;
    public int age;
    public Color colour;

    public string savedPlayerName;
    public int savedAge;
    public Color savedColour;
    
    private void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
            PlayFabSettings.TitleId = "115DF";
        
        var request = new LoginWithCustomIDRequest { CustomId = PlayerID, CreateAccount = true};
        
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login Success - successful API call");
    }

    void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Login Failure - something went wrong");
        Debug.LogError(error.GenerateErrorReport());
    }

    [ContextMenu("Set User Data")]
    public void SetUserData()
    {
        TrySetUserData("Name", playerName);
    }
    
    void TrySetUserData(string dataname, string data)
    {

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
            Data = new Dictionary<string, string>()
            {
                {dataname, data},
            }
        },
        result => Debug.Log("Successfully updated user data"),
        error =>
        {
            Debug.Log("Got error setting user data");
            Debug.LogError(error.GenerateErrorReport());
        });
        
    }

    [ContextMenu("Get User Data")]
    public void GetUserData()
    {
        TryGetUserData("Name");
    }

    void TryGetUserData(string data)
    {

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
//                PlayFabId = PlayerID,
                Keys = null
            },
            result =>
            {
                Debug.Log("Got User Data:");
                if (result.Data == null || !result.Data.ContainsKey(data)) Debug.Log("No " + data);
                else Debug.Log(data + ": " + result.Data[data].Value);
            },
            (error) =>
            {
                Debug.Log("Got error retrieving " + data + " from user data:");
                Debug.LogError(error.GenerateErrorReport());
            });

    }
    
}
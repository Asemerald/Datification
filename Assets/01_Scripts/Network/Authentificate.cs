using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using Unity.Netcode;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Vivox;

public class Authentificate : NetworkBehaviour
{
    protected async void Start()
    {
        await UnityServices.InitializeAsync();
        
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await VivoxService.Instance.InitializeAsync();
        VivoxManager.Instance.LoginToVivoxAsync();
        
    }
}

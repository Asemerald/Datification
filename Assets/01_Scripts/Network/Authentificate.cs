using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using Unity.Netcode;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Vivox;
using Utils;

public class Authentificate : LocalSingleton<Authentificate>
{

    public event EventHandler<EventArgs> OnAuthentificateSuccess;

    private void Start()
    {
        Initialise();
    }

    private async void Initialise()
    {
        try
        {
            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            await VivoxService.Instance.InitializeAsync();
            VivoxManager.Instance.LoginToVivoxAsync();
            
            OnAuthentificateSuccess?.Invoke(this, EventArgs.Empty);

        }
        catch (Exception e)
        {
            await WaitDelay.Instance.WaitFor(2).ContinueWith(_ =>
            {
                Debug.LogError("An error occurred during the authentification process: " + e.Message);
                Initialise();
            });
        }
    }
    
    
}

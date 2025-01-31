using System;
using System.Collections;
using Network;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;
using UnityEngine;
using Utils;

public class Authentificate : InstanceBase<Authentificate>
{
    public event EventHandler<EventArgs> OnAuthentificateSuccess;

    private void Start()
    {
        StartCoroutine(WaitForUnityMainThread());
    }

    private IEnumerator WaitForUnityMainThread()
    {
        while (UnityMainThread.wkr == null && VivoxManager.Instance == null)  // Wait until UnityMainThread initializes
            yield return null;

        Initialise();
    }


    private void Initialise()
    {
        UnityMainThread.wkr.AddJobAsync(async () =>
        {
            try
            {
                await UnityServices.InitializeAsync();

                if (AuthenticationService.Instance.IsSignedIn)  // Prevent double sign-in
                {
                    Debug.LogWarning("Already signed in. Skipping authentication.");
                }
                else
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }

                AuthenticationService.Instance.SignedIn += () =>
                {
                    Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
                };

                await VivoxService.Instance.InitializeAsync();
                VivoxManager.Instance.LoginToVivoxAsync();

                OnAuthentificateSuccess?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                await WaitDelay.Instance.WaitFor(2);
                Debug.LogError("An error occurred during authentication: " + e.Message);
                Initialise(); // Retry authentication
            }
        });
    }

}
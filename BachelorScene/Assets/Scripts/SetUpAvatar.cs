using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Platform;
using Oculus.Platform.Models;
using UnityEngine;

public class SetUpAvatar : MonoBehaviour
{
    public OvrAvatar avatarPrefab;

    void Start()
    {
        Oculus.Platform.Core.Initialize("2669604449819209");
        Oculus.Platform.Users.GetLoggedInUser().OnComplete(OnGetLoggedInUser);
    }

    private void OnGetLoggedInUser(Message<User> message)
    {
        if (!message.IsError)
        {
            OvrAvatar avatar = Instantiate(avatarPrefab.gameObject).GetComponent<OvrAvatar>();

            Debug.Log(message.Data.ID);
            //avatar.oculusUserID = message.Data.ID.ToString();
        }
    }

}

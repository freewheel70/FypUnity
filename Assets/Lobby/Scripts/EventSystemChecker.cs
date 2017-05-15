using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Prototype.NetworkLobby
{
    public class EventSystemChecker : MonoBehaviour
    {
        //public GameObject eventSystem;
        private bool initialized = false;

        // Use this for initialization
        void Awake()
        {
            if (!FindObjectOfType<EventSystem>() && !initialized)
            {
                Debug.Log("EventSystemChecker run!");
                //Instantiate(eventSystem);
                GameObject obj = new GameObject("EventSystem");
                obj.AddComponent<EventSystem>();
                obj.AddComponent<StandaloneInputModule>().forceModuleActive = true;

                initialized = true;
            }
        }
    }
}
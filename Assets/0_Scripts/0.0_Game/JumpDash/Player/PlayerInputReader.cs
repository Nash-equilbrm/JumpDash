using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Game.Player
{
    public class PlayerInputReader : MonoBehaviour
    {
        public UnityEvent onMouseClicked;
        public UnityEvent onScreenTouched;


        private void Update()
        {
#if UNITY_STANDALONE // PC (Standalone + Unity Editor)
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                onMouseClicked.Invoke();
            }
#elif UNITY_WEBGL  // WebGL (Runs on both desktop and mobile browsers)
        if (Input.GetMouseButtonDown(0)) 
        {
            onMouseClicked.Invoke();
        }
        if (Input.touchCount > 0) // Detect touch input on mobile browsers
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                onScreenTouched.Invoke();
            }
        }
#elif UNITY_IOS || UNITY_ANDROID // Native Mobile Apps
        if (Input.touchCount > 0) // Detect touch input
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                onScreenTouched.Invoke();
            }
        }
#endif
        }
    }

}

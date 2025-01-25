using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    public static bool tap, swipeleft, swiperight, swipeup, swipedown;
    private bool isDraging = false;
    private Vector2 startTouch, swipeDelta;
    private float minSwipeDistance = 125f;

    private void Update()
    {
        // Reset semua flag
        tap = swipedown = swipeleft = swiperight = swipeup = false;

        #region Input Detection
        if (Input.GetMouseButtonDown(0) ||
            (Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Began))
        {
            tap = true;
            isDraging = true;
            startTouch = Input.touches.Length > 0 ? Input.touches[0].position : (Vector2)Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0) ||
                 (Input.touches.Length > 0 &&
                  (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)))
        {
            Reset();
        }
        #endregion

        // Hitung swipe delta
        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            swipeDelta = Input.touches.Length > 0
                ? Input.touches[0].position - startTouch
                : (Vector2)Input.mousePosition - startTouch;

            // Deteksi swipe
            if (swipeDelta.magnitude > minSwipeDistance)
            {
                float x = swipeDelta.x;
                float y = swipeDelta.y;

                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    // Horizontal swipe
                    swipeleft = x < 0;
                    swiperight = x > 0;
                }
                else
                {
                    // Vertikal swipe
                    swipeup = y > 0;
                    swipedown = y < 0;
                }

                Reset();
            }
        }
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }
}
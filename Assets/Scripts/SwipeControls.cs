using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControls : MonoBehaviour
{
    Vector2 swipeStart;
    Vector2 swipeEnd;
    float minimumDistance = 10;

    //Delegate to store a reference to an actual function
    public static event System.Action<SwipeDirection> OnSwipe = delegate {};


    public enum SwipeDirection
    {
        Up,Down,Left,Right
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Touch touch in Input.touches)
        {
            //if finger has began touching
            if (touch.phase == TouchPhase.Began)
            {
                swipeStart = touch.position;
            }
            //if finger has ended the touch
            else if (touch.phase == TouchPhase.Ended)
            {
                swipeEnd = touch.position;
                ProcessSwipe();
            }
        }
        //mouse touch simulation
        if(Input.GetMouseButtonDown(0))
        {
            swipeStart = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            swipeEnd = Input.mousePosition;
            ProcessSwipe();
        }
    }
    void ProcessSwipe()
    {
        float distance = Vector2.Distance(swipeStart,swipeEnd);
        if (distance > minimumDistance)
        {
            if (isVerticalSwipe())
            {
                if (swipeEnd.y > swipeStart.y)
                {
                    OnSwipe(SwipeDirection.Up);
                }
                else
                {
                    OnSwipe(SwipeDirection.Down);
                }
            }
            else //horizontal
            {
                if (swipeEnd.x > swipeStart.x)
                {
                    OnSwipe(SwipeDirection.Right);
                }
                else
                {
                    OnSwipe(SwipeDirection.Left);
                }
            }
        }
    }

    bool isVerticalSwipe()
    {
        float vertical = Mathf.Abs(swipeEnd.y - swipeStart.y);
        float horizontal = Mathf.Abs(swipeEnd.x - swipeStart.x);
        if (vertical > horizontal)
            return true;
        return false;
    }
}

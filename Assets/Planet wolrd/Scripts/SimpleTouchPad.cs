using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class SimpleTouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler{

    public float smoothing;

    private Vector2 origin;
    private Vector2 direction;
    private Vector2 smoothDirection;
    private bool touched;
    private int pointerID;

    void Awake(){
        direction = Vector2.zero;
        touched = false;    
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData){
        // Set our start point
        if (!touched)
        {
            touched = true;
            pointerID = eventData.pointerId;
            origin = eventData.position;
        }
      
    }

    void IDragHandler.OnDrag(PointerEventData eventData){
        // compare the difference between our start point and current point
        if (eventData.pointerId == pointerID)
        {
            Vector2 currentPoint = eventData.position;
            Vector2 directionRaw = currentPoint - origin;
            direction = directionRaw.normalized;
            Debug.Log(direction);
        }

    }


    void IPointerUpHandler.OnPointerUp(PointerEventData eventData){
        //reset everything
        if(eventData.pointerId == pointerID)
        {
            direction = Vector2.zero;
            touched = false;
        }
  
    }

    public Vector2 GetDirection(){
        smoothDirection = Vector2.MoveTowards(smoothDirection, direction, smoothing);
        return smoothDirection;
    }
}

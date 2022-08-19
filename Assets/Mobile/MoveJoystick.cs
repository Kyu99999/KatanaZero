using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Image point;
    private Vector2 originalPoint = Vector2.zero;

    public float radius;
    private RectTransform rectTr;

    private Vector2 direction;
    private Vector2 dir = Vector2.zero;

    private void Start()
    {
        rectTr = GetComponent<RectTransform>();
        originalPoint = rectTr.position;        //ÇÇ¹þ Á¤Áß¾Ó
    }


    public float GetAxis(string axis)
    {
        switch (axis)
        {
            case "Horizontal":
                return dir.x;
            case "Vertical":
                return dir.y;
        }
        return 0f;
    }


    public void OnDrag(PointerEventData eventData)
    {
        var to = eventData.position - originalPoint;
        to.Normalize();
        var from = Vector2.up;

        direction = to - from;

        var degree = Mathf.Abs(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg * 2);
        
        if (degree >= 45 && degree <= 135)  //D
        {
            dir = new Vector2(1, 0);
        }
        else if (degree >= 135 && degree <= 225)  //S
        {
            dir = new Vector2(0, -1);
        }
        else if (degree >= 225 && degree <= 315)    //A
        {
            dir = new Vector2(-1, 0);
        }
        else if ((degree >= 315 && degree <= 360) || degree >= 0 && degree <= 45 ) //W
        {
            dir = new Vector2(0, 1);
        }

        Debug.Log(dir);

        var newPos = eventData.position;
        var direction2 = newPos - originalPoint;
        if (direction2.magnitude > radius)
        {
            newPos = originalPoint + direction2.normalized * radius;
        }
      

        point.rectTransform.position = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        direction = Vector2.zero;
        dir = Vector2.zero;
        point.rectTransform.position = originalPoint;
    }


}

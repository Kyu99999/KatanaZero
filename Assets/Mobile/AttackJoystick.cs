using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AttackJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public PlayerController playerController;
    public PlayerAttackEffect attackEffect;
    public Image point;
    private Vector2 originalPoint = Vector2.zero;

    public float radius;
    private RectTransform rectTr;

    public Vector3 playerDir = Vector3.zero;

    public float degree { get; set; } = 0f;
    public bool isAttack { get; set; } = false;
    private void Start()
    {
        rectTr = GetComponent<RectTransform>();
        originalPoint = rectTr.position;        //피벗 정중앙
    }

    public void OnDrag(PointerEventData eventData)
    {
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
        isAttack = true;
        var dir = eventData.position - originalPoint;
        dir.Normalize();

        var direction = dir - Vector2.right;
        direction.Normalize();
        degree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg * 2;


        //var degree = Mathf.Abs(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg * 2);
      
          //플레이어 오른쪽 축과 마우스 포지션의 각도를 구한다.
        playerDir = eventData.position - originalPoint;
        playerController.JoystickAttack();
        var z = Quaternion.FromToRotation(playerController.transform.right, playerDir);
        attackEffect.JoystickAttack(z);

        point.rectTransform.position = originalPoint;
    }
}

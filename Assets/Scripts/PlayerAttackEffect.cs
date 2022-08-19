using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffect : MonoBehaviour
{
    public PlayerController player;
    public GameObject animator;
    public Collider2D collider;
    public Camera camera;

    private void Awake()
    {
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    //Vector3 v = to - from;
    //return Mathf.Atan2(v.y, v.x)* Mathf.Rad2Deg;

    private void Update()
    {
#if UNITY_ANDROID && UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !animator.activeSelf && !player.isDead)
        {
            Vector3 MousePositon = Input.mousePosition;
            MousePositon = camera.ScreenToWorldPoint(MousePositon);
            MousePositon = new Vector3(MousePositon.x, MousePositon.y, transform.position.z); //���콺 �������� z���� �÷��̾� ��ǥ�� z ���� �����ϰ� �����ؾ��Ѵ�..

            Vector3 v = MousePositon - transform.position;
            var z = Quaternion.FromToRotation(transform.right, v);  //�÷��̾� ������ ��� ���콺 �������� ������ ���Ѵ�.
      
            animator.SetActive(true);
            animator.transform.rotation = z;
        }
#endif
    }   

    public void JoystickAttack(Quaternion degree)
    {
        if (!animator.activeSelf && !player.isDead)
        {
            Debug.Log(degree);
            animator.SetActive(true);
            animator.transform.rotation = degree;
            
        }
    }
}

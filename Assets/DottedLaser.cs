using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DottedLaser : MonoBehaviour
{
    //public GameObject rayObj;

    //public LineRenderer line;

    //private float distance;

    //private void Awake()
    //{
    //    line = GetComponent<LineRenderer>();
    //}

    //private void Update()
    //{
    //    RaycastHit2D[] rayHit1 = Physics2D.RaycastAll(transform.position, transform.right, 30f);
    //    for (var i = 0; i < rayHit1.Length; i++)
    //    {
    //        if (rayHit1[i].collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
    //        {
    //            line.SetPosition(1, rayHit1[i].point);
    //        }
    //    }
    //}

    //public LineRenderer lR;
    //public Renderer rend;

    //private void Start()
    //{
    //    ScaleMaterial();
    //    enabled = true;//scaleInUpdate;
    //}

    //public void ScaleMaterial()
    //{
    //    lR = GetComponent<LineRenderer>();
    //    rend = GetComponent<Renderer>();
    //    rend.material.mainTextureScale =
    //        new Vector2(Vector2.Distance(lR.GetPosition(0), lR.GetPosition(lR.positionCount - 1)) / lR.widthMultiplier,
    //            1);
    //}

    //private void Update()
    //{
    //    rend.material.mainTextureScale =
    //        new Vector2(Vector2.Distance(lR.GetPosition(0), lR.GetPosition(lR.positionCount - 1)) / lR.widthMultiplier,
    //            1);
    //}

}

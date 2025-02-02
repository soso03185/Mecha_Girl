using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ModelRotator : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public Transform model;
    private Vector3 lastMousePosition;


    public void OnBeginDrag(PointerEventData eventData)
    {
        lastMousePosition = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 delta = Input.mousePosition - lastMousePosition;
        model.Rotate(Vector3.up, -delta.x * 0.5f);
        lastMousePosition = Input.mousePosition;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        lastMousePosition = Input.mousePosition;
    //    }
    //    if (Input.GetMouseButton(0))
    //    {
    //        Vector3 delta = Input.mousePosition - lastMousePosition;
    //        model.Rotate(Vector3.up, -delta.x * 0.5f);
    //        lastMousePosition = Input.mousePosition;
    //    }
    //}
}

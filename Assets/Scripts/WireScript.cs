using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class WireScript : MonoBehaviour
{
    private GameObject target;
    private RectTransform targetRect;
    private RectTransform rect;
    private float startWidth;
    private Vector2 sizeDelta;
    private Vector3 position;
    private bool pointerDown = false;
    private bool isInTarget = false;
    public Color color;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        startWidth = rect.rect.width;
        sizeDelta = rect.sizeDelta;
        position = rect.position;
    }

    private void Update()
    {
        if (pointerDown)
        {
            CalcPos(Input.mousePosition);
            target = SolderMinigame.targets[GetClosestTarget(Input.mousePosition)];
            targetRect = target.GetComponent<RectTransform>();

            isInTarget = Vector3.Distance(targetRect.position, Input.mousePosition) < 100f;
        }
    }

    public void OnPointerDown()
    {
        SolderMinigame.currentWire = rect;
        pointerDown = true;
    }

    public void OnPointerUp()
    {
        pointerDown = false;
        if (!isInTarget)
        {
            Revert();
            SolderMinigame.currentWire = null;
        }
        else
        {
            CalcPos(target.GetComponent<RectTransform>().position);
            if (target.GetComponent<WireTargetScript>().CanConnect())
            {
                Debug.Log("Connect");
                target.GetComponent<WireTargetScript>().ConnectToWire(gameObject);
            }
            else
            {
                Revert();
            }
        }
    }

    private void CalcPos(Vector3 pos)
    {
        //point at pos
        Vector3 dir = (pos - rect.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rect.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        //stretch to pos
        float dist = Vector3.Distance(pos, transform.position);
        rect.sizeDelta = new Vector2(dist - startWidth / 2f, rect.sizeDelta.y);
    }

    private int GetClosestTarget(Vector3 pos)
    {
        List<float> dists = new();
        foreach (var target in SolderMinigame.targets)
        {
            dists.Add(Vector3.Distance(pos, target.transform.position));
        }

        float minValue = dists.Min();
        int minIndex = dists.IndexOf(minValue);
        return minIndex;
    }

    public void Revert()
    {
        rect.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        rect.sizeDelta = sizeDelta;
        rect.position = position;

        if ((target != null) && (target.GetComponent<WireTargetScript>().connectedObject == gameObject))
        {
            Debug.Log("Disconnect");
            target.GetComponent<WireTargetScript>().DisconnectFromWire();
        }
    }

    public void _Reset()
    {
        Revert();
    }
}
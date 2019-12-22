using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public enum SeparateType
{
    Center,
    Pivot
}
public class Separate : MonoBehaviour
{
    [Range(0, 5)]
    [SerializeField]
    float SeparateDis = 0;
    public SeparateType separateType = SeparateType.Center;
    Vector3 centerPos;
    Dictionary<Transform, SeparateData> ChildDistance_dic = new Dictionary<Transform, SeparateData>();

    // Start is called before the first frame update
    void Start()
    {
        switch (separateType)
        {
            case SeparateType.Center:
                centerPos = (GetCenter(transform));
                break;
            case SeparateType.Pivot:
                centerPos = transform.position;
                break;
            default:
                break;
        }

        foreach (Transform item in transform)
        {
            SeparateData separateData = new SeparateData();
            separateData.distance = Vector3.Distance(item.position, centerPos);
            separateData.direction = Vector3.Normalize(item.position - centerPos);
            ChildDistance_dic.Add(item, separateData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetSeparateDis(SeparateDis);
    }

    public void SetSeparateDis(float dis)
    {
        SeparateDis = dis;
        foreach (var item in ChildDistance_dic)
        {
            item.Key.position = centerPos + item.Value.direction * (item.Value.distance + SeparateDis);
        }
    }

    public static Vector3 GetCenter(Transform tran)
    {

        Transform parent = tran;

        Vector3 center = Vector3.zero;

        Renderer[] renders = parent.GetComponentsInChildren<Renderer>();

        foreach (Renderer child in renders)
        {
            center += child.bounds.center;
        }

        center /= parent.GetComponentsInChildren<Renderer>().Length;

        Bounds bounds = new Bounds(center, Vector3.zero);

        foreach (Renderer child in renders)
        {
            bounds.Encapsulate(child.bounds);
        }

        return bounds.center;// + parent.position;
    }
    public struct SeparateData
    {
        public Vector3 direction;
        public float distance;
    }
}

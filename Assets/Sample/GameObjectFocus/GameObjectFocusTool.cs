using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GameObjectFocusTool : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_selection;

    protected Vector3 PivotPos;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Focus(m_selection);
        }
    }

    private void Focus(Transform[] transforms)
    {
        // 获取包围盒
        Bounds bounds = TransformUtility.CalculateBounds(transforms, -1, false);
        // 修正溢出
        if (bounds.extents == Vector3.zero)
        {
            bounds.extents = Vector3.one * 0.5f;
        }
        // 包围盒尺寸
        float objSize = Mathf.Max(bounds.extents.y, bounds.extents.x, bounds.extents.z) * 2.0f;
        
        Debug.Log($"bounds.center: {bounds.center}, objSize: {objSize}");
        
        float distance;
        float fov = Camera.main.fieldOfView * Mathf.Deg2Rad;
        distance = Mathf.Abs(objSize / Mathf.Sin(fov / 2.0f));
    
        Debug.Log($"distance: {distance}, fov: {fov}");
        
        // 摄像机方向向量
        var offset = Camera.main.transform.forward* distance;
        // 目标点向量计算
        var target = bounds.center - offset;

        // 添加Center点
        var center = new GameObject("Center");
        center.transform.position = bounds.center;
        center.transform.SetParent(transforms[0]);
        
        var cameraTween = Camera.main.transform.DOMove(target, .25f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public Transform ringSegmentPrefab;

    public void BuildRing(float radius)
    {
        int numSegments = 100;// (int)(radius * 100f); ;
        Vector3 center = Vector3.zero;
        for (int i = 0; i < numSegments; i++)
        {
            Vector3 position = GetPointOnArc(center, 360, radius, i, numSegments);
            Transform ringSegment = Transform.Instantiate(ringSegmentPrefab);
            ringSegment.parent = transform;
            ringSegment.localPosition = position;
        }
    }

    private Vector3 GetPointOnArc(Vector3 center, float arcLength, float radius, int nPoint, int totalPoints)
    {

        if (totalPoints < 2)
        {
            Vector3 position = new Vector3(center.x, center.y, center.z + radius); // HO-R must test
            return position;
        }

        float step = arcLength / (totalPoints /* - 1*/);
        float ang = step * nPoint - (arcLength / 2);// - 90;//(90 - arcLength / 2);// /*90*/50;

        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;// + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }
}

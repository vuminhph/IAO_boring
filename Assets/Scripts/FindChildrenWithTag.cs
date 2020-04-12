using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class FindChildrenWithTag : MonoBehaviour
{
    public List<GameObject> GetChildren(Transform parent, string tag)
    {
        List<GameObject> childrenwithTag = new List<GameObject>();
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == tag)
            {
                childrenwithTag.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                GetChildren(child, tag);
            }
        }
        return childrenwithTag;
    }

    public GameObject GetChildWithName(Transform parent, string name)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.name.Contains(name))
                {
                    return child.gameObject;
                }
            if (child.childCount > 0)
            {
                GameObject searchChild = GetChildWithName(child, name);
                if (searchChild != null) return searchChild;
            }
        }
        return null;
    }
}
using UnityEngine;

public class Util
{
    /// <summary>
    /// 부모 오브젝트에서 이름과 타입을 기준으로 자식 오브젝트를 찾는 유틸리티 함수
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="name">null이면 이름과 상관없이 첫 번째 T를 반환</param>
    /// <param name="recursive"></param>
    /// <returns></returns>
    public static T FindChild<T>(GameObject parent, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (parent == null)
        {
            return null;
        }

        if (recursive)
        {
            foreach (T component in parent.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                {
                    return component;
                }
            }
        }
        else
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Transform child = parent.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || child.name == name)
                {
                    T component = child.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// 부모 오브젝트에서 이름을 기준으로 자식 오브젝트를 찾는 유틸리티 함수
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name">null이면 이름과 상관없이 첫 번째 T를 반환</param>
    /// <param name="recursive"></param>
    /// <returns></returns>
    public static GameObject FindChild(GameObject parent, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(parent, name, recursive);
        if (transform == null)
        {
            return null;
        }

        return transform.gameObject;
    }
}

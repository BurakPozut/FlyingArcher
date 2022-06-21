using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistent<T> : MonoBehaviour where T : Component
{
    private static T MemberInstance;
    public static T Instance
    {
        get 
        {
            if(MemberInstance == null)
            {
                GameObject gObject = new GameObject();
                gObject.name = typeof(T).Name;
                // object.hideFlags = HideFlags.HideAndDontSave;
                MemberInstance = gObject.AddComponent<T>();
            }
            return MemberInstance;
        }
    }
    public virtual void Awake()
    {
        if(MemberInstance == null)
        {
            MemberInstance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this);
    }
    private void OnDestroy()
    {
        if(MemberInstance == this) MemberInstance = null;
    }

}

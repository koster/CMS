using System.Collections.Generic;
using UnityEngine;
#if MACKYSOFT_SUBCLASSSELECTOR
// using MackySoft.SerializeReferenceExtensions;
#endif

public class CMSEntityPfb : MonoBehaviour
{
#if MACKYSOFT_SUBCLASSSELECTOR
    [SerializeReference, SubclassSelector]
#else
    [SerializeReference]
#endif
    public List<EntityComponentDefinition> Components;

    public string GetId()
    {
#if UNITY_EDITOR
        // Get the path of the prefab in the Resources folder
        string path = UnityEditor.AssetDatabase.GetAssetPath(gameObject);
        
        // Remove the "Assets/Resources/" part and ".prefab" to match the Resources.Load format
        if (path.StartsWith("Assets/ldgame/data/Resources/") && path.EndsWith(".prefab"))
        {
            path = path.Substring("Assets/ldgame/data/Resources/".Length);
            path = path.Substring(0, path.Length - ".prefab".Length);
        }

        return path;
#endif
    }
}
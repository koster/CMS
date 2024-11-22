using UnityEditor;

public static class CMSMenuItems
{
    [MenuItem("CMS/Reload")]
    public static void CMSReload()
    {
        CMS.Unload();
        CMS.Init();
    }
}
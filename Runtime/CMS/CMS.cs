using System;
using System.Collections.Generic;
using Common;
using UnityEngine;
using Object = UnityEngine.Object;

public static class CMS
{
    static CMSTable<CMSEntity> all = new CMSTable<CMSEntity>();
    
    static bool isInit;
    
    public static void Init()
    {
        if (isInit)
            return;
        isInit = true;
        
        AutoAdd();
    }
    
    static void AutoAdd()
    {
        var subs = ReflectionUtil.FindAllSubslasses<CMSEntity>();
        foreach (var subclass in subs)
            all.Add(Activator.CreateInstance(subclass) as CMSEntity);

        var resources = Resources.LoadAll<CMSEntityPfb>("CMS");
        foreach (var resEntity in resources)
        {
            Debug.Log("LOAD ENTITY " + resEntity.GetId());
            all.Add(new CMSEntity()
            {
                id = resEntity.GetId(),
                components = resEntity.Components
            });
        }
    }

    public static T Get<T>(string def_id = null) where T : CMSEntity
    {
        if (def_id == null)
            def_id = E.Id<T>();
        var findById = all.FindById(def_id) as T;

        if (findById == null)
        {
            // ok fuck it
            throw new Exception("unable to resolve entity id '" + def_id + "'");
        }

        return findById;
    }


    public static T GetData<T>(string def_id = null) where T : EntityComponentDefinition, new()
    {
        return Get<CMSEntity>(def_id).Get<T>();
    }

    public static List<T> GetAll<T>() where T : CMSEntity
    {
        var allSearch = new List<T>();

        foreach (var a in all.GetAll())
            if (a is T)
                allSearch.Add(a as T);

        return allSearch;
    }

    public static List<(CMSEntity e, T tag)> GetAllData<T>() where T : EntityComponentDefinition, new()
    {
        var allSearch = new List<(CMSEntity, T)>();

        foreach (var a in all.GetAll())
            if (a.Is<T>(out var t))
                allSearch.Add((a, t));

        return allSearch;
    }

    public static void Unload()
    {
        isInit = false;
        all = new CMSTable<CMSEntity>();
    }
}

public class CMSTable<T> where T : CMSEntity, new()
{
    List<T> list = new List<T>();
    Dictionary<string, T> dict = new Dictionary<string, T>();

    public void Add(T inst)
    {
        if (inst.id == null)
            inst.id = E.Id(inst.GetType());
        
        list.Add(inst);
        dict.Add(inst.id, inst);
    }
    
    public T New(string id)
    {
        var t = new T();
        t.id = id;
        list.Add(t);
        dict.Add(id, t);
        return t;
    }

    public List<T> GetAll()
    {
        return list;
    }

    public T FindById(string id)
    {
        return dict.GetValueOrDefault(id);
    }

    public T2 FindByType<T2>() where T2 : T
    {
        foreach(var v in list)
            if (v is T2 v2)
                return v2;
        return null;
    }
}

public partial class CMSEntity
{
    public string id;

    public List<EntityComponentDefinition> components = new List<EntityComponentDefinition>() { new AnythingTag() };

    public T Define<T>() where T : EntityComponentDefinition, new()
    {
        var t = Get<T>();
        if (t != null)
            return t;

        var entity_component = new T();
        components.Add(entity_component);
        return entity_component;
    }

    public bool Is<T>(out T unknown) where T : EntityComponentDefinition, new()
    {
        unknown = Get<T>();
        return unknown != null;
    }

    public bool Is<T>() where T : EntityComponentDefinition, new()
    {
        return Get<T>() != null;
    }

    public bool Is(Type type)
    {
        return components.Find(m => m.GetType() == type) != null;
    }

    public T Get<T>() where T : EntityComponentDefinition, new()
    {
        return components.Find(m => m is T) as T;
    }
}

[Serializable]
public class EntityComponentDefinition
{
}

public static class CMSUtil
{
    public static T Load<T>(this string path) where T : Object
    {
        return Resources.Load<T>(path);
    }
    
    public static Sprite LoadFromSpritesheet(string imageName, string spriteName)
    {
        Sprite[] all = Resources.LoadAll<Sprite>(imageName);
 
        foreach(var s in all)
        {
            if (s.name == spriteName)
            {
                return s;
            }
        }
        return null;
    }
}

public static class E
{
    public static string Id(Type getType)
    {
        return getType.FullName;
    }
    
    public static string Id<T>()
    {
        return ID<T>.Get();
    }
}

static class ID<T>
{
    static string cache;
    
    public static string Get()
    {
        if (cache == null)
            cache = typeof(T).FullName;
        return cache;
    }
    
    public static string Get<T>()
    {
        return ID<T>.Get();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class LocationAttribute : Attribute
{
    private string path;
    public string Path => path;
    public LocationAttribute(string path)
    {
        this.path = path;
    }

    public GameObject Locate()
    {
        return GameObject.Find(path);
    }

}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class PrefabLocationAttribute : Attribute
{
    private string path;
    public string Path => path;
    public PrefabLocationAttribute(string path)
    {
        this.path = path;
    }
}

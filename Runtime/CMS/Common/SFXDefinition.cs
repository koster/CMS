using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SFXArray : EntityComponentDefinition
{
    public List<AudioClip> files = new List<AudioClip>();
    public float volume = 1f;
}

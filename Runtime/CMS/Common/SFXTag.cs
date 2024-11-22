using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class SFXTag : EntityComponentDefinition
    {
        public string sfx_id;
        public float Cooldown = 0.1f;
        public bool VaryPitch;
    }

    [Serializable]
    public class MusicTag : EntityComponentDefinition
    {
        public AudioClip clip;
    }

    [Serializable]
    public class AmbientTag : EntityComponentDefinition
    {
        public AudioClip clip;
    }
}
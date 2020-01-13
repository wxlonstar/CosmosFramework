﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Cosmos
{
    [CreateAssetMenu(fileName = "NewCreatureAduioObject", menuName = "CosmosFramework/AudioObject/CreatureAduioObject")]
    public class CreatureAduioObject : AudioEventObject
    {
        [SerializeField] AudioClip aduioCilp;
        public override AudioClip AudioClip { get { return aduioCilp; } }
        public override void Reset()
        {
            audioName = "NewCreatureAduio";
            mute = false;
            playOnAwake = false;
            loop = false;
            volume = 1;
            spatialBlend = 0;
            speed = 1;
            aduioCilp = null;
        }
    }
}
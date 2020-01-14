﻿using UnityEngine;
using System.Collections;
using Cosmos.Audio;

namespace Cosmos
{
    public class WorldAudioPlayer : AudioPlayer
    {
        [SerializeField] GameObject audioAttachTarget;
        public GameObject AudioAttachTarget { get { return audioAttachTarget; } set { audioAttachTarget = value; }  }
        public override void PlayAudio()
        {
            audioArgs.AudioEventObject = AudioEventObject;
            Facade.Instance.PlayWorldAudio(AudioAttachTarget, audioArgs);
        }
       public override void StopAudio()
        {
            Facade.Instance.StopWorldAudio(AudioAttachTarget);
        }
       public override void PauseAudio()
        {
            Facade.Instance.PauseWorldAudio(AudioAttachTarget);
        }
      public override void UnpauseAudio()
        {
            Facade.Instance.UnPauseWorldAudio(AudioAttachTarget);
        }
    }
}
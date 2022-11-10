using Gummi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utility
{
    [CreateAssetMenu(menuName = "Audio Data")]
    public class AudioDataSO : ScriptableObject
    {
        public AudioClip Clip;

        [Range(0, 1)]
        public float Volume = 0.5f;

        [MinMaxRange(-3, 3)]
        public RangedFloat Pitch = new(1, 1);

        public void PlayOneShot() => PlayOneShot(this);

        public void PlayOneShot(TransformData transform)
        {
            Transform target = PlayOneShot(this).transform;

            target.SetPositionAndRotation(transform.Position, transform.Rotation);
            target.localScale = transform.Scale;
        }

        public static GameObject PlayOneShot(AudioDataSO data)
        {
            if (!data)
            {
                Debug.LogError("Cannot play oneshot SFX. No audio data was provided.");
                return null;
            }

            GameObject go = new GameObject(data.Clip.name);
            AudioSource src = go.AddComponent<AudioSource>();

            src.Load(data);
            src.Play();
            Destroy(go, data.Clip.length);

            return go;
        }
    }

    public static class AudioSourceExtensions
    {
        public static void Load(this AudioSource source, AudioDataSO data)
        {
            source.clip = data.Clip;
            source.volume = data.Volume;
            source.pitch = data.Pitch.GetRandom();
        }
    }
}

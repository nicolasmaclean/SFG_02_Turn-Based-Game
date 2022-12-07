using UnityEngine;

namespace Game.Audio
{
    [CreateAssetMenu(menuName = "SFX Data")]
    public class SFXData : ScriptableObject
    {
        public AudioClip Clip => _clip;
        public float Volume => _volume;
        
        [SerializeField] AudioClip _clip;
        [SerializeField, Range(0, 1)] float _volume = 0.5f;

        public void Play()
        {
            GameObject go = new GameObject(name);
            AudioSource src = go.AddComponent<AudioSource>();
            LoadInto(src);
            src.Play();
            Destroy(go, Clip.length);
        }

        public void LoadInto(AudioSource src)
        {
            src.clip = Clip;
            src.volume = Volume;
        }
    }
}
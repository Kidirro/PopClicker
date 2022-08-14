using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class SoundManager : Singleton<SoundManager>
{

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Sound
    {

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private string name;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private AudioClip clip;

        /// <summary>
        /// 
        /// </summary>
        public AudioClip Clip
        {
            get { return this.clip; }
            set { this.clip = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Range(0f, 1f), SerializeField]
        private float volume;

        /// <summary>
        /// 
        /// </summary>
        public float Volume
        {
            get { return this.volume; }
            set { this.volume = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Range(0f, 1f), SerializeField]
        private float pitch;

        /// <summary>
        /// 
        /// </summary>
        public float Pitch
        {
            get { return this.pitch; }
            set { this.pitch = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private bool loop;

        /// <summary>
        /// 
        /// </summary>
        public bool Loop
        {
            get { return this.loop; }
            set { this.loop = value; }
        }
    }

    [SerializeField]
    private List<Sound> _sounds;

    [SerializeField]
    private List<AudioSource> _enableSource = new List<AudioSource>();

    /// <summary>
    /// 
    /// </summary>
    private void InitializeSource()
    {
        _enableSource.Add(SoundManager.Instance.gameObject.AddComponent<AudioSource>());
        _enableSource[_enableSource.Count - 1].clip = null;
        _enableSource[_enableSource.Count - 1].volume = 0;
        _enableSource[_enableSource.Count - 1].pitch = 0;
        _enableSource[_enableSource.Count - 1].loop = false;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="Name"></param>
    public void PlayClip(string Name)
    {

        if (_enableSource.Count == 0) InitializeSource();
        foreach (Sound sound in _sounds)
        {
            if (sound.Name.Equals(Name))
            {
                Debug.Log("Started");
                StartCoroutine(IPlayClip(sound));
            }
        }
    }

    private IEnumerator IPlayClip(Sound sound)
    {
        AudioSource audio = _enableSource[0];
        _enableSource.Remove(audio);

        audio.clip = sound.Clip;
        audio.volume = sound.Volume;
        audio.pitch = sound.Pitch;
        audio.loop = sound.Loop;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);

        audio.clip =null;
        audio.volume = 0;
        audio.pitch = 0;
        audio.loop =false;
        _enableSource.Add(audio);
    }

}

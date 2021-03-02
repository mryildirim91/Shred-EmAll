using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    private struct Packs
    {
        public AudioClip clip;
        public GameObject pack;
    }

    [SerializeField] private Packs[] _packs;

    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameEvents.ONCollidedWithDestructibles += PlayAudio;
    }

    private void OnDisable()
    {
        GameEvents.ONCollidedWithDestructibles -= PlayAudio;
    }

    private void PlayAudio()
    {
        for (int i = 0; i < _packs.Length; i++)
        {
            if (_packs[i].pack.activeSelf)
            {
                _source.PlayOneShot(_packs[i].clip);
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class FAFAudio : MonoBehaviour 
{
    public static FAFAudio Instance 
    { 
        get 
        { 
            if(instance == null)
            {
                GameObject gobj = new GameObject("FAFAudio");
                instance = gobj.AddComponent<FAFAudio>();
            }
            return instance; 
        } 
    } 
    static FAFAudio instance = null;

    static AudioSource musicSource = null;
    static AudioSource nextMusicSource = null;

    public void PlayOnce(AudioClip _clip, float _volume)
    {
        if (_clip)
        {
            GameObject gobj = new GameObject(_clip.name);
            AudioSource source = gobj.AddComponent<AudioSource>();
            AutoDestruct autoDestruct = gobj.AddComponent<AutoDestruct>();

            source.clip = _clip;
            source.volume = _volume;
            source.Play();

            autoDestruct.delay = _clip.length;
        }
    }

    public void PlayMusic(AudioClip _clip)
    {
        PlayMusic(_clip, false);
    }
    public void PlayMusic(AudioClip _clip, bool _force)
    {
        //bail if force not desired and already playing then bail
        if (!_force && 
            ( ( musicSource && musicSource.clip == _clip) ) || //check current
            ( nextMusicSource && nextMusicSource.clip == _clip) )//check next (transition)
            return;

        if (nextMusicSource)
            Destroy(nextMusicSource);

        GameObject gobj = new GameObject("NextMusic");
        DontDestroyOnLoad(gobj);
        nextMusicSource = gobj.AddComponent<AudioSource>();
        nextMusicSource.clip = _clip;
        nextMusicSource.loop = true;
        nextMusicSource.Play();
        nextMusicSource.volume = 0;

    }

	void Awake () 
    {
        //there can only be one!
        if (instance)
            Destroy(this);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (nextMusicSource != null)
        {
            if(musicSource && musicSource.volume > 0)
            {
                musicSource.volume -= 1f * Time.deltaTime;
            }
            else if(nextMusicSource.volume < 0.8f)
            {
                nextMusicSource.volume += 1f * Time.deltaTime;
            }
            else
            {
                if(musicSource)
                    Destroy(musicSource);

                musicSource = nextMusicSource;
                musicSource.name = "CurrentMusic";
                nextMusicSource = null;
            }
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //인스펙터 창에 커스텀 클래스를 강제로 띄워준다. (직렬화?)
public class Sound
{
    public string name; //사운드 이름

    public AudioClip clip; //사운드 파일
    private AudioSource source; //사운드 플레이어

    public float Volumn;
    public bool loop;

    public void SetSource(AudioSource _source) //소스를 넘겨주는 함수
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.volume = Volumn;
    }

    public void SetVolumn()
    {
        source.volume = Volumn;
    }

    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void SetLoop()
    {
        source.loop = true;
    }

    public void SetLoopCancel()
    {
        source.loop = false;
    }
}

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public Sound[] sounds;
    // Start is called before the first frame update

    static public AudioManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObject = new GameObject("사운드 파일 이름 : " + i + " = " + sounds[i].name);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);
        }
        Play(sounds[0].name);
    }

    public void Play(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Play();
                return;
            }
        }
    }

    public void StopAll()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Stop();
            return;
        }
    }

    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public void SetLoop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoop();
                return;
            }
        }
    }

    public void SetLoopCancel(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoopCancel();
                return;
            }
        }
    }

    public void SetVolumn(float _Volumn)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Volumn = _Volumn;
            sounds[i].SetVolumn();
            return;
        }
    }

    public void SetMusicOnOff(bool _OnOff)
    {
        if (_OnOff)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].Stop();
                return;
            }
        }
        else
        {
            Play("TestTownBGM");
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuildMaster.Windows;

[System.Serializable] //인스펙터 창에 커스텀 클래스를 강제로 띄워준다. (직렬화?)
public class BGM
{
    public string name; //사운드 이름

    public AudioClip clip; //사운드 파일
    private AudioSource source; //사운드 플레이어
    public float Volumn;
    public bool loop;
    public bool OnOff = true;

    public void SetSource(AudioSource _source, float _Volumn) //소스를 넘겨주는 함수
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.volume = _Volumn;
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

    public void On()
    {
        OnOff = true;
    }

    public void Off()
    {
        OnOff = false;
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
[System.Serializable]
public class SoundEffects
{
    public string name; //사운드 이름

    public AudioClip clip; //사운드 파일
    private AudioSource source; //사운드 플레이어
    public float Volumn;
    public bool OnOff = true;

    public void SetSource(AudioSource _source, float _Volumn) //소스를 넘겨주는 함수
    {
        source = _source;
        source.clip = clip;
        source.volume = _Volumn;
    }

    public void SetVolumn()
    {
        source.volume = Volumn;
    }

    public void On()
    {
        OnOff = true;
    }

    public void Off()
    {
        OnOff = false;
    }


    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}


public class AudioManager : MonoBehaviour
{

    [SerializeField]

    public BGM[] bgms;
    public SoundEffects[] SoundEffects;

    private void Start()
    {
        for (int i = 0; i < bgms.Length; i++)
        {
            GameObject BGMObjects = new GameObject("BGM: " + i + " = " + bgms[i].name);
            bgms[i].SetSource(BGMObjects.AddComponent<AudioSource>(), 1);
            BGMObjects.transform.SetParent(this.transform);
        }
        for (int i = 0; i < bgms.Length; i++)
        {
            GameObject SEObjects = new GameObject("효과음: " + i + " = " + SoundEffects[i].name);
            SoundEffects[i].SetSource(SEObjects.AddComponent<AudioSource>(), 1);
            SEObjects.transform.SetParent(this.transform);
        }
        PlayBGM(0);
        SetMasterVolumeOnOff(true);
    }

    public void PlayBGMwasPlaying()
    {
        PlayBGM(SettingVariables.PlayingBGMindex);
    }

    public void PlayBGM(int _BGMindex)
    {
        if (bgms[_BGMindex].OnOff && SettingVariables.MasterOnOff && bgms[_BGMindex].Volumn > 0)
        {
            bgms[_BGMindex].Play();
            SettingVariables.PlayingBGMindex = _BGMindex;
            return;
        }
    }

    public void PlaySoundEffect(int _SoundEffectindex)
    {

        if (SoundEffects[_SoundEffectindex].OnOff && SettingVariables.MasterOnOff && SoundEffects[_SoundEffectindex].Volumn > 0)
        {
            SoundEffects[_SoundEffectindex].Play();
            return;
        }
    }

    public void SetBGMLoop(string _name)
    {
        for (int i = 0; i < bgms.Length; i++)
        {
            if (_name == bgms[i].name)
            {
                bgms[i].SetLoop();
                return;
            }
        }
    }

    public void SetBGMLoopCancel(string _name)
    {
        for (int i = 0; i < bgms.Length; i++)
        {
            if (_name == bgms[i].name)
            {
                bgms[i].SetLoopCancel();
                return;
            }
        }
    }

    public void SetMasterVolume(float _Volumn)
    {
        SettingVariables.MasterVolume = _Volumn;
        for (int i = 0; i < bgms.Length; i++)
        {
            bgms[i].Volumn = SettingVariables.BGMVolume * SettingVariables.MasterVolume;
            bgms[i].SetVolumn();
        }
        for (int i = 0; i < SoundEffects.Length; i++)
        {
            bgms[i].Volumn = SettingVariables.BGMVolume * SettingVariables.MasterVolume;
            bgms[i].SetVolumn();
        }
        return;
    }

    public void SetBGMVolumn(float _Volumn)
    {
        SettingVariables.BGMVolume = _Volumn;
        for (int i = 0; i < bgms.Length; i++)
        {
            bgms[i].Volumn = SettingVariables.BGMVolume * SettingVariables.MasterVolume;
            bgms[i].SetVolumn();
        }
        return;
    }

    public void SetSEVolumn(float _Volumn)
    {
        SettingVariables.SoundEffectVolume = _Volumn;
        for (int i = 0; i < SoundEffects.Length; i++)
        {
            bgms[i].Volumn = SettingVariables.SoundEffectVolume * SettingVariables.MasterVolume;
            bgms[i].SetVolumn();
        }
        return;
    }

    public void SetBGMOnOff(bool _OnOff)
    {
        if (_OnOff)
        {
            for (int i = 0; i < bgms.Length; i++)
            {
                bgms[i].Stop();
                bgms[i].Off();
            }
        }
        else
        {
            for (int i = 0; i < bgms.Length; i++)
            {
                bgms[i].On();
            }
            PlayBGMwasPlaying();
        }
        return;
    }

    public void SetSoundEffectOnOff(bool _OnOff)
    {
        if (_OnOff)
        {
            for (int i = 0; i < SoundEffects.Length; i++)
            {
                SoundEffects[i].Stop();
                SoundEffects[i].Off();
            }
        }
        else
        {
            for (int i = 0; i < SoundEffects.Length; i++)
            {
                SoundEffects[i].On();
            }
        }
        return;
    }

    public void SetMasterVolumeOnOff(bool _OnOff)
    {
        if (_OnOff)
        {
            SettingVariables.MasterOnOff = false;
            for (int i = 0; i < bgms.Length; i++)
            {
                bgms[i].Stop();
            }
        }
        else
        {
            SettingVariables.MasterOnOff = true;
        }
        PlayBGMwasPlaying();
        return;
    }

}

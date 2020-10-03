using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster.Dialogs
{
    public class FadeIn : MonoBehaviour {
        public UnityEngine.UI.Image fadeImage;
        public float FadeTime = 1f; // Fade효과 재생시간
        float time = 0f;
    
        bool isPlaying = false;

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update () {
            time += Time.deltaTime;
            if (FadeTime >= 0.0f && time >= 0.1f)
            {
                FadeTime -= 0.1f;
                fadeImage.color = new Color(0, 0, 0, FadeTime);
                time = 0;
            } else if (FadeTime <= 0.0f)
            {
                time = 0;
            }
        }
    }
}


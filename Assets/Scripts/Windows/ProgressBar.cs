using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public class ProgressBar: MonoBehaviour
    {
        public Image barImage;
        public Text progressText;
        
        public void SetProgress(float progress)
        {
            barImage.fillAmount = progress;
            progressText.text = $"{(int) (progress * 100)}/100";
        }
    }
}
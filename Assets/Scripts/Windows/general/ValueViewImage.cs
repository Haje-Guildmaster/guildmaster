using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    [RequireComponent(typeof(Image))]
    public class ValueViewImage : MonoBehaviour, IValueView<Sprite>
    {
        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void SetValue(Sprite obj)
        {
            _image.sprite = obj;
        }

        private Image _image;
    }
}
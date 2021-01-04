using GuildMaster.Databases;
using GuildMaster.Items;
using UnityEngine;
using UnityEngine.UI;

public class DraggingItemIcon : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private Text _itemNumberLabel;

    DraggingItemIcon(Item _item, int _number, int _index)
    {
        UpdateAppearance(_item, _number, _index);
    }

    public void UpdateAppearance(Item _item, int _number, int _index)
    {
        if (_item == null || _number == 0)
        {
            this._index = _index;
            _itemImage.sprite = (Sprite)null;
            _itemNumberLabel.text = "";
            return;
        }
        this._item = _item;
        this._number = _number;
        this._index = _index;
        _itemImage.sprite = ItemDatabase.Get(_item.Code).ItemImage;
        _itemNumberLabel.text = _number.ToString();
        return;
    }
    private Item _item;
    private int _index, _number;
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GuildMaster.Tools
{
    public interface IScrollPickerElement
    {
        Transform Transform { get; }
        Button Button { get; }
        float Alpha { get; set; }
        
    }
    /// <summary>
    /// 직접 만든 간단한 선택기 유니티 오브젝트. child는 모두 IScrollPickerElement인 컴포넌트가 있어야 합니다.
    /// 자식의 <c>localPosition</c>을 직접적으로 조작하므로 기존 위치는 무시됩니다.
    /// </summary>
    // Todo: Picked 이벤트 대신 async로 수정.
    public class ScrollPicker : MonoBehaviour
    {
        [SerializeField] private float _yDiff;
        [SerializeField] private float _alphaPerIndexDiff;
        [SerializeField] private float _moveSpeedCoefficient = 5;
        [SerializeField] private float _moveSpeedConstant = 3;

        public int SelectedIndex { get; private set;}
        
        public event Action<int> Picked;            // 선택된 항목을 다시 한번 클릭했을 때.
        public event Action<int> SelectingChange;        // 선택된 항목이 바뀌었을 때.

        private void Start()
        {
            _ResetChildList();
        }

        private void OnTransformChildrenChanged()
        {
            _ResetChildList();
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        private void Update()
        {
            if (Math.Abs(_currentCenterIndex - SelectedIndex) < 0.001f)
                return;
            
            GoToward(ref _currentCenterIndex, SelectedIndex,
                (Math.Abs(_currentCenterIndex - SelectedIndex) * _moveSpeedCoefficient + _moveSpeedConstant) *
                Time.deltaTime);
            
            UpdateChildPosition();
        }

        public void SetSelectedIndex(int index, bool anim=true)
        {
            SelectedIndex = index;
            if (!anim)
                _currentCenterIndex = index;
            UpdateChildPosition();
            SelectingChange?.Invoke(index);
        }

        private void UpdateChildPosition()
        {
            foreach (var (element, i) in _elementList.Select((tup, i) => (tup.element, i)))
            {
                var btnTrans = element.Transform;
                Assert.IsTrue(i == btnTrans.GetSiblingIndex());
                btnTrans.localPosition = new Vector3(0f, (_currentCenterIndex - i) * _yDiff);
                
                element.Alpha = Math.Max(0f, 1 - Math.Abs(i - _currentCenterIndex) * _alphaPerIndexDiff);
            }
        }

        private void GoToward(ref float original, float target, float amount)
        {
            var diff = original - target;
            if (Math.Abs(diff) < amount)
                original = target;
            else if (diff > 0)
                original -= amount;
            else
                original += amount;
        }

        private void _ResetChildList()
        {
            Cleanup();
            var childExist = false;
            foreach (Transform child in transform)
            {
                childExist = true;
                var ind = child.transform.GetSiblingIndex();

                var btn = child.GetComponent<IScrollPickerElement>();
                if (btn == null)
                    throw new Exception($"Direct child of {nameof(ScrollPicker)} must have a component that overrides {nameof(IScrollPickerElement)} component");
                
                void OnClick()
                {
                    if (btn.Alpha < 0.1f) return;     // 안보이는 건 클릭 안됨.
                        if (ind == SelectedIndex && (ind - _currentCenterIndex) < 0.5f)
                        Picked?.Invoke(ind);
                    SetSelectedIndex(ind);
                }

                btn.Button.onClick.AddListener(OnClick);
                _elementList.Add((btn, OnClick));
            }

            if (childExist)
                SetSelectedIndex(0, false);
            UpdateChildPosition();
        }

        private void Cleanup()
        {
            foreach (var (child, onClick) in _elementList)
            {
                if (child.Button == null) continue;
                child.Button.onClick.RemoveListener(onClick);
            }

            _elementList.Clear();
        }

        private float _currentCenterIndex = 0f;
        private readonly List<(IScrollPickerElement element, UnityAction onClick)> _elementList = new List<(IScrollPickerElement, UnityAction)>();
    }
}
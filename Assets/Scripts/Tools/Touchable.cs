using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Tools
{
#if UNITY_EDITOR
    using UnityEditor;
    [CustomEditor(typeof(Touchable))]
    public class Touchable_Editor : Editor
    { public override void OnInspectorGUI(){} }
#endif
    [RequireComponent(typeof(CanvasRenderer))]
    public class Touchable:Text
    { protected override void Awake() { base.Awake();} }

}
using System;
using UnityEditor;
using UnityEngine;


namespace GuildMaster.TownRoam
{
    public class TownEditorWindow : EditorWindow
    {
        [MenuItem("Window/Town Editor")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(TownEditorWindow));
        }

        public Town editingTown;

        private void OnGUI()
        {
            Town newTown;
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Town Editor", EditorStyles.boldLabel);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Editing: ", GUILayout.ExpandWidth(false));
                    newTown =
                        EditorGUILayout.ObjectField("", editingTown, typeof(Town), false,
                            GUILayout.ExpandWidth(false)) as Town;
                }
                GUILayout.EndHorizontal();

                if (editingTown != null) {
                    GUILayout.BeginHorizontal("box", GUILayout.ExpandHeight(true));
                    {
                        GUILayout.BeginScrollView(new Vector2(0,0), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        { 
                            DrawTownOverview();
                        }
                        GUILayout.EndScrollView();

                        GUILayout.BeginVertical("box", GUILayout.Width(150), GUILayout.ExpandHeight(true));
                        {
                            GUILayout.Label("asdf", GUILayout.ExpandWidth(true));
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();


            if (newTown != editingTown)
                LoadTown(newTown);
        }



        private void DrawTownOverview()
        {
            var currentEvent = Event.current;
            
            
            // GUI.Box(new Rect(300, 300, 50, 50), "hello!");
        }
        private void LoadTown(Town town)
        {
            EditorGUILayout.HelpBox("메시지 박스.", MessageType.Info);  // ???? 안되는데???
            editingTown = town;
        }
    }
}
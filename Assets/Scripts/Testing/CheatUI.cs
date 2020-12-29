using System.Collections.Generic;
using System.Linq;
using GuildMaster.Data;
using GuildMaster.Exploration;
using GuildMaster.TownRoam.TownLoad;
using GuildMaster.TownRoam.Towns;
using UnityEngine;

namespace GuildMaster.Testing
{
    /// <summary>
    /// 게임 테스트를 위해, 게임에 여러 조작을 할 수 있는 UI입니다.
    /// </summary>
    public class CheatUI : MonoBehaviour
    {
        private void OnGUI()
        {
            using (new GUILayout.VerticalScope())
            {
                // ReSharper disable AssignmentInConditionalExpression
                if (_townSceneToggleValue = GUILayout.Toggle(_townSceneToggleValue, "Town Scene"))
                    using (new Indent(20f))
                    {
                        if (GUILayout.Button("Start Town Scene"))
                            TownLoadManager.LoadTownScene(TownRefs.TestTown);
                    }


                if (_explorationToggleValue = GUILayout.Toggle(_explorationToggleValue, "Exploration"))
                    using (new Indent(20f))
                    {
                        // 캐릭터 고르기

                        var members = Player.Instance.PlayerGuild._guildMembers.GuildMemberList;
                        while (_explorationSelectedCharacter.Count < members.Count)
                            _explorationSelectedCharacter.Add(false);

                        using (new Indent(20f))
                        {
                            for (var i = 0; i < _explorationSelectedCharacter.Count; i++)
                                _explorationSelectedCharacter[i] =
                                    GUILayout.Toggle(_explorationSelectedCharacter[i], members[i].UsingName);
                        }

                        if (GUILayout.Button("Start Exploration"))
                            ExplorationLoader.Load(
                                members.Where((c, i) => _explorationSelectedCharacter[i]).ToList());
                    }
            }
        }

        private class Indent : GUI.Scope
        {
            public Indent(float pixels)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(pixels); // horizontal indent
                GUILayout.BeginVertical();
            }

            protected override void CloseScope()
            {
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }

        private bool _townSceneToggleValue = true;
        private bool _explorationToggleValue = true;
        private readonly List<bool> _explorationSelectedCharacter = new List<bool>();
    }
}
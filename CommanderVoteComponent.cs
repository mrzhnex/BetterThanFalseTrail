using EXILED.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace BetterThanFalseTrail
{
    internal class CommanderVoteComponent : MonoBehaviour
    {
        private float Timer = 0.0f;
        private readonly float TimeIsUp = 1.0f;
        private float TimerToEnd = 0.0f;
        public Dictionary<int, int> PlayersVotes = new Dictionary<int, int>();
        
        public List<int> PlayersAlreadyVotes = new List<int>();
        ReferenceHub Commander;

        private bool CommanderChosen = false;

        public void Start()
        {
            foreach (ReferenceHub referenceHub in Player.GetHubs())
            {
                if (PlayersVotes.ContainsKey(referenceHub.GetPlayerId()))
                {
                    referenceHub.ClearBroadcasts();
                    referenceHub.Broadcast(10, "Выберите командира МОГ", true);
                    referenceHub.SendConsoleMessage(GetVoteMessage(), "yellow");
                }
            }
        }

        private string GetVoteMessage()
        {
            string result = "Список игроков, доступных для голосования:";
            foreach (KeyValuePair<int, int> kvp in PlayersVotes)
            {
                result = result + "\n" + kvp.Key + ") " + (Player.GetPlayer(kvp.Key) != null ? Player.GetPlayer(kvp.Key).nicknameSync.Network_myNickSync : "Игрок не найден");
            }
            return result;
        }

        public void Update()
        {
            Timer += Time.deltaTime;

            if (Timer > TimeIsUp)
            {
                Timer = 0.0f;
                if (CommanderChosen)
                {
                    Destroy(this);
                }
                else
                {
                    TimerToEnd += TimeIsUp;

                    if (TimerToEnd > Global.TimeToVoteCommander)
                    {
                        int playerId = 0;
                        int votes = 0;
                        foreach (KeyValuePair<int, int> kvp in PlayersVotes)
                        {
                            if (votes < kvp.Value)
                            {
                                playerId = kvp.Key;
                            }
                        }
                        Commander = Player.GetPlayer(playerId);
                        if (Commander != null)
                        {
                            CommanderChosen = true;
                            Global.WhitelistCommander.Add(Commander.GetUserId().Replace("@steam", string.Empty));
                            Commander.SetRole(RoleType.NtfCommander);
                            Commander.ClearBroadcasts();
                            Commander.Broadcast(10, "Вы были выбраны командиром МОГ", true);
                        }
                        Destroy(this);
                    }
                }
            }
        }

        public void OnDestroy()
        {
            Global.PlayersVotes = new Dictionary<int, int>();
            if (Global.WhitelistCommander.Contains(Commander.GetUserId().Replace("@steam", string.Empty)))
            {
                Global.WhitelistCommander.Remove(Commander.GetUserId().Replace("@steam", string.Empty));
            }
        }
    }
}
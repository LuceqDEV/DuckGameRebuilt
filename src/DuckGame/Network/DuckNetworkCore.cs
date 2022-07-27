﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckNetworkCore
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DuckGame
{
    public class DuckNetworkCore
    {
        private static int kWinsStep = 5;
        private static int kWinsMin = 5;
        public List<MatchSetting> matchSettings = new List<MatchSetting>()
    {
      new MatchSetting()
      {
        id = "requiredwins",
        name = "Required Wins",
        value = (object) 10,
        min = DuckNetworkCore.kWinsMin,
        max = 100,
        step = DuckNetworkCore.kWinsStep,
        stepMap = new Dictionary<int, int>()
        {
          {
            50,
            DuckNetworkCore.kWinsStep
          },
          {
            100,
            10
          },
          {
            500,
            50
          },
          {
            1000,
            100
          }
        }
      },
      new MatchSetting()
      {
        id = "restsevery",
        name = "Rests Every",
        value = (object) 10,
        min = DuckNetworkCore.kWinsMin,
        max = 100,
        step = DuckNetworkCore.kWinsStep,
        stepMap = new Dictionary<int, int>()
        {
          {
            50,
            DuckNetworkCore.kWinsStep
          },
          {
            100,
            10
          },
          {
            500,
            50
          },
          {
            1000,
            100
          }
        }
      },
      new MatchSetting()
      {
        id = "wallmode",
        name = "Wall Mode",
        value = (object) false
      },
      new MatchSetting()
      {
        id = "normalmaps",
        name = "@NORMALICON@|DGBLUE|Normal Levels",
        value = (object) 90,
        suffix = "%",
        min = 0,
        max = 100,
        step = 5,
        percentageLinks = new List<string>()
        {
          "randommaps",
          "custommaps",
          "workshopmaps"
        }
      },
      new MatchSetting()
      {
        id = "randommaps",
        name = "@RANDOMICON@|DGBLUE|Random Levels",
        value = (object) 10,
        suffix = "%",
        min = 0,
        max = 100,
        step = 5,
        percentageLinks = new List<string>()
        {
          "normalmaps",
          "workshopmaps",
          "custommaps"
        }
      },
      new MatchSetting()
      {
        id = "custommaps",
        name = "@CUSTOMICON@|DGBLUE|Custom Levels",
        value = (object) 0,
        suffix = "%",
        min = 0,
        max = 100,
        step = 5,
        percentageLinks = new List<string>()
        {
          "normalmaps",
          "randommaps",
          "workshopmaps"
        }
      },
      new MatchSetting()
      {
        id = "workshopmaps",
        name = "@RAINBOWICON@|DGBLUE|Internet Levels",
        value = (object) 0,
        suffix = "%",
        min = 0,
        max = 100,
        step = 5,
        percentageLinks = new List<string>()
        {
          "normalmaps",
          "custommaps",
          "randommaps"
        }
      },
      new MatchSetting()
      {
        id = "clientlevelsenabled",
        name = "Client Maps",
        value = (object) false
      }
    };
        public List<MatchSetting> onlineSettings = new List<MatchSetting>()
    {
      new MatchSetting()
      {
        id = "maxplayers",
        name = "Max Players",
        value = (object) 4,
        min = 2,
        max = 8,
        step = 1
      },
      new MatchSetting()
      {
        id = "teams",
        name = "Teams",
        value = (object) false,
        filtered = false,
        filterOnly = true
      },
      new MatchSetting()
      {
        id = "customlevelsenabled",
        name = "Custom Levels",
        value = (object) false,
        filtered = false,
        filterOnly = true
      },
      new MatchSetting()
      {
        id = "modifiers",
        name = "Modifiers",
        value = (object) false,
        filtered = true,
        filterOnly = true
      },
      new MatchSetting()
      {
        id = "type",
        name = "Type",
        value = (object) 2,
        min = 0,
        max = 3,
        createOnly = true,
        valueStrings = new List<string>()
        {
          "PRIVATE",
          "FRIENDS",
          "PUBLIC",
          "LAN"
        }
      },
      new MatchSetting()
      {
        id = "name",
        name = "Name",
        value = (object) "",
        filtered = false,
        filterOnly = false,
        createOnly = true
      },
      new MatchSetting()
      {
        id = "password",
        name = "Password",
        value = (object) "",
        filtered = false,
        filterOnly = false,
        createOnly = true
      },
      new MatchSetting()
      {
        id = "port",
        name = "Port",
        value = (object) "1337",
        filtered = false,
        filterOnly = false,
        condition = (Func<bool>) (() => (int) TeamSelect2.GetOnlineSetting("type").value == 3)
      },
      new MatchSetting()
      {
        id = "dedicated",
        name = "Dedicated",
        value = (object) false,
        filtered = false,
        filterOnly = false,
        createOnly = true
      }
    };
        public Dictionary<string, XPPair> _xpEarned = new Dictionary<string, XPPair>();
        public int levelTransferSize;
        public int levelTransferProgress;
        public int logTransferSize;
        public int logTransferProgress;
        public bool isDedicatedServer;
        public string serverPassword = "";
        public UIMenu xpMenu;
        public UIComponent ducknetUIGroup;
        public DuckNetwork.LobbyType lobbyType;
        public bool speedrunMode;
        public int speedrunMaxTrophy;
        public List<Profile> profiles = new List<Profile>();
        public List<Profile> profilesFixedOrder = new List<Profile>();
        public Profile localProfile;
        public Profile hostProfile;
        public List<NetMessage> pending = new List<NetMessage>();
        public MemoryStream compressedLevelData;
        public bool enteringText;
        public string currentEnterText = "";
        public int cursorFlash;
        public ushort chatIndex;
        public ushort levelTransferSession;
        public NetworkConnection localConnection = new NetworkConnection((object)null);
        public DuckNetStatus status;
        public FancyBitmapFont _builtInChatFont;
        public FancyBitmapFont _rasterChatFont;
        public bool initialized;
        public int randomID;
        public bool inGame;
        public bool stopEnteringText;
        public List<ChatMessage> chatMessages = new List<ChatMessage>();
        private int swearCharOffset;
        private string[] swearChars = new string[7]
        {
      "%",
      "+",
      "{",
      "}",
      "$",
      "!",
      "Z"
        };
        private string[] swearChars2 = new string[7]
        {
      "%",
      "+",
      "#",
      "$",
      "~",
      "!",
      "Z"
        };
        private int rainbowIndex;
        private string[] swearColors = new string[7]
        {
      "|RBOW_1|",
      "|RBOW_2|",
      "|RBOW_3|",
      "|RBOW_4|",
      "|RBOW_5|",
      "|RBOW_6|",
      "|RBOW_7|"
        };
        public static string filteredSpeech;
        public UIMenu _ducknetMenu;
        public UIMenu _optionsMenu;
        public UIMenu _confirmMenu;
        public UIMenu _confirmBlacklistMenu;
        public UIMenu _confirmBlock;
        public UIMenu _confirmReturnToLobby;
        public UIMenu _confirmKick;
        public UIMenu _confirmBan;
        public UIMenu _confirmEditSlots;
        public UIMenu _confirmMatchSettings;
        public MenuBoolean _quit = new MenuBoolean();
        public MenuBoolean _menuClosed = new MenuBoolean();
        public MenuBoolean _returnToLobby = new MenuBoolean();
        public UIMenu _levelSelectMenu;
        public Profile _menuOpenProfile;
        public Profile kickContext;
        public List<ulong> _invitedFriends = new List<ulong>();
        public MenuBoolean _inviteFriends = new MenuBoolean();
        public UIMenu _inviteMenu;
        public UIMenu _slotEditor;
        public UIMenu _matchSettingMenu;
        public UIMenu _matchModifierMenu;
        public UIComponent _noModsUIGroup;
        public UIMenu _noModsMenu;
        public UIComponent _restartModsUIGroup;
        public UIMenu _restartModsMenu;
        public UIComponent _resUIGroup;
        public UIMenu _resMenu;
        public bool _pauseOpen;
        public string _settingsBeforeOpen = "";
        public bool _willOpenSettingsInfo;
        public int _willOpenSpectatorInfo;
        public bool startCountdown;
        public List<string> _activatedLevels = new List<string>();

        public void RecreateProfiles()
        {
            this.profiles.Clear();
            this.profilesFixedOrder.Clear();
            int num;
            for (int index = 0; index < DG.MaxPlayers; ++index)
            {
                num = index + 1;
                Profile profile = new Profile("Netduck" + num.ToString(), InputProfile.GetVirtualInput(index), varDefaultPersona: Persona.all.ElementAt<DuckPersona>(index), network: true);
                profile.SetNetworkIndex((byte)index);
                profile.SetFixedGhostIndex((byte)index);
                if (index > 3)
                    profile.Special_SetSlotType(SlotType.Closed);
                this.profiles.Add(profile);
                this.profilesFixedOrder.Add(profile);
            }
            for (int index = 0; index < DG.MaxSpectators; ++index)
            {
                num = index + 1;
                Profile profile = new Profile("Observer" + num.ToString(), InputProfile.GetVirtualInput(index + DG.MaxPlayers), varDefaultPersona: Persona.all.ElementAt<DuckPersona>(0), network: true);
                profile.SetNetworkIndex((byte)(index + DG.MaxPlayers));
                profile.SetFixedGhostIndex((byte)(index + DG.MaxPlayers));
                profile.slotType = SlotType.Spectator;
                this.profiles.Add(profile);
                this.profilesFixedOrder.Add(profile);
            }
        }

        public DuckNetworkCore()
        {
            this.RecreateProfiles();
            this.randomID = Rando.Int(2147483646);
        }

        public DuckNetworkCore(bool waitInit)
        {
            if (!waitInit)
                this.RecreateProfiles();
            this.randomID = Rando.Int(2147483646);
        }

        public void ReorderFixedList() => this.profilesFixedOrder = this.profiles.OrderBy<Profile, byte>((Func<Profile, byte>)(x => x.fixedGhostIndex)).ToList<Profile>();

        public FancyBitmapFont _chatFont => this._rasterChatFont == null ? this._builtInChatFont : this._rasterChatFont;

        public string FilterText(string pText, User pUser)
        {
            if (Options.Data.languageFilter)
            {
                DuckNetworkCore.filteredSpeech = "";
                pText = pText.Replace("*", "@_sr_@");
                pText = Steam.FilterText(pText, pUser);
                this.swearCharOffset = 0;
                bool flag = false;
                string str1 = "";
                for (int index = 0; index < pText.Length; ++index)
                {
                    if (pText[index] == '*')
                    {
                        if (!flag)
                        {
                            flag = true;
                            DuckNetworkCore.filteredSpeech += "quack";
                        }
                        string str2 = str1 + this.swearColors[this.rainbowIndex];
                        this.rainbowIndex = (this.rainbowIndex + 1) % this.swearColors.Length;
                        if (this._rasterChatFont == null)
                        {
                            str1 = str2 + this.swearChars[Rando.Int(this.swearChars.Length - 1)] + "|PREV|";
                            this.swearCharOffset = (this.swearCharOffset + 1) % this.swearChars2.Length;
                        }
                        else
                        {
                            str1 = str2 + this.swearChars2[Rando.Int(this.swearChars2.Length - 1)] + "|PREV|";
                            this.swearCharOffset = (this.swearCharOffset + 1) % this.swearChars2.Length;
                        }
                    }
                    else
                    {
                        flag = false;
                        this.swearCharOffset = 0;
                        str1 += pText[index].ToString();
                        DuckNetworkCore.filteredSpeech += pText[index].ToString();
                    }
                }
                pText = str1;
                pText = pText.Replace("@_sr_@", "*");
                DuckNetworkCore.filteredSpeech = DuckNetworkCore.filteredSpeech.Replace("@_sr_@", "*");
            }
            else
                DuckNetworkCore.filteredSpeech = pText;
            return pText;
        }

        public void AddChatMessage(ChatMessage pMessage)
        {
            if (pMessage.who == null)
                return;
            ChatMessage chatMessage = (ChatMessage)null;
            if (this.chatMessages.Count > 0)
                chatMessage = this.chatMessages[0];
            pMessage.text = this.FilterText(pMessage.text, (User)null);
            if (Options.Data.textToSpeech)
            {
                if (Options.Data.textToSpeechReadNames)
                    SFX.Say(pMessage.who.nameUI + " says " + DuckNetworkCore.filteredSpeech);
                else
                    SFX.Say(DuckNetworkCore.filteredSpeech);
            }
            float chatScale = DuckNetwork.chatScale;
            this._chatFont.scale = new Vec2(2f * pMessage.scale * chatScale);
            if (this._chatFont is RasterFont)
            {
                FancyBitmapFont chatFont = this._chatFont;
                chatFont.scale = chatFont.scale * 0.5f;
            }
            try
            {
                pMessage.text = this._chatFont.FormatWithNewlines(pMessage.text, 800f);
            }
            catch (Exception ex)
            {
                pMessage.text = "??????";
            }
            int num = pMessage.text.Count<char>((Func<char, bool>)(x => x == '\n'));
            if (chatMessage != null && num == 0 && chatMessage.newlines < 3 && (double)chatMessage.timeout > 2.0 && chatMessage.who == pMessage.who)
            {
                pMessage.text = "|GRAY|" + pMessage.who.nameUI + ": |BLACK|" + pMessage.text;
                chatMessage.timeout = 10f;
                chatMessage.text += "\n";
                chatMessage.text += pMessage.text;
                chatMessage.index = pMessage.index;
                chatMessage.slide = 0.5f;
                ++chatMessage.newlines;
            }
            else
            {
                pMessage.newlines = num + 1;
                pMessage.text = "|WHITE|" + pMessage.who.nameUI + ": |BLACK|" + pMessage.text;
                this.chatMessages.Add(pMessage);
            }
            this.chatMessages = this.chatMessages.OrderBy<ChatMessage, int>((Func<ChatMessage, int>)(x => (int)-x.index)).ToList<ChatMessage>();
        }
    }
}
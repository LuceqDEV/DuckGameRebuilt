﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Keyboard
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Xna.Framework;
namespace DuckGame
{
    public class Keyboard : InputDevice
    {
        static Keyboard()
        {
            TextInputEXT.TextInput += new Action<char>(FNACharEnteredHandler);
            if (Environment.GetEnvironmentVariable("FNADROID") != "1")
                TextInputEXT.StartTextInput();
        }
        private static KeyboardState _keyState;
        private static KeyboardState _keyStatePrev;
        private static bool _keyboardPress = false;
        private static int _lastKeyCount = 0;
        private static int _flipper = 0;
        public static string keyString = "";
        private bool _fakeDisconnect;
        private Dictionary<int, string> _triggerNames;
        private static Dictionary<int, Sprite> _triggerImages;
        private static bool _repeat = false;
        private static List<Keys> _repeatList = new List<Keys>();
        private List<RepeatKey> _repeatingKeys = new List<RepeatKey>();
        public static bool isComposing = false;
        private static int ignoreEnter;
        private static bool ignoreCore = false;
        private static int _usingVoiceRegister;
        private static Thing _registerSetThing;
        private static bool _registerLock = false;
        //private static int _currentNote = 0;

        public static bool NothingPressed() => _keyState.GetPressedKeys().Length == 0 && _keyStatePrev.GetPressedKeys().Length == 0;

        public override bool isConnected => !_fakeDisconnect;

        public Keyboard(string name, int index)
          : base(index)
        {
            _name = "keyboard";
            _productName = name;
            _productGUID = "";
        }

        public override Dictionary<int, string> GetTriggerNames()
        {
            if (_triggerNames == null)
            {
                _triggerNames = new Dictionary<int, string>();
                foreach (Keys key in Enum.GetValues(typeof(Keys)).Cast<Keys>())
                {
                    char ch = KeyToChar(key);
                    if (ch == ' ')
                    {
                        switch (key)
                        {
                            case Keys.Back:
                                _triggerNames[(int)key] = "BACK";
                                continue;
                            case Keys.Tab:
                                _triggerNames[(int)key] = "TAB";
                                continue;
                            case Keys.Enter:
                                _triggerNames[(int)key] = "ENTER";
                                continue;
                            case Keys.Escape:
                                _triggerNames[(int)key] = "ESC";
                                continue;
                            case Keys.Space:
                                _triggerNames[(int)key] = "SPACE";
                                continue;
                            case Keys.PageUp:
                                _triggerNames[(int)key] = "PGUP";
                                continue;
                            case Keys.PageDown:
                                _triggerNames[(int)key] = "PGDN";
                                continue;
                            case Keys.End:
                                _triggerNames[(int)key] = "END";
                                continue;
                            case Keys.Home:
                                _triggerNames[(int)key] = "HOME";
                                continue;
                            case Keys.Left:
                                _triggerNames[(int)key] = Triggers.Left;
                                continue;
                            case Keys.Up:
                                _triggerNames[(int)key] = Triggers.Up;
                                continue;
                            case Keys.Right:
                                _triggerNames[(int)key] = Triggers.Right;
                                continue;
                            case Keys.Down:
                                _triggerNames[(int)key] = Triggers.Down;
                                continue;
                            case Keys.Insert:
                                _triggerNames[(int)key] = "INSRT";
                                continue;
                            case Keys.F1:
                                _triggerNames[(int)key] = "F1";
                                continue;
                            case Keys.F2:
                                _triggerNames[(int)key] = "F2";
                                continue;
                            case Keys.F3:
                                _triggerNames[(int)key] = "F3";
                                continue;
                            case Keys.F4:
                                _triggerNames[(int)key] = "F4";
                                continue;
                            case Keys.F5:
                                _triggerNames[(int)key] = "F5";
                                continue;
                            case Keys.F6:
                                _triggerNames[(int)key] = "F6";
                                continue;
                            case Keys.F7:
                                _triggerNames[(int)key] = "F7";
                                continue;
                            case Keys.F8:
                                _triggerNames[(int)key] = "F8";
                                continue;
                            case Keys.F9:
                                _triggerNames[(int)key] = "F9";
                                continue;
                            case Keys.F10:
                                _triggerNames[(int)key] = "F10";
                                continue;
                            case Keys.F11:
                                _triggerNames[(int)key] = "F11";
                                continue;
                            case Keys.F12:
                                _triggerNames[(int)key] = "F12";
                                continue;
                            case Keys.LeftShift:
                                _triggerNames[(int)key] = "LSHFT";
                                continue;
                            case Keys.RightShift:
                                _triggerNames[(int)key] = "RSHFT";
                                continue;
                            case Keys.LeftControl:
                                _triggerNames[(int)key] = "LCTRL";
                                continue;
                            case Keys.RightControl:
                                _triggerNames[(int)key] = "RCTRL";
                                continue;
                            case Keys.LeftAlt:
                                _triggerNames[(int)key] = "LALT";
                                continue;
                            case Keys.RightAlt:
                                _triggerNames[(int)key] = "RALT";
                                continue;
                            case Keys.MouseLeft:
                                _triggerNames[(int)key] = "MB L";
                                continue;
                            case Keys.MouseMiddle:
                                _triggerNames[(int)key] = "MB M";
                                continue;
                            case Keys.MouseRight:
                                _triggerNames[(int)key] = "MB R";
                                continue;
                            default:
                                continue;
                        }
                    }
                    else
                        _triggerNames[(int)key] = ch.ToString() ?? "";
                }
            }
            return _triggerNames;
        }

        public static void InitTriggerImages()
        {
            if (_triggerImages != null)
                return;
            _triggerImages = new Dictionary<int, Sprite>();
            _triggerImages[9999] = new Sprite("buttons/keyboard/arrows");
            _triggerImages[9998] = new Sprite("buttons/keyboard/wasd");
            _triggerImages[int.MaxValue] = new Sprite("buttons/keyboard/key");
            foreach (Keys key1 in Enum.GetValues(typeof(Keys)).Cast<Keys>())
            {
                char key2 = KeyToChar(key1);
                if (key2 == ' ')
                {
                    switch (key1)
                    {
                        case Keys.Back:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/back");
                            continue;
                        case Keys.Tab:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/tab");
                            continue;
                        case Keys.Enter:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/enter");
                            continue;
                        case Keys.Escape:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/escape");
                            continue;
                        case Keys.Space:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/space");
                            continue;
                        case Keys.PageUp:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/pgup");
                            continue;
                        case Keys.PageDown:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/pgdown");
                            continue;
                        case Keys.End:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/end");
                            continue;
                        case Keys.Home:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/home");
                            continue;
                        case Keys.Left:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/leftKey");
                            continue;
                        case Keys.Up:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/upKey");
                            continue;
                        case Keys.Right:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/rightKey");
                            continue;
                        case Keys.Down:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/downKey");
                            continue;
                        case Keys.Insert:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/insert");
                            continue;
                        case Keys.F1:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/f1");
                            continue;
                        case Keys.F2:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/f2");
                            continue;
                        case Keys.F3:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/f3");
                            continue;
                        case Keys.F4:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/f4");
                            continue;
                        case Keys.F5:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/f5");
                            continue;
                        case Keys.F6:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/f6");
                            continue;
                        case Keys.F7:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/f7");
                            continue;
                        case Keys.F8:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/f8");
                            continue;
                        case Keys.F9:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/f9");
                            continue;
                        case Keys.F10:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/f10");
                            continue;
                        case Keys.F11:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/f11");
                            continue;
                        case Keys.F12:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/f12");
                            continue;
                        case Keys.LeftShift:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/shift");
                            continue;
                        case Keys.RightShift:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/shift");
                            continue;
                        case Keys.LeftControl:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/control");
                            continue;
                        case Keys.RightControl:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/control");
                            continue;
                        case Keys.LeftAlt:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/alt");
                            continue;
                        case Keys.RightAlt:
                            _triggerImages[(int)key1] = new Sprite("buttons/keyboard/alt");
                            continue;
                        case Keys.MouseLeft:
                            _triggerImages[(int)key1] = new SpriteMap("buttons/mouse", 12, 15)
                            {
                                frame = 0
                            };
                            continue;
                        case Keys.MouseMiddle:
                            _triggerImages[(int)key1] = new SpriteMap("buttons/mouse", 12, 15)
                            {
                                frame = 1
                            };
                            continue;
                        case Keys.MouseRight:
                            _triggerImages[(int)key1] = new SpriteMap("buttons/mouse", 12, 15)
                            {
                                frame = 2
                            };
                            continue;
                        default:
                            continue;
                    }
                }
                else
                    _triggerImages[(int)key1] = new KeyImage(key2);
            }
        }

        public override Sprite GetMapImage(int map)
        {
            Sprite sprite;
            _triggerImages.TryGetValue(map, out sprite);
            return sprite ?? _triggerImages[int.MaxValue];
        }

        public static char KeyToChar(Keys key, bool caps = true, bool shift = false)
        {
            if (caps)
            {
                switch (key)
                {
                    case Keys.D0:
                        return '0';
                    case Keys.D1:
                        return '1';
                    case Keys.D2:
                        return '2';
                    case Keys.D3:
                        return '3';
                    case Keys.D4:
                        return '4';
                    case Keys.D5:
                        return '5';
                    case Keys.D6:
                        return '6';
                    case Keys.D7:
                        return '7';
                    case Keys.D8:
                        return '8';
                    case Keys.D9:
                        return '9';
                    case Keys.A:
                        return 'A';
                    case Keys.B:
                        return 'B';
                    case Keys.C:
                        return 'C';
                    case Keys.D:
                        return 'D';
                    case Keys.E:
                        return 'E';
                    case Keys.F:
                        return 'F';
                    case Keys.G:
                        return 'G';
                    case Keys.H:
                        return 'H';
                    case Keys.I:
                        return 'I';
                    case Keys.J:
                        return 'J';
                    case Keys.K:
                        return 'K';
                    case Keys.L:
                        return 'L';
                    case Keys.M:
                        return 'M';
                    case Keys.N:
                        return 'N';
                    case Keys.O:
                        return 'O';
                    case Keys.P:
                        return 'P';
                    case Keys.Q:
                        return 'Q';
                    case Keys.R:
                        return 'R';
                    case Keys.S:
                        return 'S';
                    case Keys.T:
                        return 'T';
                    case Keys.U:
                        return 'U';
                    case Keys.V:
                        return 'V';
                    case Keys.W:
                        return 'W';
                    case Keys.X:
                        return 'X';
                    case Keys.Y:
                        return 'Y';
                    case Keys.Z:
                        return 'Z';
                    case Keys.NumPad0:
                        return '0';
                    case Keys.NumPad1:
                        return '1';
                    case Keys.NumPad2:
                        return '2';
                    case Keys.NumPad3:
                        return '3';
                    case Keys.NumPad4:
                        return '4';
                    case Keys.NumPad5:
                        return '5';
                    case Keys.NumPad6:
                        return '6';
                    case Keys.NumPad7:
                        return '7';
                    case Keys.NumPad8:
                        return '8';
                    case Keys.NumPad9:
                        return '9';
                    case Keys.OemSemicolon:
                        return ';';
                    case Keys.OemPlus:
                        return '=';
                    case Keys.OemComma:
                        return ',';
                    case Keys.OemMinus:
                        return '-';
                    case Keys.OemPeriod:
                        return '.';
                    case Keys.OemQuestion:
                        return '/';
                    case Keys.OemTilde:
                        return '~';
                    case Keys.OemOpenBrackets:
                        return '[';
                    case Keys.OemPipe:
                        return '\\';
                    case Keys.OemCloseBrackets:
                        return ']';
                    case Keys.OemQuotes:
                        return '\'';
                    case Keys.OemBackslash:
                        return '\\';
                }
            }
            else if (shift)
            {
                switch (key)
                {
                    case Keys.D0:
                        return ')';
                    case Keys.D1:
                        return '!';
                    case Keys.D2:
                        return '@';
                    case Keys.D3:
                        return '#';
                    case Keys.D4:
                        return '$';
                    case Keys.D5:
                        return '%';
                    case Keys.D6:
                        return '^';
                    case Keys.D7:
                        return '&';
                    case Keys.D8:
                        return '*';
                    case Keys.D9:
                        return '(';
                    case Keys.A:
                        return 'A';
                    case Keys.B:
                        return 'B';
                    case Keys.C:
                        return 'C';
                    case Keys.D:
                        return 'D';
                    case Keys.E:
                        return 'E';
                    case Keys.F:
                        return 'F';
                    case Keys.G:
                        return 'G';
                    case Keys.H:
                        return 'H';
                    case Keys.I:
                        return 'I';
                    case Keys.J:
                        return 'J';
                    case Keys.K:
                        return 'K';
                    case Keys.L:
                        return 'L';
                    case Keys.M:
                        return 'M';
                    case Keys.N:
                        return 'N';
                    case Keys.O:
                        return 'O';
                    case Keys.P:
                        return 'P';
                    case Keys.Q:
                        return 'Q';
                    case Keys.R:
                        return 'R';
                    case Keys.S:
                        return 'S';
                    case Keys.T:
                        return 'T';
                    case Keys.U:
                        return 'U';
                    case Keys.V:
                        return 'V';
                    case Keys.W:
                        return 'W';
                    case Keys.X:
                        return 'X';
                    case Keys.Y:
                        return 'Y';
                    case Keys.Z:
                        return 'Z';
                    case Keys.NumPad0:
                        return '0';
                    case Keys.NumPad1:
                        return '1';
                    case Keys.NumPad2:
                        return '2';
                    case Keys.NumPad3:
                        return '3';
                    case Keys.NumPad4:
                        return '4';
                    case Keys.NumPad5:
                        return '5';
                    case Keys.NumPad6:
                        return '6';
                    case Keys.NumPad7:
                        return '7';
                    case Keys.NumPad8:
                        return '8';
                    case Keys.NumPad9:
                        return '9';
                    case Keys.OemSemicolon:
                        return ':';
                    case Keys.OemPlus:
                        return '+';
                    case Keys.OemComma:
                        return '<';
                    case Keys.OemMinus:
                        return '_';
                    case Keys.OemPeriod:
                        return '>';
                    case Keys.OemQuestion:
                        return '?';
                    case Keys.OemTilde:
                        return '~';
                    case Keys.OemOpenBrackets:
                        return '{';
                    case Keys.OemPipe:
                        return '|';
                    case Keys.OemCloseBrackets:
                        return '}';
                    case Keys.OemQuotes:
                        return '"';
                    case Keys.OemBackslash:
                        return '|';
                }
            }
            else
            {
                switch (key)
                {
                    case Keys.D0:
                        return '0';
                    case Keys.D1:
                        return '1';
                    case Keys.D2:
                        return '2';
                    case Keys.D3:
                        return '3';
                    case Keys.D4:
                        return '4';
                    case Keys.D5:
                        return '5';
                    case Keys.D6:
                        return '6';
                    case Keys.D7:
                        return '7';
                    case Keys.D8:
                        return '8';
                    case Keys.D9:
                        return '9';
                    case Keys.A:
                        return 'a';
                    case Keys.B:
                        return 'b';
                    case Keys.C:
                        return 'c';
                    case Keys.D:
                        return 'd';
                    case Keys.E:
                        return 'e';
                    case Keys.F:
                        return 'f';
                    case Keys.G:
                        return 'g';
                    case Keys.H:
                        return 'h';
                    case Keys.I:
                        return 'i';
                    case Keys.J:
                        return 'j';
                    case Keys.K:
                        return 'k';
                    case Keys.L:
                        return 'l';
                    case Keys.M:
                        return 'm';
                    case Keys.N:
                        return 'n';
                    case Keys.O:
                        return 'o';
                    case Keys.P:
                        return 'p';
                    case Keys.Q:
                        return 'q';
                    case Keys.R:
                        return 'r';
                    case Keys.S:
                        return 's';
                    case Keys.T:
                        return 't';
                    case Keys.U:
                        return 'u';
                    case Keys.V:
                        return 'v';
                    case Keys.W:
                        return 'w';
                    case Keys.X:
                        return 'x';
                    case Keys.Y:
                        return 'y';
                    case Keys.Z:
                        return 'z';
                    case Keys.NumPad0:
                        return '0';
                    case Keys.NumPad1:
                        return '1';
                    case Keys.NumPad2:
                        return '2';
                    case Keys.NumPad3:
                        return '3';
                    case Keys.NumPad4:
                        return '4';
                    case Keys.NumPad5:
                        return '5';
                    case Keys.NumPad6:
                        return '6';
                    case Keys.NumPad7:
                        return '7';
                    case Keys.NumPad8:
                        return '8';
                    case Keys.NumPad9:
                        return '9';
                    case Keys.OemSemicolon:
                        return ';';
                    case Keys.OemPlus:
                        return '=';
                    case Keys.OemComma:
                        return ',';
                    case Keys.OemMinus:
                        return '-';
                    case Keys.OemPeriod:
                        return '.';
                    case Keys.OemQuestion:
                        return '/';
                    case Keys.OemTilde:
                        return '~';
                    case Keys.OemOpenBrackets:
                        return '[';
                    case Keys.OemPipe:
                        return '\\';
                    case Keys.OemCloseBrackets:
                        return ']';
                    case Keys.OemQuotes:
                        return '\'';
                    case Keys.OemBackslash:
                        return '\\';
                }
            }
            return ' ';
        }

        public static bool repeat
        {
            get => _repeat;
            set => _repeat = value;
        }

        public object KeyInterop { get; private set; }

        public override void Update()
        {
            if (_usingVoiceRegister > 0)
                --_usingVoiceRegister;
            --ignoreEnter;
            if (ignoreEnter < 0)
                ignoreEnter = 0;
            if (!Graphics.inFocus)
                return;
            if (_usingVoiceRegister == 0)
            {
                if (Pressed(Keys.D8) && index == 0)
                    _fakeDisconnect = !_fakeDisconnect;
                if (Pressed(Keys.D9) && index == 1)
                    _fakeDisconnect = !_fakeDisconnect;
            }
            if (_flipper == 0)
            {
                _keyStatePrev = _keyState;
                _keyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                _keyboardPress = false;
                int num = _keyState.GetPressedKeys().Count();
                if (num != _lastKeyCount && num != 0)
                    _keyboardPress = true;
                _lastKeyCount = num;
                updateKeyboardString();
                _flipper = 1;
                if (_registerLock && (_registerSetThing == null || _registerSetThing.removeFromLevel || _registerSetThing.owner == null || DevConsole.open || DuckNetwork.core.enteringText))
                {
                    _registerLock = false;
                    //Keyboard._currentNote = 0;
                }
            }
            else
                --_flipper;
            if (index == 0)
                _repeatList.Clear();
            ignoreCore = true;
            if (_repeat)
            {
                foreach (Keys keys in Enum.GetValues(typeof(Keys)).Cast<Keys>())
                {
                    Keys k = keys;
                    if (MapPressed((int)k, false) && (k < Keys.F1 || k > Keys.F12) && _repeatingKeys.FirstOrDefault(x => x.key == k) == null)
                        _repeatingKeys.Add(new RepeatKey()
                        {
                            key = k,
                            repeatTime = 2f
                        });
                }
                List<RepeatKey> repeatKeyList = new List<RepeatKey>();
                foreach (RepeatKey repeatingKey in _repeatingKeys)
                {
                    repeatingKey.repeatTime -= 0.1f;
                    bool flag = MapDown((int)repeatingKey.key, false);
                    if (flag && repeatingKey.repeatTime < 0.0)
                        _repeatList.Add(repeatingKey.key);
                    if (repeatingKey.repeatTime <= 0.0 & flag)
                        repeatingKey.repeatTime = 0.25f;
                    if (!flag)
                        repeatKeyList.Add(repeatingKey);
                }
                foreach (RepeatKey repeatKey in repeatKeyList)
                    _repeatingKeys.Remove(repeatKey);
            }
            ignoreCore = false;
        }

        [DllImport("user32.dll")]
        public static extern int ToUnicode(
          uint wVirtKey,
          uint wScanCode,
          byte[] lpKeyState,
          [MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pwszBuff,
          int cchBuff,
          uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);
        public static Dictionary<int, char> DicVirtualChar = new Dictionary<int, char>() { { 8, (char)8 },
        { 9, (char)9},
        { 3, (char)13 },
        { 27, (char)27 },
        { 48, '0'},
        { 49, '1'},
        { 50, '2'},
        { 51, '3'},
        { 52, '4'},
        { 53, '5'},
        { 54, '6'},
        { 55, '7'},
        { 56, '8'},
        { 57, '9'},
        { 65, 'a'},
        { 66, 'b'},
        { 67, 'c'},
        { 68, 'd'},
        { 69, 'e'},
        { 70, 'f'},
        { 71, 'g'},
        { 72, 'h'},
        { 73, 'i'},
        { 74, 'j'},
        { 75, 'k'},
        { 76, 'l'},
        { 77, 'm'},
        { 78, 'n'},
        { 79, 'o'},
        { 80, 'p'},
        { 81, 'q'},
        { 82, 'r'},
        { 83, 's'},
        { 84, 't'},
        { 85, 'u'},
        { 86, 'v'},
        { 87, 'w'},
        { 88, 'x'},
        { 89, 'y'},
        { 90, 'z'},
        { 96, '0'},
        { 97, '1'},
        { 98, '2'},
        { 99, '3'},
        { 100, '4'},
        { 101, '5'},
        { 102, '6'},
        { 103, '7'},
        { 104, '8'},
        { 105, '9'},
        { 106, '*'},
        { 107, '+'},
        { 109, '-'},
        { 110, '.'},
        { 111, '/'},
        { 186, ';'},
        { 187, '='},
        { 188, ','},
        { 189, '-'},
        { 190, '.'},
        { 191, '/'},
        { 192, '`'},
        { 219, '['},
        { 220, '\\'},
        { 221, ']'},
        { 222, '\''},
        { 226, '\\'},
        { 999989, '5'},
        { 999990, '6'},
        { 999991, '7'},
        { 999992, '8'}};
        public static char GetCharFromKey(Keys key)
        {
            int num = (int)key;
            if (Program.IsLinuxD || Program.isLinux)
            {
                if (DicVirtualChar.ContainsKey(num))
                {
                    return DicVirtualChar[num];
                }
                return ' ';
            }
            char charFromKey = ' ';
            byte[] lpKeyState = new byte[256];
            GetKeyboardState(lpKeyState);
            uint wScanCode = MapVirtualKey((uint)num, MapType.MAPVK_VK_TO_VSC);
            StringBuilder pwszBuff = new StringBuilder(2);
            int unicode = ToUnicode((uint)num, wScanCode, lpKeyState, pwszBuff, pwszBuff.Capacity, 0U);
            if (pwszBuff.Length < 1)
                return ' ';
            switch (unicode)
            {
                case -1:
                case 0:
                label_7:
                    return charFromKey;
                case 1:
                    charFromKey = pwszBuff[0];
                    switch (charFromKey)
                    {
                        case 'ª':
                            charFromKey = '~';
                            goto label_7;
                        case 'º':
                            charFromKey = '`';
                            goto label_7;
                        default:
                            goto label_7;
                    }
                default:
                    charFromKey = pwszBuff[0];
                    goto case -1;
            }
        }

        public static void IMECharEnteredHandler(object sender, CharacterEventArgs e)
        {
            keyString = e.Character != '　' ? keyString + e.Character.ToString() : keyString + " ";
            ignoreEnter = 4;
        }

        public static void ALTCharEnteredHandler(object sender, CharacterEventArgs e)
        {
            if (!e.ExtendedKey)
                return;
            if (e.Character == '　')
                keyString += " ";
            else
                keyString += e.Character.ToString();
        }
        public static List<char> TextInputCharacters = new List<char>(FNAPlatform.TextInputCharacters);
        public static void FNACharEnteredHandler(char c) // FNA SDL call back for key char pressed sht
        {
            if (TextInputCharacters.Contains(c)) // Not Chars
            {
                if (c == '\b') // Keys.Back
                {
                    if (keyString.Length > 0)
                    {
                        keyString = keyString.Remove(keyString.Length - 1, 1);
                    }
                }
                return;
            }
            keyString += c.ToString();
        }

        private void updateKeyboardString() // old way to get keyboard inputs, idk maby implementing it optionally? idk man
        {
            //Keyboard.ignoreCore = true;
            //int num1 = Keyboard.Down(Keys.LeftShift) ? 1 : (Keyboard.Down(Keys.RightShift) ? 1 : 0);
            //int num2 = Keyboard.Down(Keys.LeftControl) ? 1 : (Keyboard.Down(Keys.RightControl) ? 1 : 0);
            //int num3 = Console.CapsLock ? 1 : 0;
            //Microsoft.Xna.Framework.Input.Keys[] pressedKeys = Keyboard._keyState.GetPressedKeys();
            //if (!Keyboard.isComposing)
            //{
            //    foreach (Microsoft.Xna.Framework.Input.Keys keys in pressedKeys)
            //    {
            //        if (MapPressed((int)keys, false))
            //        {
            //            switch (keys)
            //            {
            //                case Microsoft.Xna.Framework.Input.Keys.Back:
            //                    if (Keyboard.keyString.Length > 0)
            //                    {
            //                        Keyboard.keyString = Keyboard.keyString.Remove(Keyboard.keyString.Length - 1, 1);
            //                        continue;
            //                    }
            //                    continue;
            //                case Microsoft.Xna.Framework.Input.Keys.Enter:
            //                case Microsoft.Xna.Framework.Input.Keys.Escape:
            //                    continue;
            //                case Microsoft.Xna.Framework.Input.Keys.Space:
            //                    Keyboard.keyString = Keyboard.keyString.Insert(Keyboard.keyString.Length, " ");
            //                    continue;
            //                default:
            //                    char charFromKey = Keyboard.GetCharFromKey((Keys)keys);
            //                    if (charFromKey != ' ')
            //                    {
            //                        Keyboard.keyString += charFromKey.ToString();
            //                        continue;
            //                    }
            //                    continue;
            //            }
            //        }
            //    }
            //}
            //Keyboard.ignoreCore = false;
            //Keyboard.isComposing = false;
        }

        private static bool IsKeyNote(Keys pKey) => pKey == Keys.D1 || pKey == Keys.D2 || pKey == Keys.D3 || pKey == Keys.D4 || pKey == Keys.D5 || pKey == Keys.D6 || pKey == Keys.D7 || pKey == Keys.D8 || pKey == Keys.D9 || pKey == Keys.D0 || pKey == Keys.OemPlus || pKey == Keys.OemMinus || pKey == Keys.Back;

        private static int KeyNote()
        {
            _usingVoiceRegister = 0;
            int num = -1;
            if (_registerLock)
            {
                if (Down(Keys.D1))
                    num = 0;
                if (Down(Keys.D2))
                    num = 1;
                if (Down(Keys.D3))
                    num = 2;
                if (Down(Keys.D4))
                    num = 3;
                if (Down(Keys.D5))
                    num = 4;
                if (Down(Keys.D6))
                    num = 5;
                if (Down(Keys.D7))
                    num = 6;
                if (Down(Keys.D8))
                    num = 7;
                if (Down(Keys.D9))
                    num = 8;
                if (Down(Keys.D0))
                    num = 9;
                if (Down(Keys.OemMinus))
                    num = 10;
                if (Down(Keys.OemPlus))
                    num = 11;
                if (Down(Keys.Back))
                    num = 12;
                _usingVoiceRegister = 3;
            }
            return num;
        }

        public static int CurrentNote(InputProfile pProfile, Thing pInstrument)
        {
            _registerSetThing = pInstrument;
            _usingVoiceRegister = 0;
            if (Input.Pressed(Triggers.VoiceRegister))
                _registerLock = !_registerLock;
            return KeyNote();
        }

        public override bool MapPressed(int mapping, bool any = false)
        {
            if (!ignoreCore && (DevConsole.open || DuckNetwork.enteringText || Editor.enteringText))
                return false;
            Keys key = (Keys)mapping;
            return Pressed(key, any) || _repeatList.Contains(key);
        }

        public static bool Pressed(Keys key, bool any = false)
        {
            if (_usingVoiceRegister > 0 && IsKeyNote(key) || Input.ignoreInput)
                return false;
            if (any && _keyboardPress)
                return true;
            if (key == Keys.Enter && ignoreEnter > 0)
                return false;
            if (key >= Keys.MouseKeys)
            {
                if (key == Keys.MouseLeft)
                    return Mouse.left == InputState.Pressed;
                if (key == Keys.MouseMiddle)
                    return Mouse.middle == InputState.Pressed;
                return key == Keys.MouseRight && Mouse.right == InputState.Pressed;
            }
            return _keyState.IsKeyDown((Microsoft.Xna.Framework.Input.Keys)key) && !_keyStatePrev.IsKeyDown((Microsoft.Xna.Framework.Input.Keys)key) || _repeatList.Contains(key);
        }

        public override bool MapReleased(int mapping) => (ignoreCore || !DevConsole.open && !DuckNetwork.enteringText && !Editor.enteringText) && Released((Keys)mapping);

        public static bool Released(Keys key)
        {
            if (_usingVoiceRegister > 0 && IsKeyNote(key) || Input.ignoreInput || key == Keys.Enter && ignoreEnter > 0)
                return false;
            if (key >= Keys.MouseKeys)
            {
                if (key == Keys.MouseLeft)
                    return Mouse.left == InputState.Released;
                if (key == Keys.MouseMiddle)
                    return Mouse.middle == InputState.Released;
                return key == Keys.MouseRight && Mouse.right == InputState.Released;
            }
            return !_keyState.IsKeyDown((Microsoft.Xna.Framework.Input.Keys)key) && _keyStatePrev.IsKeyDown((Microsoft.Xna.Framework.Input.Keys)key);
        }

        public override bool MapDown(int mapping, bool any = false) => (ignoreCore || !DevConsole.open && !DuckNetwork.enteringText && !Editor.enteringText) && Down((Keys)mapping);

        public static bool control => Down(Keys.LeftControl) || Down(Keys.RightControl);

        public static bool alt => Down(Keys.LeftAlt) || Down(Keys.RightAlt);

        public static bool shift => Down(Keys.LeftShift) || Down(Keys.RightShift);

        public static bool Down(Keys key)
        {
            if (_usingVoiceRegister > 0 && IsKeyNote(key) || Input.ignoreInput || key == Keys.Enter && ignoreEnter > 0)
                return false;
            if (key >= Keys.MouseKeys)
            {
                if (key == Keys.MouseLeft)
                    return Mouse.left == InputState.Down || Mouse.left == InputState.Pressed;
                if (key == Keys.MouseMiddle)
                    return Mouse.middle == InputState.Down || Mouse.middle == InputState.Pressed;
                if (key != Keys.MouseRight)
                    return false;
                return Mouse.right == InputState.Down || Mouse.right == InputState.Pressed;
            }
            return _keyState.IsKeyDown((Microsoft.Xna.Framework.Input.Keys)key);
        }

        public class RepeatKey
        {
            public Keys key;
            public float repeatTime;
        }

        public enum MapType : uint
        {
            MAPVK_VK_TO_VSC,
            MAPVK_VSC_TO_VK,
            MAPVK_VK_TO_CHAR,
            MAPVK_VSC_TO_VK_EX,
        }
    }
}

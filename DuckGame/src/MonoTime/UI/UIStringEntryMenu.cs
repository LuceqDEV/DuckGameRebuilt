﻿using System;
using System.Text.RegularExpressions;

namespace DuckGame
{
    public class UIStringEntryMenu : UIMenu
    {
        public string text = "";
        public bool _directional;
        private FieldBinding _binding;
        private int _maxLength;
        private int _minNumber;
        private int _maxNumber;
        private bool _numeric;
        private bool _cancelled = true;
        private string _originalValue = "";
        private float blink;
        private bool wasOpen;

        public UIStringEntryMenu(
          bool directional,
          string title,
          FieldBinding pBinding,
          int pMaxLength = 50,
          bool pNumeric = false,
          int pMinNumber = -2147483648,
          int pMaxNumber = 2147483647)
          : base(title, Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, directional ? 160f : 220f, 60f, directional ? "@WASD@SET @SELECT@ACCEPT" : "@ENTERKEY@ACCEPT @ESCAPEKEY@CANCEL")
        {
            Add(new UIBox(0f, 0f, 100f, 16f, isVisible: false), true);
            _binding = pBinding;
            _directional = directional;
            _numeric = pNumeric;
            _maxLength = pMaxLength;
            _minNumber = pMinNumber;
            _maxNumber = pMaxNumber;
        }

        public void SetValue(string pValue) => text = pValue;

        public override void Open()
        {
            if (_directional)
                text = "";
            _originalValue = text;
            Keyboard.KeyString = text;
            _cancelled = true;
            base.Open();
        }

        public override void OnClose()
        {
            Keyboard.repeat = false;
            if (wasOpen && _cancelled)
                _binding.value = _directional ? "" : (object)_originalValue;
            wasOpen = false;
            base.OnClose();
        }

        public override void Update()
        {
            if (open)
            {
                Input._imeAllowed = true;
                Keyboard.repeat = true;
                wasOpen = true;
                blink += 0.02f;
                if (_directional)
                {
                    if (text.Length < 6)
                    {
                        if (Input.Pressed(Triggers.Left))
                            text += "L";
                        else if (Input.Pressed(Triggers.Right))
                            text += "R";
                        else if (Input.Pressed(Triggers.Up))
                            text += "U";
                        else if (Input.Pressed(Triggers.Down))
                            text += "D";
                    }
                    if (Input.Pressed(Triggers.Select))
                    {
                        _binding.value = text;
                        _cancelled = false;
                        _backFunction.Activate();
                    }
                }
                else
                {
                    globalUILock = true;
                    if (Graphics._biosFont.GetLength(Keyboard.KeyString) > _maxLength)
                        Keyboard.KeyString = Graphics._biosFont.Crop(Keyboard.KeyString, 0, _maxLength);
                    if (_numeric)
                        Keyboard.KeyString = Regex.Replace(Keyboard.KeyString, "[^0-9]", "");
                    InputProfile.ignoreKeyboard = true;
                    text = Keyboard.KeyString;
                    if (Keyboard.Pressed(Keys.Enter))
                    {
                        bool invalid = false;
                        if (_numeric)
                        {
                            try
                            {
                                int num = Convert.ToInt32(Keyboard.KeyString);
                                if (num < _minNumber)
                                {
                                    num = _minNumber;
                                    invalid = true;
                                }
                                else if (num > _maxNumber)
                                {
                                    num = _maxNumber;
                                    invalid = true;
                                }
                                Keyboard.KeyString = num.ToString();
                            }
                            catch (Exception)
                            {
                                Keyboard.KeyString = "";
                                invalid = true;
                            }
                        }
                        if (!invalid)
                        {
                            globalUILock = false;
                            _binding.value = text;
                            _cancelled = false;
                            _backFunction.Activate();
                            if (_acceptFunction != null)
                                _acceptFunction.Activate();
                        }
                    }
                    else if (Keyboard.Pressed(Keys.Escape) || Input.Pressed(Triggers.Cancel))
                    {
                        globalUILock = false;
                        _cancelled = true;
                        _backFunction.Activate();
                    }
                }
                InputProfile.ignoreKeyboard = false;
            }
            base.Update();
        }

        public override void Draw()
        {
            float textScale = 1f;
            float textWidth = Graphics._biosFont.GetWidth(text);
            if (textWidth > 34 * 8)
                textScale = 0.5f;
            else if (textWidth > 24 * 8)
                textScale = 0.75f;

            if (_directional)
                Graphics.DrawPassword(text, new Vec2(x - textWidth / 2 * textScale, y - 6f * textScale), Color.White, depth + 10);
            else
                Graphics.DrawString(text + (blink % 1f > 0.5f ? "_" : ""), new Vec2(x - textWidth / 2 * textScale, y - 6f * textScale), Color.White, depth + 10, scale: textScale);
            base.Draw();
        }
    }
}

﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.UIChangingText
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class UIChangingText : UIText
    {
        private FieldBinding _field;
        private FieldBinding _filterBinding;
        public string defaultSizeString = "ON OFF  ";

        public UIChangingText(float wide, float high, FieldBinding field, FieldBinding filterBinding)
          : base("ON OFF  ", Color.White)
        {
            _field = field;
            _filterBinding = filterBinding;
        }

        public override string text
        {
            get => _text;
            set
            {
                _text = value;
                if (minLength <= 0)
                    return;
                while (_text.Length < minLength)
                    _text = " " + _text;
            }
        }

        public override void Draw()
        {
            _font.scale = scale;
            _font.alpha = alpha;
            float width = _font.GetWidth(defaultSizeString);
            float num1 = (align & UIAlign.Left) <= UIAlign.Center ? ((align & UIAlign.Right) <= UIAlign.Center ? (float)(-width / 2.0) : this.width / 2f - width) : (float)-(this.width / 2.0);
            float num2 = (align & UIAlign.Top) <= UIAlign.Center ? ((align & UIAlign.Bottom) <= UIAlign.Center ? (float)(-_font.height / 2.0) : height / 2f - _font.height) : (float)-(height / 2.0);
            string text = this.text;
            while (text.Length < 8)
                text = " " + text;
            _font.colorOverride = UIMenu.disabledDraw ? Colors.BlueGray : new Color();
            _font.Draw(text, x + num1, y + num2, Color.White, depth);
        }
    }
}

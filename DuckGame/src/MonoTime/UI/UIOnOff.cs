﻿namespace DuckGame
{
    public class UIOnOff : UIText
    {
        private FieldBinding _field;
        private FieldBinding _filterBinding;

        public UIOnOff(float wide, float high, FieldBinding field, FieldBinding filterBinding)
          : base("ON OFF", Color.White)
        {
            _field = field;
            _filterBinding = filterBinding;
        }

        public override void Draw()
        {
            UILerp.UpdateLerpState(position, MonoMain.IntraTick, MonoMain.UpdateLerpState);

            _font.scale = scale;
            _font.alpha = alpha;
            float width = _font.GetWidth("ON OFF");
            float num1 = (align & UIAlign.Left) <= UIAlign.Center ? ((align & UIAlign.Right) <= UIAlign.Center ? (float)(-width / 2.0) : this.width / 2f - width) : (float)-(this.width / 2.0);
            float num2 = (align & UIAlign.Top) <= UIAlign.Center ? ((align & UIAlign.Bottom) <= UIAlign.Center ? (float)(-_font.height / 2.0) : height / 2f - _font.height) : (float)-(height / 2.0);
            bool flag = (bool)_field.value;
            if (_filterBinding != null)
            {
                if (!(bool)_filterBinding.value)
                    _font.Draw("   ANY", UILerp.x + num1, UILerp.y + num2, Color.White, depth);
                else if (flag)
                    _font.Draw("    ON", UILerp.x + num1, UILerp.y + num2, Color.White, depth);
                else
                    _font.Draw("   OFF", UILerp.x + num1, UILerp.y + num2, Color.White, depth);
            }
            else
            {
                _font.Draw("ON", UILerp.x + num1, UILerp.y + num2, flag ? Color.White : new Color(70, 70, 70), depth);
                _font.Draw("   OFF", UILerp.x + num1, UILerp.y + num2, !flag ? Color.White : new Color(70, 70, 70), depth);
            }
        }
    }
}

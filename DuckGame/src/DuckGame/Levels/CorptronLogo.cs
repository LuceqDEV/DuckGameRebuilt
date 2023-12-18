﻿namespace DuckGame
{
    public class CorptronLogo : Level
    {
        private BitmapFont _font;
        private Sprite _logo;
        private float _wait = 1f;
        private bool _fading;

        public CorptronLogo() => _centeredView = true;

        public override void Initialize()
        {
            _font = new BitmapFont("biosFont", 8);
            _logo = new Sprite("corptron");
            Graphics.fade = 0f;
        }

        public override void Update()
        {
            TitleScreen.SpargLogic();
            if (!_fading)
            {
                if (Graphics.fade < 1f)
                    Graphics.fade += 0.013f;
                else
                    Graphics.fade = 1f;
            }
            else if (Graphics.fade > 0f)
            {
                Graphics.fade -= 0.013f;
            }
            else
            {
                Graphics.fade = 0f;
                current = new AdultSwimLogo();
            }
            _wait -= 0.06f;
            if (_wait >= 0f)
                return;
            _fading = true;
        }

        public override void PostDrawLayer(Layer layer)
        {
            if (layer != Layer.Game)
                return;
            Graphics.Draw(_logo, 32f, 70f);
            _font.Draw("PRESENTED BY", 50f, 60f, Color.White);
        }
    }
}

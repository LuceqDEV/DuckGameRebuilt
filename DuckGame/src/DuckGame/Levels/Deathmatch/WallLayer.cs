﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.WallLayer
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DuckGame
{
    public class WallLayer : Layer
    {
        private Effect _fx;
        private float _scroll;
        public float fieldHeight;
        public Matrix _view;
        public Matrix _proj;
        private float _ypos;
        private List<Sprite> _sprites = new List<Sprite>();
        private List<Sprite> _wallSprites = new List<Sprite>();

        public float scroll
        {
            get => _scroll;
            set => _scroll = value;
        }

        public new Matrix view => _view;

        public new Matrix projection => _proj;

        public float rise { get; set; }

        public float ypos
        {
            get => _ypos;
            set => _ypos = value;
        }

        public void AddSprite(Sprite s) => _sprites.Add(s);

        public void AddWallSprite(Sprite s) => _wallSprites.Add(s);

        public WallLayer(string nameval, int depthval = 0)
          : base(nameval, depthval)
        {
            _fx = (Effect)Content.Load<MTEffect>("Shaders/fieldFadeAdd");
            _view = Matrix.CreateLookAt(new Vec3(0f, 0f, -5f), new Vec3(0f, 0f, 0f), Vec3.Up);
            _proj = Matrix.CreatePerspectiveFieldOfView(((float)Math.PI / 4.0f), (float)320 / (float)180, 0.01f, 100000);
        }

        public override void Update()
        {
            float num1 = 53f + fieldHeight + rise;
            float num2 = -0.1f;
            float scroll = this.scroll;
            _view = Matrix.CreateLookAt(new Vec3(scroll, 300f, -num1 + num2), new Vec3(scroll, 100f, -num1), Vec3.Down);
        }

        public override void Begin(bool transparent, bool isTargetDraw = false)
        {
            Vec3 fade = new Vec3((Graphics.fade * _fade) * (1.0f - _darken)) * colorMul;
            Vec3 fadeAdd = _colorAdd + new Vec3(_fadeAdd) + new Vec3(Graphics.flashAddRenderValue) + new Vec3(Graphics.fadeAddRenderValue) - new Vec3(darken);
            fadeAdd = new Vec3(Maths.Clamp(fadeAdd.x, -1f, 1f), Maths.Clamp(fadeAdd.y, -1f, 1f), Maths.Clamp(fadeAdd.z, -1f, 1f));
            if (_darken > 0f) _darken -= 0.15f;
            else _darken = 0f;
            if (_fx != null)
            {
                _fx.Parameters["fade"]?.SetValue((Vector3)fade);
                _fx.Parameters["add"]?.SetValue((Vector3)fadeAdd);
            }
            Graphics.screen = _batch;
            if (_state.ScissorTestEnable) Graphics.SetScissorRectangle(_scissor);
            _batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.DepthRead, _state, (MTEffect)_fx, camera.getMatrix());
        }

        public override void Draw(bool transparent, bool isTargetDraw = false)
        {
            Graphics.currentLayer = this;
            _fx.Parameters["WVP"].SetValue(Matrix.CreateRotationY(-(float)(Math.PI * 0.5f)) * Matrix.CreateTranslation(new Vec3((1 * 625), 20, 0.1f)) * _view * _proj);
            Begin(transparent, false);
            foreach (Sprite wallSprite in _wallSprites)
            {
                float x = wallSprite.x;
                Graphics.Draw(wallSprite, wallSprite.x, wallSprite.y);
                wallSprite.x = x;
            }
            _batch.End();
            _fx.Parameters["WVP"].SetValue(Matrix.CreateRotationY(-(float)(Math.PI * 0.5f)) * Matrix.CreateRotationZ(-(float)(Math.PI * 0.5f)) * Matrix.CreateTranslation(new Vec3((1 * 625.5f), 160, 0.1f)) * _view * _proj);
            Begin(transparent, false);
            foreach (Sprite wallSprite in _wallSprites)
            {
                float x = wallSprite.x;
                Graphics.Draw(wallSprite, wallSprite.x, wallSprite.y);
                wallSprite.x = x;
            }
            _batch.End();
            Graphics.screen = null;
            Graphics.currentLayer = null;
        }
    }
}

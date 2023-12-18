﻿using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace DuckGame
{
    public class Tex2D : Tex2DBase
    {
        protected Texture2D _base;
        public bool skipSpriteAtlas;
        private RenderTarget2D _effectTexture;

        public RenderTarget2D effectTexture
        {
            get
            {
                if (_effectTexture == null)
                    _effectTexture = new RenderTarget2D(width, height);
                return _effectTexture;
            }
        }
        public string Namebase
        {
            get
            {
                return _base.Name;
            }
            set
            {
                _base.Name = value;
            }
        }
        public Texture2D Texbase => _base;
        public override object nativeObject => _base;

        public override int width => _base == null ? -1 : _base.Width;

        public override int height => _base == null ? -1 : _base.Height;

        internal Tex2D(Texture2D tex, string texName, short curTexIndex = 0)
          : base(texName, curTexIndex)
        {
            _base = tex;
            _frameWidth = tex.Width;
            _frameHeight = tex.Height;
        }

        public Tex2D(int width, int height)
          : base("__internal", 0)
        {
            _base = new Texture2D(Graphics.device, width, height, false, SurfaceFormat.Color);
            _frameWidth = width;
            _frameHeight = height;
            Content.AssignTextureIndex(this);
        }
        public void SaveAsPng(Stream stream, int width, int height)
        {
            if (_base == null)
                return;
            _base.SaveAsPng(stream, width, height);
        }
        public override void GetData<T>(T[] data)
        {
            if (_base == null)
                return;
            _base.GetData(data);
        }

        public override Color[] GetData()
        {
            if (_base == null)
                return null;
            Color[] data = new Color[_base.Width * _base.Height];
            _base.GetData(data);
            return data;
        }

        public Color[,] GetData2D()
        {
            Color[] rawData = GetData();
            Color[,] data2D = new Color[_base.Width, _base.Height];
            
            for (int y = 0, i = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++, i++)
                {
                    data2D[x, y] = rawData[i];
                }
            }

            return data2D;
        }

        public override void SetData<T>(T[] colors)
        {
            if (_base == null)
                return;
            _base.SetData(colors);
        }

        public override void SetData(Color[] colors)
        {
            if (_base == null)
                return;
            _base.SetData(colors);
        }

        protected override void DisposeNative()
        {
            if (_base == null)
                return;
            if (!Graphics.disposingObjects)
            {
                lock (Graphics.objectsToDispose)
                    Graphics.objectsToDispose.Add(_base);
            }
            _base = null;
        }

        public static implicit operator Texture2D(Tex2D tex) => tex._base;

        public static implicit operator Tex2D(Texture2D tex)
        {
            return Content.GetTex2D(tex);
        }
    }
}

﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.WallLightRight
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System.Collections.Generic;

namespace DuckGame
{
    [EditorGroup("Details|Arcade", EditorItemType.Lighting)]
    [BaggedProperty("isInDemo", true)]
    public class WallLightRight : Thing
    {
        private PointLight _light;
        private SpriteThing _shade;
        private List<LightOccluder> _occluders = new List<LightOccluder>();

        public WallLightRight(float xpos, float ypos)
          : base(xpos, ypos)
        {
            this.graphic = new Sprite("wallLight");
            this.center = new Vec2(8f, 8f);
            this._collisionSize = new Vec2(5f, 16f);
            this._collisionOffset = new Vec2(2f, -8f);
            this.depth = (Depth)0.9f;
            this.hugWalls = WallHug.Right;
            this.layer = Layer.Game;
        }

        public override void Initialize()
        {
            if (Level.current is Editor)
                return;
            this._occluders.Add(new LightOccluder(this.topLeft, this.topRight + new Vec2(2f, 0.0f), new Color(1f, 0.8f, 0.8f)));
            this._occluders.Add(new LightOccluder(this.bottomLeft, this.bottomRight + new Vec2(2f, 0.0f), new Color(1f, 0.8f, 0.8f)));
            this._light = new PointLight(this.x + 5f, this.y, new Color((int)byte.MaxValue, (int)byte.MaxValue, 190), 100f, this._occluders);
            Level.Add((Thing)this._light);
            this._shade = new SpriteThing(this.x, this.y, new Sprite("wallLight"));
            this._shade.center = this.center;
            this._shade.layer = Layer.Foreground;
            this._shade.flipHorizontal = true;
            Level.Add((Thing)this._shade);
        }

        public override void Update()
        {
            this._light.visible = this.visible;
            this._shade.visible = this.visible;
            base.Update();
        }

        public override void Draw()
        {
            this.graphic.flipH = true;
            base.Draw();
        }
    }
}
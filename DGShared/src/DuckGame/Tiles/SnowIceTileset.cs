﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.SnowIceTileset
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    [EditorGroup("Blocks|Snow")]
    [BaggedProperty("isInDemo", false)]
    public class SnowIceTileset : AutoBlock
    {
        protected string meltedTileset = "snowTileset";
        protected string frozenTileset = "snowIceTileset";
        private float melt;
        private bool melted;

        public SnowIceTileset(float x, float y, string tset = "snowIceTileset")
          : base(x, y, tset)
        {
            _editorName = "Snow Ice";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidthThick = 15f;
            verticalWidth = 14f;
            horizontalHeight = 15f;
            _impactThreshold = -1f;
            willHeat = true;
            _tileset = "snowTileset";
            _sprite = new SpriteMap("snowIceTileset", 16, 16)
            {
                frame = 40
            };
            graphic = _sprite;
            frozenTileset = tset;
        }

        public override void Initialize()
        {
            if (level != null)
                level.cold = true;
            base.Initialize();
        }

        public void Freeze()
        {
            if (Network.isActive)
                return;
            melted = false;
            _sprite = new SpriteMap(frozenTileset, 16, 16)
            {
                frame = (graphic as SpriteMap).frame
            };
            graphic = _sprite;
            DoPositioning();
            melt = 0f;
        }

        public override void HeatUp(Vec2 location)
        {
            if (!Network.isActive)
            {
                melt += 0.05f;
                if (melt > 1.0)
                {
                    melted = true;
                    _sprite = new SpriteMap(meltedTileset, 16, 16)
                    {
                        frame = (graphic as SpriteMap).frame
                    };
                    graphic = _sprite;
                    DoPositioning();
                }
            }
            base.HeatUp(location);
        }

        public override void OnSolidImpact(MaterialThing with, ImpactedFrom from)
        {
            if (!melted && with is PhysicsObject)
            {
                (with as PhysicsObject).specialFrictionMod = 0.16f;
                (with as PhysicsObject).modFric = true;
            }
            base.OnSolidImpact(with, from);
        }
    }
}

﻿namespace DuckGame
{
    [EditorGroup("Stuff|Props")]
    [BaggedProperty("isInDemo", true)]
    public class Slag : Holdable, IPlatform
    {
        private SpriteMap _sprite;

        public Slag(float xpos, float ypos)
          : base(xpos, ypos)
        {
            _sprite = new SpriteMap("slag", 16, 16);
            graphic = _sprite;
            center = new Vec2(8f, 8f);
            collisionOffset = new Vec2(-8f, -8f);
            collisionSize = new Vec2(16f, 16f);
            depth = -0.5f;
            thickness = 4f;
            weight = 7f;
            flammable = 0f;
            collideSounds.Add("rockHitGround2");
            physicsMaterial = PhysicsMaterial.Metal;
            buoyancy = 1f;
            onlyFloatInLava = true;
            editorTooltip = "What a WORD! Floats in Lava and doesn't burn.";
        }

        public override void Update()
        {
            heat = 0f;
            base.Update();
        }

        public override bool Hit(Bullet bullet, Vec2 hitPos)
        {
            if (bullet.isLocal && owner == null)
                Fondle(this, DuckNetwork.localConnection);
            if (isServerForObject && bullet.isLocal && TeamSelect2.Enabled("EXPLODEYCRATES"))
            {
                if (duck != null)
                    duck.ThrowItem();
                Destroy(new DTShot(bullet));
                Level.Remove(this);
                Level.Add(new GrenadeExplosion(x, y));
            }
            return base.Hit(bullet, hitPos);
        }
    }
}

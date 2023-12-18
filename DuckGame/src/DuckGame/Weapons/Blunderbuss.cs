﻿namespace DuckGame
{
    [EditorGroup("Guns|Shotguns")]
    public class Blunderbuss : TampingWeapon
    {
        public Blunderbuss(float xval, float yval)
          : base(xval, yval)
        {
            wideBarrel = true;
            ammo = 99;
            _ammoType = new ATShrapnel
            {
                range = 140f,
                rangeVariation = 40f,
                accuracy = 0.01f
            };
            _numBulletsPerFire = 4;
            _ammoType.penetration = 0.4f;
            _type = "gun";
            graphic = new Sprite("blunderbuss");
            center = new Vec2(19f, 5f);
            collisionOffset = new Vec2(-8f, -3f);
            collisionSize = new Vec2(16f, 7f);
            _barrelOffsetTL = new Vec2(34f, 4f);
            _fireSound = "shotgun";
            _kickForce = 2f;
            _fireRumble = RumbleIntensity.Light;
            _holdOffset = new Vec2(4f, 1f);
            editorTooltip = "Old-timey shotgun, takes approximately 150 years to reload.";
            _editorPreviewOffset.x += 3;
            _editorPreviewWidth = 33;
        }

        public override void Update() => base.Update();

        public override void OnPressAction()
        {
            if (_tamped)
            {
                base.OnPressAction();
                int num = 0;
                for (int index = 0; index < 14 * Maths.Clamp(DGRSettings.ActualParticleMultiplier, 1, 100000); ++index)
                {
                    MusketSmoke musketSmoke = new MusketSmoke(barrelPosition.x - 16f + Rando.Float(32f), barrelPosition.y - 16f + Rando.Float(32f))
                    {
                        depth = (Depth)(0.9f + index * (1f / 1000f))
                    };
                    if (num < 6)
                        musketSmoke.move -= barrelVector * Rando.Float(0.1f);
                    if (num > 5 && num < 10)
                        musketSmoke.fly += barrelVector * (2f + Rando.Float(7.8f));
                    Level.Add(musketSmoke);
                    ++num;
                }
                _tampInc = 0f;
                _tampTime = infinite.value ? 0.5f : 0f;
                _tamped = false;
            }
            else
            {
                if (_raised || !(this.owner is Duck owner) || !owner.grounded)
                    return;
                owner.immobilized = true;
                owner.sliding = false;
                _rotating = true;
            }
        }
    }
}

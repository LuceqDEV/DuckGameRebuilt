﻿namespace DuckGame
{
    [BaggedProperty("canSpawn", false)]
    [BaggedProperty("isFatal", false)]
    public class WaterRifle : Gun
    {
        private FluidStream _stream;
        private ConstantSound _sound;
        private int _wait;

        public WaterRifle(float xval, float yval)
          : base(xval, yval)
        {
            ammo = 9;
            _ammoType = new AT9mm();
            _type = "gun";
            graphic = new Sprite("waterGun");
            center = new Vec2(11f, 7f);
            collisionOffset = new Vec2(-11f, -6f);
            collisionSize = new Vec2(23f, 13f);
            _barrelOffsetTL = new Vec2(24f, 6f);
            _fireSound = "pistolFire";
            _kickForce = 3f;
            _holdOffset = new Vec2(-1f, 0f);
            loseAccuracy = 0.1f;
            maxAccuracyLost = 0.6f;
            _bio = "";
            _editorName = "Water Blaster";
            physicsMaterial = PhysicsMaterial.Metal;
            _stream = new FluidStream(x, y, new Vec2(1f, 0f), 2f);
            isFatal = false;
        }

        public override void Initialize()
        {
            _sound = new ConstantSound("demoBlaster");
            Level.Add(_stream);
        }

        public override void Terminate() => Level.Remove(_stream);

        public override void Update() => base.Update();

        public override void OnPressAction()
        {
        }

        public override void OnHoldAction()
        {
            ++_wait;
            if (_wait != 3)
                return;
            _stream.sprayAngle = barrelVector * 2f;
            _stream.position = barrelPosition;
            FluidData dat = Fluid.Water;
            dat.amount = 0.01f;
            _stream.Feed(dat);
            _wait = 0;
        }
    }
}

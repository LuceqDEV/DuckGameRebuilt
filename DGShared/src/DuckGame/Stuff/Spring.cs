﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Spring
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System;

namespace DuckGame
{
    [EditorGroup("Stuff|Springs")]
    [BaggedProperty("isInDemo", true)]
    [BaggedProperty("previewPriority", true)]
    public class Spring : MaterialThing, IDontMove
    {
        public bool purple;
        protected SpriteMap _sprite;
        protected float _soundWait;
        protected float _mult;
        protected float _setMult;
        private bool _prevPurple;

        public Spring(float xpos, float ypos, float mult = 1f)
          : base(xpos, ypos)
        {
            _setMult = mult;
            purple = (bool)new EditorProperty<bool>(false, this);
            UpdateSprite();
            UpdatePower();
            editorCycleType = typeof(SpringUpRight);
            center = new Vec2(8f, 7f);
            collisionOffset = new Vec2(-8f, 0f);
            collisionSize = new Vec2(16f, 8f);
            depth = -0.5f;
            _editorName = nameof(Spring);
            editorTooltip = "Can't reach a high platform or want to get somewhere fast? That's why we built springs.";
            thickness = 0.1f;
            physicsMaterial = PhysicsMaterial.Metal;
            _impactThreshold = 0f;
            _mult = mult;
        }

        public virtual void UpdateAngle()
        {
        }

        public override ContextMenu GetContextMenu()
        {
            EditorGroupMenu contextMenu = base.GetContextMenu() as EditorGroupMenu;
            contextMenu.AddItem(new ContextCheckBox("Purple", null, new FieldBinding(this, "purple")));
            return contextMenu;
        }

        protected virtual void UpdateSprite()
        {
            if (purple)
            {
                _sprite = new SpriteMap("lightSpring", 16, 15);
                _sprite.ClearAnimations();
                _sprite.AddAnimation("idle", 1f, false, new int[1]);
                _sprite.AddAnimation("spring", 4f, false, 1, 2, 1, 0);
                _sprite.SetAnimation("idle");
                _sprite.speed = 0.1f;
                graphic = _sprite;
            }
            else
            {
                _sprite = new SpriteMap("spring", 16, 15);
                _sprite.ClearAnimations();
                _sprite.AddAnimation("idle", 1f, false, new int[1]);
                _sprite.AddAnimation("spring", 4f, false, 1, 2, 1, 0);
                _sprite.SetAnimation("idle");
                _sprite.speed = 0.1f;
                graphic = _sprite;
            }
        }

        public override BinaryClassChunk Serialize()
        {
            BinaryClassChunk binaryClassChunk = base.Serialize();
            binaryClassChunk.AddProperty("purple", purple);
            return binaryClassChunk;
        }

        public override bool Deserialize(BinaryClassChunk node)
        {
            base.Deserialize(node);
            purple = node.GetProperty<bool>("purple");
            return true;
        }

        protected void UpdatePower()
        {
            if (purple)
                _mult = _setMult * 0.3f;
            else
                _mult = _setMult;
        }

        public override void Initialize()
        {
            UpdateSprite();
            UpdatePower();
            base.Initialize();
        }

        public override void Update()
        {
            if (_soundWait > 0.0)
                _soundWait -= 0.1f;
            base.Update();
        }

        public override void EditorUpdate()
        {
            if (_prevPurple != purple)
            {
                UpdatePower();
                UpdateSprite();
                _prevPurple = purple;
            }
            base.EditorUpdate();
        }

        public void SpringUp()
        {
            _sprite.currentAnimation = "spring";
            _sprite.frame = 0;
            if (_soundWait > 0.0)
                return;
            SFX.Play("spring", 0.2f, Rando.Float(0.2f) - 0.1f);
            _soundWait = 1f;
        }

        protected void DoRumble(Duck duck) => RumbleManager.AddRumbleEvent(duck.profile, new RumbleEvent(RumbleIntensity.Kick, RumbleDuration.Short, RumbleFalloff.None));

        public override void Touch(MaterialThing with)
        {
            if (with.isServerForObject && with.Sprung(this))
            {
                if (with.vSpeed > -22.0 * _mult)
                    with.vSpeed = -22f * _mult;
                if (with is RagdollPart)
                {
                    if (Math.Abs(with.hSpeed) < 0.1f)
                        with.hSpeed = Rando.Float(1f) >= 0.5 ? 1.3f : -1.3f;
                    else
                        with.hSpeed *= Rando.Float(1.1f, 1.4f);
                }
                if (with is Mine)
                {
                    if (Math.Abs(with.hSpeed) < 0.1f)
                        with.hSpeed = Rando.Float(1f) >= 0.5 ? 1.2f : -1.2f;
                    else
                        with.hSpeed *= Rando.Float(1.1f, 1.2f);
                }
                with.lastHSpeed = with._hSpeed;
                with.lastVSpeed = with._vSpeed;
                if (with is Duck)
                {
                    (with as Duck).jumping = false;
                    DoRumble(with as Duck);
                }
                if (with is Gun)
                    (with as Gun).PressAction();
            }
            SpringUp();
        }

        public override void Draw()
        {
            UpdateAngle();
            base.Draw();
        }
    }
}

﻿using Microsoft.Xna.Framework.Audio;

namespace DuckGame
{
    public class MultiSound : Sound
    {
        private MultiSoundUpdater _controller;
        private SoundState _state = SoundState.Stopped;

        public override bool IsDisposed => _controller.IsDisposed;

        public override float Pitch
        {
            get => _controller.Pitch;
            set => _controller.Pitch = value;
        }

        public override float Pan
        {
            get => _controller.Pan;
            set => _controller.Pan = value;
        }

        public override bool IsLooped
        {
            get => _controller.IsLooped;
            set => _controller.IsLooped = value;
        }

        public override float Volume
        {
            set => _volume = value;
        }

        public override SoundState State => _state;

        public override void Play()
        {
            if (_killed)
                return;
            _controller.Play(this);
            _state = SoundState.Playing;
        }

        public override void Stop()
        {
            if (_killed)
                return;
            _controller.Stop(this);
            _state = SoundState.Stopped;
        }

        public new void Unpooled()
        {
            if (_state != SoundState.Stopped)
                _controller.Stop(this);
            _pooled = false;
        }

        public new void Pause()
        {
            int num = _killed ? 1 : 0;
        }

        public MultiSound(MultiSoundUpdater updater)
        {
            _controller = updater;
            _volume = 0f;
        }
    }
}

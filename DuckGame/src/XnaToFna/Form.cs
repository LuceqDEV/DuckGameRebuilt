﻿// Decompiled with JetBrains decompiler
// Type: XnaToFna.ProxyForms.Form
// Assembly: XnaToFna, Version=18.5.1.29483, Culture=neutral, PublicKeyToken=null
// MVID: C1D3521D-C7E9-4C43-B430-D28CC69450A3
// Assembly location: C:\Users\daniel\Desktop\Release\XnaToFna.exe

using System;
using System.Threading;

namespace XnaToFna.ProxyForms
{
    public class Form : Control
    {
        public IntPtr WindowHookPtr;
        public Delegate WindowHook;
        public int ThreadId;

        public virtual FormBorderStyle FormBorderStyle { get; set; }

        public virtual FormWindowState WindowState { get; set; }

        public virtual FormStartPosition StartPosition { get; set; }

        public virtual bool KeyPreview { get; set; }

        public Form()
        {
            Form = this;
            ThreadId = Thread.CurrentThread.ManagedThreadId;
            StartPosition = FormStartPosition.WindowsDefaultLocation;
            KeyPreview = false;
        }

        public event FormClosingEventHandler FormClosing;

        public event FormClosedEventHandler FormClosed;

        protected virtual void OnFormClosing(FormClosingEventArgs e)
        {
        }

        protected virtual void OnFormClosed(FormClosedEventArgs e)
        {
        }

        protected virtual void _Close()
        {
        }

        public void Close()
        {
            FormClosingEventArgs e1 = new FormClosingEventArgs(CloseReason.None, false);
            OnFormClosing(e1);
            FormClosing(this, e1);
            _Close();
            FormClosedEventArgs e2 = new FormClosedEventArgs(CloseReason.None);
            OnFormClosed(e2);
            FormClosed(this, e2);
        }

        protected override void WndProc(ref Message msg)
        {
            ref Message local = ref msg;
            Delegate windowHook = WindowHook;
            object obj;
            if ((object)windowHook == null)
                obj = null;
            else
                obj = windowHook.DynamicInvoke(msg.HWnd, msg.Msg, msg.WParam, msg.LParam);
            IntPtr num = (IntPtr)obj;
            local.Result = num;
        }
    }
}

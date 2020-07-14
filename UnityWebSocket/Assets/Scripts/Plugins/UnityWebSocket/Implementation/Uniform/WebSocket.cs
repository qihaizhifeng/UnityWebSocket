﻿using System;

namespace UnityWebSocket
{
    /// <summary>
    /// For All Platform
    /// </summary>
    public class WebSocket : IWebSocket
    {
        #region Public Events
        public event EventHandler OnOpen;
        public event EventHandler<CloseEventArgs> OnClose;
        public event EventHandler<ErrorEventArgs> OnError;
        public event EventHandler<MessageEventArgs> OnMessage;
        #endregion

        public string Address { get { return _rawSocket.Address; } }

        public WebSocketState ReadyState { get { return _rawSocket.ReadyState; } }

        private readonly IWebSocket _rawSocket;

        public WebSocket()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            _rawSocket = new WebGL.WebSocket();
#elif !NET_LEGACY
            _rawSocket = new Common.WebSocket();
#else
            throw new Exception("Scripting Runtime Version should be .NET 4.x, via Menu:\nPlayerSettings -> Other Settings -> Script Runtime Version -> .Net 4.x Equivalent");
#endif

            _rawSocket.OnOpen += (o, e) =>
            {
                if (OnOpen != null)
                    OnOpen.Invoke(this, EventArgs.Empty);
            };
            _rawSocket.OnClose += (o, e) =>
            {
                if (OnClose != null)
                    OnClose.Invoke(this, new CloseEventArgs(e.Code, e.Reason, e.WasClean));
            };
            _rawSocket.OnError += (o, e) =>
            {
                if (OnError != null)
                    OnError.Invoke(this, new ErrorEventArgs(e.Message, e.Exception));
            };
            _rawSocket.OnMessage += (o, e) =>
            {
                if (OnMessage != null)
                    OnMessage.Invoke(this, new MessageEventArgs(e.Opcode, e.RawData));
            };
        }


        public void SendAsync(string data, Action<bool> callback)
        {
            _rawSocket.SendAsync(data, callback);
        }

        public void ConnectAsync(string address)
        {
            _rawSocket.ConnectAsync(address);
        }
        public void CloseAsync()
        {
            _rawSocket.CloseAsync();
        }
        public void SendAsync(byte[] data, Action<bool> completed)
        {
            _rawSocket.SendAsync(data, completed);
        }

    }
}
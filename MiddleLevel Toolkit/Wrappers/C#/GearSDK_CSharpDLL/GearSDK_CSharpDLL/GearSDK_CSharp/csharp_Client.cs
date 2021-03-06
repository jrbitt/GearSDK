﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GearSDK_CSharpDLL
{
    public class csharp_Client
    {
        #region Private Attributes

        int atualNumberOfMessages = 0;
        bool handShakeDone = false;
        bool canUpdateObjects = false;

        #endregion

        #region Public Attributes

        public csharp_Connection connection;
        public csharp_Wrapper wrapper;

        #endregion

        #region Private Methods

        void HandShakeWithServer()
        {
            while (!wrapper.HeaderSetted)
            {
                string message = connection.ReceivedMessage();

                if(!string.IsNullOrEmpty(message))
                    wrapper.Init(connection.ReceivedMessage());
            }
            if (!handShakeDone)
            {
                connection.SendMessage("@ Hand Shake");
                handShakeDone = true;
            }
        }

        #endregion

        #region Constructors

        public csharp_Client()
        {

        }

        public csharp_Client(string ip, int port = 81)
        {
            this.connection = new csharp_Connection(ip, port);
            this.wrapper = new csharp_Wrapper();
        }

        #endregion

        #region Getters and Setters

        public int AtualNumberOfMessages { get => atualNumberOfMessages; set => atualNumberOfMessages = value; }
        public bool HandShakeDone { get => handShakeDone; set => handShakeDone = value; }
        public bool CanUpdateObjects { get => canUpdateObjects; }

        #endregion

        #region Public Methods

        public void Init()
        {
            connection.StablishConnection();
        }

        public void Update()
        {
            try
            {
                //while connected with the server, try read messages
                if (connection.IsConnected)
                {
                    if (!handShakeDone)
                        HandShakeWithServer(); //Loop until handshake done!

                    wrapper.UpdateObjects(connection.ReceivedMessage());    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error on client loop: " + e.Data);
            }

        }

        #endregion
    }
}

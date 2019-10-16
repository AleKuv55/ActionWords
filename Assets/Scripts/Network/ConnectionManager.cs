using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;

using System.Threading;

public class ConnectionManager : MonoBehaviour
{
    private class AssyncInfo
    {
        public const int BufferSize = 256;
        public byte[] buffer = new byte[BufferSize];

        private Action<string> _action;


        public void SetAction(Action<string> action) { _action = action; }
        public void DoAction(string msg)
        {
            _action(msg);
        }
    }


    [SerializeField] private MainMenuManager _mainMenuManager;
    [SerializeField] private Text _ipText;
    [SerializeField] private Text _portText;

    private static AssyncInfo _assyncInfo;
    private static string _IP;      public string GetIP() { return _IP; }
    private static int _port;
    private static TcpClient _client;




    private void Start()
    {
        _assyncInfo = new AssyncInfo();
        
    }

    public void TryToConnect()
    {
        _IP = _ipText.text;
        _port = int.Parse(_portText.text);

        Debug.Log("IP: " + _IP);
        Debug.Log("Port: " + _port);
        
        _client = new TcpClient();

        try
        {
            _client.Connect(_IP, _port);
        }
        catch (SocketException e)
        {
            Disconnect();
        }

        //SendMsg("0");
        _mainMenuManager.OnConnected();
    }

    public void Disconnect()
    {
        //_client
    }

    public void PrintIP()
    { Debug.Log(GetIP()); }

    
    public static void SendMsg(string json)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        int n = 0;
        for (int i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] == 0)
            {
                n = i;
                break;
            }
        }
        Debug.Log("Sending bytes length: " + bytes.Length + ". First zero is at " + n + ".");
        //byte[] bytes = StringToByteArray(json);

        NetworkStream stream = _client.GetStream();
        stream.BeginWrite(bytes, 0, bytes.Length, OnMessageSent, _assyncInfo);
    }

    public static byte[] StringToByteArray(string str)
    {
        char[] chArr = str.ToCharArray();
        byte[] byArr = new byte[chArr.Length];

        for (int i = 0; i < chArr.Length; i++)
            byArr[i] = (byte)chArr[i];
        return byArr;
    }

    public static void OnMessageSent(IAsyncResult ar)
    {
        NetworkStream stream = _client.GetStream();
        stream.EndWrite(ar);
    }


    public static void GetMsg(Action<string> OnReceivedCallback)
    {
        Debug.Log("Entered GetMsg");
        _assyncInfo.SetAction(OnReceivedCallback);
        
        NetworkStream stream = _client.GetStream();

        stream.BeginRead (_assyncInfo.buffer, 0, _assyncInfo.buffer.Length,
                          new AsyncCallback(OnMessageReceived), _assyncInfo);
    }

    private static void OnMessageReceived (IAsyncResult ar)
    {
        int     nBytes  = _client.GetStream().EndRead(ar);
        string  message = System.Text.Encoding.UTF8.GetString(_assyncInfo.buffer, 0, nBytes);

        Debug.Log("Message received: " + message + " (" + message.Length + ")");
        Debug.Log(message.Length + ")");

        try
        {
            _assyncInfo.DoAction(message);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }


    public void TrialReceiveMessage()
    {
        GetMsg(Debug.Log);
    }

    public void Snd(string msg)
    {
        SendMsg(msg);
    }
}

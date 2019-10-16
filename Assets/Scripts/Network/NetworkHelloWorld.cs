using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using UnityEngine.UI;

using System.Threading;

public class NetworkHelloWorld : MonoBehaviour
{
    [SerializeField] Text _ipText;
    [SerializeField] Text _portText;
    [SerializeField] Text _receivedMessageText;
    TcpClient client;
    Socket socket;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ConnectToServer()
    {
        Debug.Log("IP: " + _ipText.text);
        Debug.Log("Port: " + _portText.text);

        
        client = new TcpClient();
        client.Connect(_ipText.text, int.Parse(_portText.text));
        
        /*
        IPHostEntry ipHost = Dns.GetHostEntry("localhost");
        IPAddress ipAddr = ipHost.AddressList[0];

        IPAddress address;
        IPAddress.TryParse(_ipText.text, out address);
        IPEndPoint ipEndPoint = new IPEndPoint(address, int.Parse(_portText.text));

        socket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(ipEndPoint);
        */
    }

    public void SendMessage()
    {
        string msg = "Vy poymali troyanskuyu programmu. Sent nomer vashey credit card.";
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(msg);
        //byte[] bytes = { 1, 2, 3};
        NetworkStream stream = client.GetStream();
        stream.Write(bytes, 0, bytes.Length);
        
        
        //socket.Send(bytes);
    }

    public void GetMessage()
    {
        byte[] data = new byte[256];
        NetworkStream stream = client.GetStream();

        int nBytes = stream.Read(data, 0, data.Length);

        //int nBytes = socket.Receive(data);

        _receivedMessageText.text = "" + data[0] + data[1] + data[2];
    }



    static int x = 0;
    static object locker = new object();

    public void StartThread()
    {
        x = 1;
        for (int i = 0; i < 5; i++)
        {
            Thread myThread = new Thread(Count);
            myThread.Name = "Поток " + i.ToString();
            myThread.Start();
        }
    }

    public static void Count()
    {
        for (int i = 1; i < 9; i++)
        {
            lock (locker)
            {
                Debug.Log(Thread.CurrentThread.Name + ": " + x);
                x++;
                Thread.Sleep(100);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NetMgr : MonoBehaviour
{
    private static NetMgr instance;

    public int playerID;
    public static NetMgr Instance=>instance;
    private Socket clientSocket;
    //���ڷ�����Ϣ�Ķ��У��������������߳�����ţ������̴߳�����ȡ
    private Queue<string> sendMsgQueue = new Queue<string>();
        //�洢PlayerMsg�Ķ���
    private Queue<PlayerMsg> playerMsgQueue= new Queue<PlayerMsg>();
    //���ڽ�����Ϣ�Ķ��У��������������߳�����ţ����̴߳���ȡ
    public Queue<string> receiveMsgQueue = new Queue<string>();
    public Queue<PlayerMsg> receivePlayerMsgQueue = new Queue<PlayerMsg>();
    //���ڽ�����Ϣ������
    private byte[] receiveBytes = new byte[1024*1024];
    private int receiveNum;

    PlayerMsg playerMsgOP=new PlayerMsg();

    private bool isConnected;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        if(receiveMsgQueue.Count > 0)
        {
            print(receiveMsgQueue.Dequeue());
        }
        if (receivePlayerMsgQueue.Count > 0)
        {
            playerMsgOP= receivePlayerMsgQueue.Dequeue();
        }
    }
    //���ӷ�����
    public bool ConnectServer(string ip,int point)
    {
        if (isConnected)
            return true;
        if (clientSocket == null)
        {
            clientSocket=new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);   
        }
        IPEndPoint ipPoint=new IPEndPoint(IPAddress.Parse(ip),point);
        try
        {
            clientSocket.Connect(ipPoint);
            isConnected = true;
            //��ý�ɫID
            clientSocket.Receive(receiveBytes);
            playerID=BitConverter.ToInt32(receiveBytes, 0);
            print(playerID);
            //���������߳�
            Task.Run(() => SendMsg());
            //���������߳�
            Task.Run(() => ReceiveMsg());
            return true;
        }
        catch (SocketException e)
        {
            if (e.ErrorCode == 10061)
            {
                print("�������ܾ�����");
            }                
            else
            {
                print("����ʧ��" + e.ErrorCode + e.Message);
            }
            return false;
        }
    }
    //������Ϣ
    public void GetMsg(string msgStr)
    {
        //if (clientSocket != null)
        //{
        //    clientSocket.Send(Encoding.UTF8.GetBytes(msgStr));
        //}
        sendMsgQueue.Enqueue(msgStr);
    }
        //����PlayerMsg
    public void GetPlayerMsg(PlayerMsg playerMsg)
    {
        playerMsgQueue.Enqueue(playerMsg);
    }
    private void SendMsg()
    {
        while(isConnected)
        {
            if(sendMsgQueue.Count > 0)
            {
                clientSocket.Send(Encoding.UTF8.GetBytes(sendMsgQueue.Dequeue()));
            }
            if(playerMsgQueue.Count>0)
            {
                clientSocket.Send(playerMsgQueue.Dequeue().Writing());
            }
        }
    }
    //��ͣ������Ϣ
    private void ReceiveMsg()
    {
        while (isConnected)
        {
            if (clientSocket.Available > 0)
            {
                receiveNum = clientSocket.Receive(receiveBytes);
                if (BitConverter.ToInt32(receiveBytes,0)==1001)
                {
                    print("Receive PlayerMsg");
                    PlayerMsg playerMsg = new PlayerMsg();
                    playerMsg.Reading(receiveBytes, 4);
                    receivePlayerMsgQueue.Enqueue(playerMsg);
                }
                else
                {
                    receiveMsgQueue.Enqueue(Encoding.UTF8.GetString(receiveBytes, 0, receiveNum));
                }               
            }
        }
    }
       
    public void CloseConnect()
    {
        if(clientSocket!=null)
        {
            isConnected = false;
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            clientSocket = null;
        }
    }
    private void OnDestroy()
    {
        CloseConnect();
    }
}

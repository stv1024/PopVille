using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using ProtoBuffer;
using UnityEngine;


public class UnityClient : MonoBehaviour
{

    private static UnityClient _instance;

    /// <summary>
    /// ����������
    /// </summary>
    public static UnityClient Instance
    {
        get
        {
            return _instance;
        }
        set
        {
            if (_instance)
            {
                if (_instance.HasConnectedToServer)
                {
                    _instance.Close();
                }
                Destroy(_instance.gameObject);
            }
            _instance = value;
        }
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }


    private Responder _responsor;

    private NetworkManager _networkManager;

    #region �Զ������enum

    /// <summary>
    /// �¼�����
    /// </summary>
    private enum EventType
    {

        /// <summary>
        /// ���Ϸ�������
        /// </summary>
        Connected,

        /// <summary>
        /// �޷����Ϸ�����
        /// </summary>
        CanNotConnect,

        /// <summary>
        /// ��������Ͽ������ˣ�һ����������ʱ�����߷��ͺͽ��յ�ʱ��session�ϵ��ˣ�
        /// </summary>
        DisConnect
    }

    /// <summary>
    /// �¼�����Ҫ��������3���¼����ͣ�
    /// </summary>
    private class Event
    {
        public EventType Type { get; set; }
    }

    /// <summary>
    /// �첽���յ����ݡ�
    /// </summary>
    private class RsvData
    {
        /// <summary>
        /// header һ��4���ֽڵ�����
        /// </summary>
        public byte[] Header;

        /// <summary>
        /// ���ݻ�������
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// ��Ҫ���յ����ݳ��ȡ�
        /// </summary>
        public int DataLength;

        /// <summary>
        /// ��ǰ���ݱ����ƫ���������磺��Ҫ1024���Ѿ���ȡ��800����Offset==800��
        /// </summary>
        public int Offset;

        /// <summary>
        /// socket��������
        /// </summary>
        public NetworkStream Stream;
    }

    #endregion

    #region ����

    /// <summary>
    /// ����
    /// </summary>
    protected string Name { get; set; }

    /// <summary>
    /// �ͻ����Ƿ����Ϸ�����
    /// </summary>
    private bool HasConnectedToServer { get; set; }

    private TcpClient Client { get; set; }

    /// <summary>
    /// ��������ַ
    /// </summary>
    private string Host { get; set; }

    /// <summary>
    /// �������˿ں�
    /// </summary>
    private int Port { get; set; }

    /// <summary>
    /// ��ʼ���ӷ�������ʱ�䣬ֻ�е�һ�γ�ʼ����ʱ�����ã����಻�����ã���Ҫ�������ӳ�ʱ���ж�
    /// </summary>
    private DateTime ConnectStartTime { get; set; }

    /// <summary>
    /// �Ƿ��������ӳ�ʱ���������������첽���ʱ����м�⣬���򲻽��м������������ӷ�����
    /// </summary>
    protected bool EnableConnectTimeOut { get; set; }

    /// <summary>
    /// ���ӷ�������ʱʱ�䣨ǰ��������EnableConnectTimeOutΪtrue��,��λ����
    /// </summary>
    protected int ConnectTimeOut { get; set; }

    public DateTime LastClientHeartBeat { get; set; }

    public DateTime LastServerHeartBeat { get; set; }

    /// <summary>
    /// �Ƿ���������
    /// </summary>
    protected bool EnableHeartBeat { get; set; }

    /// <summary>
    /// �������ʱ�䣨������������һ��ͨ�ż�¼����ʱ����ڶ��������������������
    /// </summary>
    protected int HeartBeatInterval { get; set; }

    /// <summary>
    /// ������ʱʱ�䣨����������һ��ͨ��ʱ�������ڴ��ڶ��������Ϊ�Ƕ����ˣ�
    /// </summary>
    protected int HeartBeatTimeOut { get; set; }

    /// <summary>
    /// д��Ϣ����
    /// </summary>
    private Queue WriteQueue { get; set; }

    /// <summary>
    /// ��ȡ��Ϣ����
    /// </summary>
    private Queue ReadQueue { get; set; }

    /// <summary>
    /// �¼�����
    /// </summary>
    private Queue EventQueue { get; set; }

    /// <summary>
    /// �첽�߳��Ƿ���д����
    /// </summary>
    private bool IsWriting { get; set; }

    /// <summary>
    /// �첽�߳��Ƿ��ڶ�ȡ����
    /// </summary>
    private bool IsReading { get; set; }

    /// <summary>
    /// �Ƿ���ʾlog��
    /// </summary>
    private bool ShowLog { get { return false; } }// set; }

    protected int MaxReadCmdNumberPerFrame { get; set; }

    private int TmpReadCmdNumber { get; set; }

    #endregion

    //TODO:MonoBehaviourҪ����������
    public UnityClient()
        : this("UnityClient", true, 30, 60, 3, 0)
    {
    }

    public UnityClient(
        string name,
        bool enableHeartBeat,
        int heartBeatInterval,
        int heartBeatTimeOut,
        int maxReadCmdPerFrame,
        int tmpReadCmd,
        bool showLog = true)
    {
        Name = name;
        EnableHeartBeat = enableHeartBeat;
        EnableConnectTimeOut = EnableConnectTimeOut;
        HeartBeatInterval = heartBeatInterval;
        HeartBeatTimeOut = heartBeatTimeOut;
        MaxReadCmdNumberPerFrame = maxReadCmdPerFrame;
        TmpReadCmdNumber = tmpReadCmd;

        //ShowLog = showLog;
        WriteQueue = Queue.Synchronized(new Queue());
        ReadQueue = Queue.Synchronized(new Queue());
        EventQueue = Queue.Synchronized(new Queue());
    }

    /// <summary>
    /// ��ʼ�������ӷ�����
    /// </summary>
    /// <param name="host">��������ַ</param>
    /// <param name="port">�������˿ں�</param>
    public void InitAndConnect(string host, int port, Responder responsor, NetworkManager networkManager)
    {
        Log(Name, string.Format("��ʼ���ͻ��ˣ���������ַ��{0}���������˿ںţ�{1},ʱ��:{2}", host, port, DataTime2String(DateTime.Now)));

        _responsor = responsor;

        _networkManager = networkManager;



        Host = host;
        Port = port;
        HasConnectedToServer = false;
        ConnectStartTime = DateTime.Now;
        HasConnectedToServer = false;
        Connect();
    }

    /// <summary>
    /// �ر����ӡ�
    /// </summary>
    public void Close()
    {
        Log(Name, string.Format("�رտͻ��ˣ�ʱ�䣺{0}", DataTime2String(DateTime.Now)));
        if (Client != null)
        {
            Client.Close();
        }
    }

    #region ���ӷ�����

    /// <summary>
    /// ���ӷ�������
    /// </summary>
    private void Connect()
    {
        Log(Name, string.Format("��ʼ���ӷ�������ʱ��:{0}", DataTime2String(DateTime.Now)));
        try
        {
            Client = new TcpClient { NoDelay = true };
            Client.BeginConnect(Host, Port, FinishConnect, Client);
        }
        catch (Exception exception)
        {
            LogError(Name, string.Format("{0},{1}", exception.Message, exception.StackTrace));
            EventQueue.Enqueue(new Event { Type = EventType.CanNotConnect });
        }
    }

    /// <summary>
    /// �첽�����ӹ�������ˡ�
    /// </summary>
    /// <param name="result"></param>
    private void FinishConnect(IAsyncResult result)
    {
        Log(Name, string.Format("�������ӷ�������ʱ�䣺{0}", DataTime2String(DateTime.Now)));

        var client = result.AsyncState as TcpClient;

        if (EnableConnectTimeOut)
        {
            if (DateTime.Now.Ticks - ConnectStartTime.Ticks > ConnectTimeOut * 1000 * 10000)
            {
                LogError(Name, "���ӳ�ʱ���޷����Ϸ�����");
                EventQueue.Enqueue(new Event { Type = EventType.CanNotConnect });
                return;
            }
        }

        if (client == null)
        {
            LogError(Name, "TcpClient == null ����ʧ�ܣ�");
        }
        else
        {
            try
            {
                client.EndConnect(result);
            }
            catch (Exception exception)
            {
                LogError(Name, string.Format("{0},{1}", exception.Message, exception.StackTrace));
            }

            if (client.Connected)
            {
                Log(Name, string.Format("���Ϸ������ˣ�ʱ�䣺{0}", DataTime2String(DateTime.Now)));
                HasConnectedToServer = true;
                LastClientHeartBeat = DateTime.Now;
                LastServerHeartBeat = DateTime.Now;
                IsWriting = false;
                IsReading = false;
                EventQueue.Enqueue(new Event { Type = EventType.Connected });
            }
            else
            {
                LogError(Name, "��������ʧ�ܣ���3�룬���ٳ�������");
                Thread.Sleep(3 * 1000);
                Connect();
            }
        }
    }

    #endregion

    #region ����

    private void HeartBeat()
    {
        if (EnableHeartBeat)
        {
            if (DateTime.Now.Ticks - LastServerHeartBeat.Ticks > HeartBeatTimeOut * 1000 * 10000)
            {
                LogError(Name, string.Format("�����ˣ�ʱ�䣺{0}", DataTime2String(DateTime.Now)));
                HasConnectedToServer = false;
                EventQueue.Enqueue(new Event { Type = EventType.DisConnect });
                return;
            }
            if (DateTime.Now.Ticks - LastClientHeartBeat.Ticks > HeartBeatInterval * 1000 * 10000) //����ʱ�䵽��
            {
                Log(Name, string.Format("����������ʱ�䣺{0}", DataTime2String(DateTime.Now)));
                var hb = BuildHeartBeatCmd();
                WriteCmd(hb);
                LastClientHeartBeat = DateTime.Now;
            }
        }
    }

    /// <summary>
    /// ����������Ϣ��
    /// </summary>
    /// <returns></returns>
    protected object BuildHeartBeatCmd()
    {
        // todo ����������Ϣ��
        var cmd = new HBReq {SerialId = DateTime.Now.Ticks};
        var packet = new Packet(0) {Content = cmd.GetProtoBufferBytes()};
        return packet;
    }


    #endregion

    #region ������Ϣ

    public void WriteCmd(object obj)
    {
        Log(Name, string.Format("����Ϣ�������һ����Ϣ��ʱ�䣺{0}", DataTime2String(DateTime.Now)));
        WriteQueue.Enqueue(obj);
    }

    /// <summary>
    /// ִ��ʵ�ʵ�д������
    /// </summary>
    private void Write()
    {
        if (WriteQueue.Count < 1)
        {
            return;
        }

        if (IsWriting)
        {
            LogWarning(Name, string.Format("�첽�߳�����д��Ϣ,{0}", DataTime2String(DateTime.Now)));
            return;
        }

        StartWrite();
    }

    /// <summary>
    /// ��ʼ�첽д��Ϣ��
    /// </summary>
    private void StartWrite()
    {
        try
        {
            IsWriting = true;
            var stream = Client.GetStream();

            var cmd = WriteQueue.Dequeue();
            var data = BuildData(cmd);
            var buf = new byte[4 + data.Length];

            buf[0] = (byte)(data.Length >> 24);
            buf[1] = (byte)(data.Length >> 16);
            buf[2] = (byte)(data.Length >> 8);
            buf[3] = (byte)(data.Length >> 0);

            data.CopyTo(buf, 4);
            Log(Name, string.Format("��ʼ������Ϣ��ʱ�䣺{0}", DataTime2String(DateTime.Now)));
            stream.BeginWrite(buf, 0, buf.Length, FinishWrite, stream);
        }
        catch (Exception exception)
        {
            HasConnectedToServer = false;
            EventQueue.Enqueue(new Event { Type = EventType.DisConnect });
            LogError(Name, string.Format("{0},{1}", exception.Message, exception.StackTrace));
        }
    }

    /// <summary>
    /// ��ȡ�����bytes��
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected byte[] BuildData(object obj)
    {
        var cmd = obj as Packet;
        return cmd.GetProtoBufferBytes();
    }

    /// <summary>
    /// ����д������
    /// </summary>
    /// <param name="result"></param>
    private void FinishWrite(IAsyncResult result)
    {
        Log(Name, string.Format("������Ϣ��ɣ�ʱ�䣺{0}", DataTime2String(DateTime.Now)));
        var stream = result.AsyncState as NetworkStream;
        if (stream == null)
        {
            LogError(Name, "stream = null");
        }
        else
        {
            try
            {
                stream.EndWrite(result);
            }
            catch (Exception exception)
            {
                LogError(Name, string.Format("{0},{1}", exception.Message, exception.StackTrace));
                HasConnectedToServer = false;
                EventQueue.Enqueue(new Event { Type = EventType.DisConnect });
            }
            if (WriteQueue.Count > 0)
            {
                StartWrite();
            }
            else
            {
                IsWriting = false;
            }
        }
    }

    #endregion

    #region ������Ϣ

    private void Read()
    {
        if (IsReading) return;

        if (Client.Available > 4)
        {
            TmpReadCmdNumber = 0;
            StartReadHeader();
        }
    }

    private void StartReadHeader()
    {
        try
        {
            IsReading = true;
            var stream = Client.GetStream();
            var data = new RsvData
            {
                Header = new byte[4],
                Stream = stream
            };

            Log(Name, string.Format("���յ���Ϣ��ʱ�䣺{0}", DataTime2String(DateTime.Now)));
            stream.BeginRead(data.Header, 0, 4, FinishReadHeader, data);
        }
        catch (Exception exception)
        {
            LogError(Name, string.Format("{0},{1}", exception.Message, exception.StackTrace));
            HasConnectedToServer = false;
            EventQueue.Enqueue(new Event { Type = EventType.DisConnect });
        }
    }

    private void FinishReadHeader(IAsyncResult result)
    {
        try
        {
            var data = result.AsyncState as RsvData;
            if (data == null)
            {
                LogError(Name, "data = null");
            }
            else
            {
                var stream = data.Stream;
                var leng = 0;
                leng += data.Header[0] << 24;
                leng += data.Header[1] << 16;
                leng += data.Header[2] << 8;
                leng += data.Header[3] << 0;
                data.Data = new byte[leng];
                data.DataLength = leng;
                data.Offset = 0;
                stream.BeginRead(data.Data, 0, leng, FinishReadBody, data);
            }
        }
        catch (Exception exception)
        {
            LogError(Name, string.Format("{0},{1}", exception.Message, exception.StackTrace));
            HasConnectedToServer = false;
            EventQueue.Enqueue(new Event { Type = EventType.DisConnect });
        }
    }

    private void FinishReadBody(IAsyncResult result)
    {
        try
        {
            var data = result.AsyncState as RsvData;
            if (data == null)
            {
                LogError(Name, "data = null FinishReadBody");
            }
            else
            {
                var stream = data.Stream;
                var size = stream.EndRead(result);
                data.Offset = data.Offset + size;
                if (data.Offset < data.DataLength)
                {
                    data.Stream.BeginRead(
                        data.Data,
                        data.Offset,
                        data.DataLength - data.Offset,
                        FinishReadBody,
                        data);
                }
                else
                {
                    TmpReadCmdNumber++;
                    Log(Name,
                        string.Format("��������Ϣ��ʱ�䣺{0}����  {1} ������", DataTime2String(DateTime.Now), TmpReadCmdNumber));
                    LastServerHeartBeat = DateTime.Now;
                    LastClientHeartBeat = DateTime.Now;
                    var cmd = ParseCmd(data.Data);
                    ReadQueue.Enqueue(cmd);

                    if (TmpReadCmdNumber < MaxReadCmdNumberPerFrame)
                    {
                        if (Client.Available > 4)
                        {
                            StartReadHeader();
                        }
                        else
                        {
                            IsReading = false;
                        }
                    }
                    else
                    {
                        LogWarning(Name, string.Format("��һ֡���ٽ���������,ʱ�䣺{0}", DateTime.Now));
                        IsReading = false;
                    }
                }

            }

        }
        catch (Exception exception)
        {
            LogError(Name, string.Format("{0},{1}", exception.Message, exception.StackTrace));
            HasConnectedToServer = false;
            EventQueue.Enqueue(new Event { Type = EventType.DisConnect });
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="buf"></param>
    /// <returns></returns>
    protected object ParseCmd(byte[] buf)
    {
        var cmd = new Packet();
        cmd.ParseFrom(buf);
        return cmd;
    }

    #endregion

    #region �����¼�

    private void HandleEvent()
    {
        if (EventQueue.Count < 1)
        {
            return;
        }

        var e = EventQueue.Dequeue() as Event;
        if (e != null)
        {
            switch (e.Type)
            {
                case EventType.Connected:
                    Log(Name, string.Format("���Ϸ������ˣ��ϲ㴦���¼���{0}", DataTime2String(DateTime.Now)));
                    ConnectedToServer();
                    break;
                case EventType.CanNotConnect:
                    Log(Name, string.Format("�޷����ӷ��������ϲ㴦���¼���{0}", DataTime2String(DateTime.Now)));
                    CannotConnectToServer();
                    break;
                case EventType.DisConnect:
                    Log(Name, string.Format("�����ˣ��ϲ㴦���¼���{0}", DataTime2String(DateTime.Now)));
                    DisConnectToServer();
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// �����Ѿ��������������
    /// </summary>
    private void HandleReceiveCmds()
    {
        while (ReadQueue.Count > 0)
        {
            Log(Name, string.Format("���յ�����Ϣ�����ϲ㴦��ʱ�䣺{0}", DataTime2String(DateTime.Now)));
            Packet packet = ReadQueue.Dequeue() as Packet;

            if (packet != null)
            {
                HandleCmd(packet);
            }

        }
    }

    /// <summary>
    /// �����ȡ�������
    /// </summary>
    /// <param name="cmd"></param>
    private void HandleCmd(Packet cmd)
    {
        // TODO ���ܹ���

        if (_responsor == null)
        {
            return;
        }
        _responsor.Response(cmd);
    }

    /// <summary>
    /// �ӷ������Ͽ��ˡ�
    /// </summary>
    private void DisConnectToServer()
    {
        if (_networkManager == null)
        {
            return;
        }

        _networkManager.DisConnectToServer();
    }

    /// <summary>
    /// �ɹ������Ϸ�������
    /// </summary>
    private void ConnectedToServer()
    {
        if (_networkManager == null)
        {
            return;
        }
        _networkManager.ConnectedToServer();
    }

    /// <summary>
    /// �޷����ӷ�������
    /// </summary>
    private void CannotConnectToServer()
    {
        if (_networkManager == null)
        {
            return;
        }
        _networkManager.CannotConnectToServer();
    }

    #endregion

    /// <summary>
    /// ÿ�����50֡��
    /// </summary>
    private void FixedUpdate()
    {
        HandleEvent();

        if (!HasConnectedToServer) return;

        Write();
        Read();
        HandleReceiveCmds();
        HeartBeat();
    }

    public void Log(string tag, string content)
    {
        if (!ShowLog) return;
        Debug.Log(tag + "��" + content);
    }

    public void LogWarning(string tag, string content)
    {
        if (!ShowLog) return;
        Debug.LogWarning(tag + "��" + content);
    }

    public void LogError(string tag, string content)
    {
        if (!ShowLog) return;
        Debug.LogError(tag + "��" + content);
    }

    /// <summary>
    /// DateTime --> string 2013-08-02 19:50:24:345
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    protected string DataTime2String(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss:fff");
    }
}



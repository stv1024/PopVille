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
    /// 单例方法。
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

    #region 自定义类和enum

    /// <summary>
    /// 事件类型
    /// </summary>
    private enum EventType
    {

        /// <summary>
        /// 连上服务器了
        /// </summary>
        Connected,

        /// <summary>
        /// 无法连上服务器
        /// </summary>
        CanNotConnect,

        /// <summary>
        /// 与服务器断开连接了（一般是心跳超时，或者发送和接收的时候session断掉了）
        /// </summary>
        DisConnect
    }

    /// <summary>
    /// 事件（主要就是以上3中事件类型）
    /// </summary>
    private class Event
    {
        public EventType Type { get; set; }
    }

    /// <summary>
    /// 异步接收的数据。
    /// </summary>
    private class RsvData
    {
        /// <summary>
        /// header 一个4个字节的数据
        /// </summary>
        public byte[] Header;

        /// <summary>
        /// 数据缓冲区域
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// 想要接收的数据长度。
        /// </summary>
        public int DataLength;

        /// <summary>
        /// 当前数据保存的偏移量。比如：需要1024，已经获取了800，则Offset==800。
        /// </summary>
        public int Offset;

        /// <summary>
        /// socket数据流。
        /// </summary>
        public NetworkStream Stream;
    }

    #endregion

    #region 属性

    /// <summary>
    /// 名称
    /// </summary>
    protected string Name { get; set; }

    /// <summary>
    /// 客户端是否连上服务器
    /// </summary>
    private bool HasConnectedToServer { get; set; }

    private TcpClient Client { get; set; }

    /// <summary>
    /// 服务器地址
    /// </summary>
    private string Host { get; set; }

    /// <summary>
    /// 服务器端口号
    /// </summary>
    private int Port { get; set; }

    /// <summary>
    /// 开始连接服务器的时间，只有第一次初始化的时候设置，其余不能设置，主要是在连接超时的判断
    /// </summary>
    private DateTime ConnectStartTime { get; set; }

    /// <summary>
    /// 是否允许连接超时，若允许则连接异步完成时候进行检测，否则不进行检测继续尝试连接服务器
    /// </summary>
    protected bool EnableConnectTimeOut { get; set; }

    /// <summary>
    /// 连接服务器超时时间（前提是设置EnableConnectTimeOut为true）,单位：秒
    /// </summary>
    protected int ConnectTimeOut { get; set; }

    public DateTime LastClientHeartBeat { get; set; }

    public DateTime LastServerHeartBeat { get; set; }

    /// <summary>
    /// 是否允许心跳
    /// </summary>
    protected bool EnableHeartBeat { get; set; }

    /// <summary>
    /// 心跳检测时间（当与服务器最后一次通信记录现在时间大于多少秒后，主动发送心跳）
    /// </summary>
    protected int HeartBeatInterval { get; set; }

    /// <summary>
    /// 心跳超时时间（与服务器最后一次通信时间与现在大于多少秒后，认为是短线了）
    /// </summary>
    protected int HeartBeatTimeOut { get; set; }

    /// <summary>
    /// 写消息队列
    /// </summary>
    private Queue WriteQueue { get; set; }

    /// <summary>
    /// 读取消息队列
    /// </summary>
    private Queue ReadQueue { get; set; }

    /// <summary>
    /// 事件队列
    /// </summary>
    private Queue EventQueue { get; set; }

    /// <summary>
    /// 异步线程是否在写命令
    /// </summary>
    private bool IsWriting { get; set; }

    /// <summary>
    /// 异步线程是否在读取命令
    /// </summary>
    private bool IsReading { get; set; }

    /// <summary>
    /// 是否显示log。
    /// </summary>
    private bool ShowLog { get { return false; } }// set; }

    protected int MaxReadCmdNumberPerFrame { get; set; }

    private int TmpReadCmdNumber { get; set; }

    #endregion

    //TODO:MonoBehaviour要构造器何用
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
    /// 初始化和连接服务器
    /// </summary>
    /// <param name="host">服务器地址</param>
    /// <param name="port">服务器端口号</param>
    public void InitAndConnect(string host, int port, Responder responsor, NetworkManager networkManager)
    {
        Log(Name, string.Format("初始化客户端，服务器地址：{0}，服务器端口号：{1},时间:{2}", host, port, DataTime2String(DateTime.Now)));

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
    /// 关闭连接。
    /// </summary>
    public void Close()
    {
        Log(Name, string.Format("关闭客户端，时间：{0}", DataTime2String(DateTime.Now)));
        if (Client != null)
        {
            Client.Close();
        }
    }

    #region 连接服务器

    /// <summary>
    /// 连接服务器。
    /// </summary>
    private void Connect()
    {
        Log(Name, string.Format("开始连接服务器，时间:{0}", DataTime2String(DateTime.Now)));
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
    /// 异步的连接过程完成了。
    /// </summary>
    /// <param name="result"></param>
    private void FinishConnect(IAsyncResult result)
    {
        Log(Name, string.Format("结束连接服务器，时间：{0}", DataTime2String(DateTime.Now)));

        var client = result.AsyncState as TcpClient;

        if (EnableConnectTimeOut)
        {
            if (DateTime.Now.Ticks - ConnectStartTime.Ticks > ConnectTimeOut * 1000 * 10000)
            {
                LogError(Name, "连接超时，无法连上服务器");
                EventQueue.Enqueue(new Event { Type = EventType.CanNotConnect });
                return;
            }
        }

        if (client == null)
        {
            LogError(Name, "TcpClient == null 连接失败！");
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
                Log(Name, string.Format("连上服务器了，时间：{0}", DataTime2String(DateTime.Now)));
                HasConnectedToServer = true;
                LastClientHeartBeat = DateTime.Now;
                LastServerHeartBeat = DateTime.Now;
                IsWriting = false;
                IsReading = false;
                EventQueue.Enqueue(new Event { Type = EventType.Connected });
            }
            else
            {
                LogError(Name, "本次连接失败，等3秒，我再尝试连接");
                Thread.Sleep(3 * 1000);
                Connect();
            }
        }
    }

    #endregion

    #region 心跳

    private void HeartBeat()
    {
        if (EnableHeartBeat)
        {
            if (DateTime.Now.Ticks - LastServerHeartBeat.Ticks > HeartBeatTimeOut * 1000 * 10000)
            {
                LogError(Name, string.Format("掉线了，时间：{0}", DataTime2String(DateTime.Now)));
                HasConnectedToServer = false;
                EventQueue.Enqueue(new Event { Type = EventType.DisConnect });
                return;
            }
            if (DateTime.Now.Ticks - LastClientHeartBeat.Ticks > HeartBeatInterval * 1000 * 10000) //心跳时间到了
            {
                Log(Name, string.Format("发送心跳，时间：{0}", DataTime2String(DateTime.Now)));
                var hb = BuildHeartBeatCmd();
                WriteCmd(hb);
                LastClientHeartBeat = DateTime.Now;
            }
        }
    }

    /// <summary>
    /// 构建心跳消息。
    /// </summary>
    /// <returns></returns>
    protected object BuildHeartBeatCmd()
    {
        // todo 构建心跳消息。
        var cmd = new HBReq {SerialId = DateTime.Now.Ticks};
        var packet = new Packet(0) {Content = cmd.GetProtoBufferBytes()};
        return packet;
    }


    #endregion

    #region 发送消息

    public void WriteCmd(object obj)
    {
        Log(Name, string.Format("向消息队列添加一个消息，时间：{0}", DataTime2String(DateTime.Now)));
        WriteQueue.Enqueue(obj);
    }

    /// <summary>
    /// 执行实际的写操作。
    /// </summary>
    private void Write()
    {
        if (WriteQueue.Count < 1)
        {
            return;
        }

        if (IsWriting)
        {
            LogWarning(Name, string.Format("异步线程正在写消息,{0}", DataTime2String(DateTime.Now)));
            return;
        }

        StartWrite();
    }

    /// <summary>
    /// 开始异步写消息。
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
            Log(Name, string.Format("开始发送消息，时间：{0}", DataTime2String(DateTime.Now)));
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
    /// 获取命令的bytes。
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected byte[] BuildData(object obj)
    {
        var cmd = obj as Packet;
        return cmd.GetProtoBufferBytes();
    }

    /// <summary>
    /// 结束写操作。
    /// </summary>
    /// <param name="result"></param>
    private void FinishWrite(IAsyncResult result)
    {
        Log(Name, string.Format("发送消息完成，时间：{0}", DataTime2String(DateTime.Now)));
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

    #region 接收消息

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

            Log(Name, string.Format("接收到消息，时间：{0}", DataTime2String(DateTime.Now)));
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
                        string.Format("接收完消息，时间：{0}，第  {1} 个命令", DataTime2String(DateTime.Now), TmpReadCmdNumber));
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
                        LogWarning(Name, string.Format("这一帧不再接收命令了,时间：{0}", DateTime.Now));
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
    /// 解析命令。
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

    #region 处理事件

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
                    Log(Name, string.Format("连上服务器了，上层处理事件，{0}", DataTime2String(DateTime.Now)));
                    ConnectedToServer();
                    break;
                case EventType.CanNotConnect:
                    Log(Name, string.Format("无法连接服务器，上层处理事件，{0}", DataTime2String(DateTime.Now)));
                    CannotConnectToServer();
                    break;
                case EventType.DisConnect:
                    Log(Name, string.Format("掉线了，上层处理事件，{0}", DataTime2String(DateTime.Now)));
                    DisConnectToServer();
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 处理已经解析出来的命令。
    /// </summary>
    private void HandleReceiveCmds()
    {
        while (ReadQueue.Count > 0)
        {
            Log(Name, string.Format("将收到得消息丢给上层处理，时间：{0}", DataTime2String(DateTime.Now)));
            Packet packet = ReadQueue.Dequeue() as Packet;

            if (packet != null)
            {
                HandleCmd(packet);
            }

        }
    }

    /// <summary>
    /// 处理获取到的命令。
    /// </summary>
    /// <param name="cmd"></param>
    private void HandleCmd(Packet cmd)
    {
        // TODO 解密工作

        if (_responsor == null)
        {
            return;
        }
        _responsor.Response(cmd);
    }

    /// <summary>
    /// 从服务器断开了。
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
    /// 成功连接上服务器。
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
    /// 无法连接服务器。
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
    /// 每秒更新50帧。
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
        Debug.Log(tag + "：" + content);
    }

    public void LogWarning(string tag, string content)
    {
        if (!ShowLog) return;
        Debug.LogWarning(tag + "：" + content);
    }

    public void LogError(string tag, string content)
    {
        if (!ShowLog) return;
        Debug.LogError(tag + "：" + content);
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



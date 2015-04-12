using System.Collections.Generic;
using Assets.Sanxiao;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;

public class NetworkManager
{

    private NetworkManager()
    {

    }

    private static NetworkManager _instance;

    public static NetworkManager Instance
    {
        get { return _instance ?? (_instance = new NetworkManager()); }
    }

    public interface INetworkEventListener
    {
        void ConnectedToServer();
        void CannotConnectToServer();
        void DisConnectToServer();
    }

    private readonly List<INetworkEventListener> _listeners = new List<INetworkEventListener>();

    public void AddListener(INetworkEventListener listener)
    {
        if (!_listeners.Contains(listener)) _listeners.Add(listener);
    }

    public void RemoveListener(INetworkEventListener listener)
    {
        _listeners.RemoveAll(x => ReferenceEquals(x, listener));
    }

    public void ConnectedToServer()
    {
        foreach (var networkEventListener in _listeners)
        {
            networkEventListener.ConnectedToServer();
        }
    }

    public void CannotConnectToServer()
    {
        foreach (var networkEventListener in _listeners)
        {
            networkEventListener.CannotConnectToServer();
        }
    }

    public void DisConnectToServer()
    {
        foreach (var networkEventListener in _listeners)
        {
            networkEventListener.DisConnectToServer();
        }
    }
}
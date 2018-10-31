namespace Tel.Egram.Components.Application
{
    public enum ConnectionState
    {
        Connecting = 0,
        ConnectingToProxy = 1,
        Ready = 2,
        Updating = 3,
        WaitingForNetwork = 4
    }
}

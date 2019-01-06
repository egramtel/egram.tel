namespace Tel.Egram.Model.Application
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

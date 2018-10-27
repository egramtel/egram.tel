using PropertyChanged;
using TdLib;

namespace Tel.Egram.Models.Settings.Connection
{
    [AddINotifyPropertyChangedInterface]
    public class ProxyModel
    {
        public TdApi.Proxy Proxy { get; set; }
        
        public bool IsSocks5 { get; set; }
        
        public bool IsHttp { get; set; }
        
        public bool IsMtProto { get; set; }
        
        public bool IsUsernameInputVisible { get; set; }
        
        public bool IsPasswordInputVisible { get; set; }
        
        public bool IsSecretInputVisible { get; set; }
        
        public string Server { get; set; }
        
        public string Port { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public string Secret { get; set; }

        public static ProxyModel FromProxy(TdApi.Proxy proxy)
        {
            var model = new ProxyModel
            {
                Proxy = proxy
            };

            model.Server = proxy.Server;
            model.Port = proxy.Port == 0 ? "" : proxy.Port.ToString();
            
            switch (proxy.Type)
            {
                case TdApi.ProxyType.ProxyTypeSocks5 socks5:
                    model.IsSocks5 = true;
                    model.Username = socks5.Username;
                    model.Password = socks5.Password;
                    break;
                
                case TdApi.ProxyType.ProxyTypeHttp http:
                    model.IsHttp = true;
                    model.Username = http.Username;
                    model.Password = http.Password;
                    break;
                
                case TdApi.ProxyType.ProxyTypeMtproto mtproto:
                    model.IsMtProto = true;
                    model.Secret = mtproto.Secret;
                    break;
            }
            
            model.IsUsernameInputVisible = model.IsSocks5 || model.IsHttp;
            model.IsPasswordInputVisible = model.IsSocks5 || model.IsHttp;
            model.IsSecretInputVisible = model.IsMtProto;
            
            return model;
        }

        public TdApi.Proxy ToProxy()
        {
            var proxy = new TdApi.Proxy
            {
                Id = Proxy?.Id ?? 0,
                IsEnabled = Proxy?.IsEnabled ?? false,
                LastUsedDate = Proxy?.LastUsedDate ?? 0
            };

            int.TryParse(Port, out var port);
            proxy.Port = port;
            proxy.Server = Server;

            if (IsSocks5)
            {
                proxy.Type = new TdApi.ProxyType.ProxyTypeSocks5
                {
                    Username = Username,
                    Password = Password
                };
            }

            if (IsHttp)
            {
                proxy.Type = new TdApi.ProxyType.ProxyTypeHttp
                {
                    Username = Username,
                    Password = Password
                };
            }

            if (IsMtProto)
            {
                proxy.Type = new TdApi.ProxyType.ProxyTypeMtproto
                {
                    Secret = Secret
                };
            }
            
            return proxy;
        }
    }
}
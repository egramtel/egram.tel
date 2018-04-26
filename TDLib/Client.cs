using System;
using TD.Bindings;

namespace TD
{
    public class Client : IDisposable
    {
        private IntPtr _handle;
        
        public Client()
        {
            _handle = Interop.ClientCreate();
        }

        public void Send(string data)
        {
            using (var ctx = Interop.StringToIntPtr(data))
            {
                Interop.ClientSend(_handle, ctx.Ptr);
            }
        }

        public string Receive(double timeout)
        {
            var res = Interop.ClientReceive(_handle, timeout);
            return Interop.IntPtrToString(res);
        }

        public string Execute(string data)
        {
            using (var ctx = Interop.StringToIntPtr(data))
            {
                var res = Interop.ClientExecute(_handle, ctx.Ptr);
                return Interop.IntPtrToString(res);
            }
        }
        
        private void Destroy()
        {
            Interop.ClientDestroy(_handle);
            _handle = IntPtr.Zero;
        }

        public void Dispose()
        {
            Destroy();
            GC.SuppressFinalize(this);
        }

        ~Client()
        {
            Destroy();
        }
        
        public static class Log
        {
            public static bool SetFilePath(string path)
            {
                using (var ctx = Interop.StringToIntPtr(path))
                {
                    return Interop.SetLogFilePath(ctx.Ptr) != 0;
                }
            }

            public static void SetMaxFileSize(long size)
            {
                Interop.SetLogFileMaxSize(size);
            }

            public static void SetVerbosityLevel(int level)
            {
                Interop.SetLogVerbosityLevel(level);
            }

            public static void SetFatalErrorCallback(Action<string> callback)
            {
                Interop.SetLogFatalErrorCallback(ptr =>
                {
                    var str = Interop.IntPtrToString(ptr);
                    callback(str);
                });
            }
        }
    }
}
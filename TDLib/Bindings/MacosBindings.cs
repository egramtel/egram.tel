using System;
using System.Runtime.InteropServices;

namespace TD.Bindings
{
    internal static class MacosBindings
    {
        private const string Dll = "libtdjson.dylib";
        
        [DllImport(Dll)]
        internal static extern IntPtr td_json_client_create();
        
        [DllImport(Dll)]
        internal static extern void td_json_client_destroy(IntPtr handle);
        
        [DllImport(Dll)]
        internal static extern void td_json_client_send(IntPtr handle, IntPtr data);
        
        [DllImport(Dll)]
        internal static extern IntPtr td_json_client_receive(IntPtr handle, double t);
        
        [DllImport(Dll)]
        internal static extern IntPtr td_json_client_execute(IntPtr handle, IntPtr data);
        
        [DllImport(Dll)]
        internal static extern int td_set_log_file_path(IntPtr path);
        
        [DllImport(Dll)]
        internal static extern void td_set_log_max_file_size(long size);
        
        [DllImport(Dll)]
        internal static extern void td_set_log_verbosity_level(int level);
        
        [DllImport(Dll)]
        internal static extern void td_set_log_fatal_error_callback([MarshalAs(UnmanagedType.FunctionPtr)]Callback callback);
    }
}
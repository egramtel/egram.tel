using System;
using System.Collections.Generic;
using ReactiveUI;

namespace Egram.Components.I18N
{
    public class I18N : ReactiveObject
    {
        private string _text;
        public string Text
        {
            get => _text;
            private set => this.RaiseAndSetIfChanged(ref _text, value);
        }
        
        private I18N()
        {
            
        }
        
        public class Manager : ReactiveObject
        {
            private Dictionary<string, WeakReference<I18N>> _references;
            
            private Manager()
            {
                _references = new Dictionary<string, WeakReference<I18N>>();
            }
            
            public static Manager Instance => lazy.Value;
            private static readonly Lazy<Manager> lazy = new Lazy<Manager>(() => new Manager());

            public I18N Get(string format, params object[] args)
            {
                lock (_references)
                {
                    WeakReference<I18N> reference;
                    I18N i18n;

                    var key = format + "\n" + string.Join("\n", args);
                    
                    if (_references.TryGetValue(key, out reference))
                    {
                        if (reference.TryGetTarget(out i18n))
                        {
                            return i18n;
                        }

                        _references.Remove(key);
                    }
                
                    i18n = new I18N();
                    reference = new WeakReference<I18N>(i18n);
                    
                    _references.Add(key, reference);
                    
                    return i18n;
                }
            }
        }
    }
}
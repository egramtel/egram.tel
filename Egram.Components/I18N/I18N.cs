using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using ReactiveUI;

namespace Egram.Components.I18N
{
    public class I18N : ReactiveObject
    {   
        private I18N(string key, object[] args, string text)
        {
            _key = key;
            _args = args;
            _text = text;
        }
        
        private string _key;
        public string Key
        {
            get => _key;
            private set => this.RaiseAndSetIfChanged(ref _key, value);
        }
        
        private object[] _args;
        public object[] Args
        {
            get => _args;
            private set => this.RaiseAndSetIfChanged(ref _args, value);
        }
        
        private string _text;
        public string Text
        {
            get => _text;
            private set => this.RaiseAndSetIfChanged(ref _text, value);
        }
        
        public class Manager
        {
            private CultureInfo _cultureInfo;
            private Dictionary<string, List<WeakReference<I18N>>> _references;
            private Dictionary<string, string> _strings;
            
            public static Manager Instance => lazy.Value;
            private static readonly Lazy<Manager> lazy = new Lazy<Manager>(() => new Manager());

            private Manager()
            {
                _cultureInfo = CultureInfo.InvariantCulture;
                _references = new Dictionary<string, List<WeakReference<I18N>>>();
                _strings = LoadStrings(CultureInfo.InvariantCulture);
            }

            public I18N Get(string key, params object[] args)
            {
                lock (_references)
                {
                    var i18n = new I18N(key, args, string.Format(_strings[key], args));
                    var reference = new WeakReference<I18N>(i18n);

                    if (!_references.TryGetValue(key, out var references))
                    {
                        references = new List<WeakReference<I18N>>();
                        _references.Add(key, references);
                    }
                    else
                    {
                        references.RemoveAll(r => !r.TryGetTarget(out var _));
                    }
                    
                    references.Add(reference);
                    
                    return i18n;
                }
            }

            public void SetCulture(CultureInfo cultureInfo)
            {
                lock (_references)
                {
                    _strings = LoadStrings(cultureInfo);
                    _cultureInfo = cultureInfo;
                    
                    var keys = _references.Keys;
                    foreach (var key in keys)
                    {
                        var references = _references[key];
                        var format = _strings[key];

                        for (int i = 0; i < references.Count; i++)
                        {
                            var reference = references[i];
                            
                            if (reference.TryGetTarget(out var i18n))
                            {
                                i18n.Text = string.Format(format, i18n.Args);
                            }
                            else
                            {
                                references.Remove(reference);
                            }
                        }
                    }
                }
            }

            private Dictionary<string, string> LoadStrings(CultureInfo cultureInfo)
            {
                using (var stream = GetType().Assembly.GetManifestResourceStream("Egram.Components.I18N.i18n.xml"))
                {
                    var document = XDocument.Load(stream);
                
                    var culture = document.Root.Elements("culture")
                        .FirstOrDefault(e => e.Attribute("culture").Value == cultureInfo.Name);

                    var strings = culture.Elements("string")
                        .ToDictionary(e => e.Attribute("key").Value, e => e.Value);

                    return strings;
                }
            }

            private string GetFootprint(string key, object[] args)
            {
                return args != null && args.Length > 0
                    ? $"{key}\t{string.Join("\t", args)}"
                    : key;
            }
        }
    }
}
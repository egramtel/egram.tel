using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Tel.Egram.Services.Persistance.Resources;

namespace Tel.Egram.Services.Persistance
{
    public class ResourceManager : IResourceManager
    {
        private readonly Assembly _assembly;
        private readonly string _prefix;

        public ResourceManager(Assembly assembly)
        {
            _assembly = assembly;
            _prefix = assembly.GetName().Name + ".Resources.";
        }
        
        public IList<PhoneCode> GetPhoneCodes()
        {
            using (var stream = _assembly.GetManifestResourceStream(_prefix + "PhoneCodes.txt"))
            using (var reader = new StreamReader(stream))
            {
                var codes = new List<PhoneCode>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var code = new PhoneCode();
                        var parts = line.Split(';');
                        
                        if (parts.Length > 0)
                        {
                            code.Code = parts[0];
                        }

                        if (parts.Length > 1)
                        {
                            code.CountryCode = parts[1];
                        }

                        if (parts.Length > 2)
                        {
                            code.CountryName = parts[2];
                        }

                        if (parts.Length > 3)
                        {
                            code.Mask = parts[3];
                        }
                        
                        codes.Add(code);
                    }
                }

                return codes;
            }
        }
    }
}
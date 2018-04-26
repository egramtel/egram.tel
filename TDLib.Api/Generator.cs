using System;
using System.Collections.Generic;
using System.Linq;

namespace TD
{
    class Generator
    {
        static List<TDType> _types = new List<TDType>();
        static List<TDFunc> _funcs = new List<TDFunc>();
        
        static void Main(string[] args)
        {
            GenerateTypes();
            GenerateFuncs();
        }
        
        private static string GetFileName(string str)
        {
            var arr = str.ToCharArray();
            arr[0] = arr[0].ToString().ToUpper()[0];
            return new string(arr);
        }

        private static void GenerateTypes()
        {
            var lines = System.IO.File.ReadAllLines("./types.tl")
                .Where(l => !l.StartsWith("//"))
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => l.Replace(":bytes ", ":vector<byte> "))
                .ToArray();
            
            foreach (var line in lines)
            {
                ParseType(line);
            }

            var builtins = new []
            {
                "bool",
                "byte",
                "int",
                "long",
                "Int64",
                "double?",
                "string"
            };
            
            foreach (var type in _types)
            {
                if (builtins.Contains(type.Name))
                {
                    continue;
                }
                
                if (!type.Name.Contains("<") && !type.Super)
                {
                    var str = type.Generate();
                    System.IO.File.WriteAllText("./Types/"+GetFileName(type.Name)+".cs", str);
                }
            }
        }

        private static void GenerateFuncs()
        {
            var lines = System.IO.File.ReadAllLines("./methods.tl")
                .Where(l => !l.StartsWith("//"))
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => l.Replace(":bytes ", ":vector<byte> "))
                .ToArray();
            
            foreach (var line in lines)
            {
                ParseFunc(line);
            }

            foreach (var func in _funcs)
            {
                var str = func.Generate();
                System.IO.File.WriteAllText("./Methods/"+GetFileName(func.Name)+".cs", str);
            }
        }

        private static TDType ParseType(string line)
        {
            var parts = line.Split(new[] {" = ", " "}, StringSplitOptions.RemoveEmptyEntries);

            var type = GetType(GetName(parts));
            type.Fields = GetFields(parts);
            type.Base = GetType(GetUnion(parts));
            type.Base.Super = true;
            
            return type;
        }

        private static TDFunc ParseFunc(string line)
        {
            var parts = line.Split(new[] {" = ", " "}, StringSplitOptions.RemoveEmptyEntries);
            
            var func = GetFunc(GetName(parts));
            func.Args = GetFields(parts);
            func.Result = GetType(GetUnion(parts));
            func.Result.Super = true;

            return func;
        }

        private static string GetName(string[] parts)
        {
            return GetName(parts[0]);
        }

        private static string GetName(string part)
        {
            switch (part)
            {
                case "Bool":
                    return "bool";
                case "byte":
                    return "byte";
                case "int32":
                    return "int";
                case "int53":
                    return "long";
                case "int64":
                    return "Int64";
                case "double":
                    return "double?";
            }
            
            return part
                .Replace("<Bool>", "<bool>")
                .Replace("<byte>", "<byte>")
                .Replace("<int32>", "<int>")
                .Replace("<int53>", "<long>")
                .Replace("<int64>", "<Int64>")
                .Replace("<double>", "<double>");
        }

        private static string GetUnion(string[] parts)
        {
            return parts[parts.Length - 1].Replace(";", "");
        }

        private static TDField[] GetFields(string[] parts)
        {
            var list = new List<TDField>();
            
            for (int i = 1; i < parts.Length - 1; i++)
            {
                list.Add(GetField(parts[i]));
            }

            return list.ToArray();
        }

        private static TDField GetField(string str)
        {
            var parts = str.Split(':');

            return new TDField
            {
                Name = parts[0],
                Type = GetType(parts[1])
            };
        }

        private static TDType GetType(string str)
        {
            TDType type;
            int begin = str.IndexOf('<');
            int end = str.LastIndexOf('>');
            
            if (begin >= 0)
            {
                var s = str.Substring(begin + 1, end - begin - 1);

                type = _types.ToList().FirstOrDefault(t => t.Name == str && t.Generic == GetType(s));
                if (type == null)
                {
                    type = new TDType
                    {
                        Name = GetName(str),
                        Fields = new TDField[0],
                        Generic = GetType(s)
                    };
                    _types.Add(type);
                }
            }
            else
            {
                type = _types.ToList().FirstOrDefault(t => t.Name == str);
                if (type == null)
                {
                    type = new TDType
                    {
                        Name = GetName(str),
                        Fields = new TDField[0]
                    };
                    _types.Add(type);
                }
            }

            return type;
        }

        private static TDFunc GetFunc(string str)
        {
            var func = new TDFunc
            {
                Name = str
            };
            _funcs.Add(func);

            return func;
        }
    }
}
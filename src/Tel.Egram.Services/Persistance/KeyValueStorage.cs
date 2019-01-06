using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Tel.Egram.Services.Persistance.Entities;

namespace Tel.Egram.Services.Persistance
{
    public class KeyValueStorage : IKeyValueStorage
    {
        private readonly DatabaseContext _db;

        public KeyValueStorage(DatabaseContext db)
        {
            _db = db;
        }

        public void Set<T>(string key, T value)
        {
            var entity = _db.Values.FirstOrDefault(v => v.Key == key);
            var obj = Serialize(value);
            
            if (entity != null)
            {
                entity.Value = obj;
                _db.Values.Update(entity);
            }
            else
            {
                _db.Values.Add(new KeyValueEntity
                {
                    Key = key,
                    Value = obj
                });
            }
            
            _db.SaveChanges();
        }

        public T Get<T>(string key)
        {
            var entity = _db.Values.AsNoTracking().FirstOrDefault(v => v.Key == key);

            if (entity == null)
            {
                throw new NullReferenceException($"Value for key '{key}' is not set");
            }
            
            return Deserialize<T>(entity.Value);
        }

        public IList<KeyValuePair<string, T>> GetAll<T>()
        {
            return _db.Values.AsNoTracking()
                .Select(v => new KeyValuePair<string, T>(v.Key, Deserialize<T>(v.Value)))
                .ToList();
        }

        public bool TryGet<T>(string key, out T value)
        {
            var entity = _db.Values.AsNoTracking().FirstOrDefault(v => v.Key == key);

            if (entity == null)
            {
                value = default(T);
                return false;
            }

            value = Deserialize<T>(entity.Value);
            return true;
        }

        public void Delete(string key)
        {
            var entity = _db.Values.FirstOrDefault(v => v.Key == key);
            
            if (entity != null)
            {
                _db.Values.Remove(entity);
                _db.SaveChanges();
            }
        }

        private string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj);
        
        private T Deserialize<T>(string v) => v != null
            ? JsonConvert.DeserializeObject<T>(v)
            : default(T);
    }
}
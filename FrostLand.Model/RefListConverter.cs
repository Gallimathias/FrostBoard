using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace FrostLand.Model
{
    public class RefListConverter : JsonConverter
    {
        private readonly Type collectionType;

        public RefListConverter()
        {

        }
        public RefListConverter(Type collectionType)
        {
            this.collectionType = collectionType;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ICollection).IsAssignableFrom(objectType) &&
                objectType.GetProperties().Where(p => p.GetCustomAttributes<KeyAttribute>().Any()).Any();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var list = serializer.Deserialize<List<Dictionary<string, object>>>(reader);
            
            Type itemType = null;

            if (existingValue == null)
            {
                var type = collectionType ?? objectType;

                if (type.IsInterface || type.IsAbstract)
                    existingValue = new List<object>();
                else
                    existingValue = Activator.CreateInstance(type);

                if (type.GenericTypeArguments.Length > 0)
                    itemType = type.GenericTypeArguments[0];

            }

            Dictionary<string, PropertyInfo> props = null;

            foreach (var itemDictionary in list)
            {
                if (itemType == null)
                    itemType = Type.GetType((string)itemDictionary["type"]);

                if (props == null)
                    props = itemType.GetProperties().ToDictionary(k => k.Name);

                object item = Activator.CreateInstance(itemType);
                foreach (var prop in itemDictionary)
                {
                    var info = props[prop.Key];
                    var value = prop.Value;

                    if (info.PropertyType == typeof(int))
                        value = Convert.ToInt32(prop.Value);
                    else if (info.PropertyType == typeof(short))
                        value = Convert.ToInt16(prop.Value);                        

                    info.SetValue(item, value);
                }

                existingValue.GetType().GetRuntimeMethod("Add", new[] { itemType }).Invoke(existingValue, new[] { item });
            }

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var collection = (IEnumerable)value;
            var list = new List<object>();

            PropertyInfo[] keys = null;

            foreach (var item in collection)
            {
                var tmpDic = new Dictionary<string, object>();

                if (keys == null)
                {
                    keys = item.GetType()
                               .GetProperties()
                               .Select(p => new { Property = p, Attribute = p.GetCustomAttribute<KeyAttribute>() })
                               .Where(p => p.Attribute != null)
                               .Select(p => p.Property).ToArray();
                }

                foreach (var key in keys)
                {
                    tmpDic.Add(key.Name, key.GetValue(item));
                }

                list.Add(tmpDic);
            }

            serializer.Serialize(writer, list);
        }
    }
}

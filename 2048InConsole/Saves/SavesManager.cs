using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace _2048InConsole.Saves
{
    internal class SavesManager<T> : ISavesManager<T>
    {
        public bool TrySave(string filePath, T item, IEnumerable<Type> knownTypes)
        {
            if (item == null) return false;

            try
            {
                using (FileStream stream = File.Create(filePath))
                {
                    DataContractSerializer serializer;

                    var enumerable = knownTypes as Type[] ?? knownTypes.ToArray();
                    if (enumerable.Any())
                    {
                        serializer = new DataContractSerializer(typeof(T), new DataContractSerializerSettings
                        {
                            KnownTypes = enumerable
                        });
                    }
                    else
                    {
                        serializer = new DataContractSerializer(typeof(T));
                    }

                    serializer.WriteObject(stream, item);
                    return true;
                }
            }
            catch (SerializationException) { }
            catch (ArgumentException) { }
            catch (DirectoryNotFoundException) { }
            catch (PathTooLongException) { }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            catch (NotSupportedException) { }

            return false;
        }

        public bool TryLoad(string filePath, IEnumerable<Type> knownTypes, out T item)
        {
            item = default(T);

            if (!File.Exists(filePath)) return false;

            try
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    DataContractSerializer serializer;

                    var enumerable = knownTypes as Type[] ?? knownTypes.ToArray();
                    if (enumerable.Any())
                    {
                        serializer = new DataContractSerializer(typeof(T), new DataContractSerializerSettings
                        {
                            KnownTypes = enumerable
                        });
                    }
                    else
                    {
                        serializer = new DataContractSerializer(typeof(T));
                    }

                    item = (T)serializer.ReadObject(stream);
                    return true;
                }
            }
            catch (SerializationException) { }
            catch (XmlException) { }
            catch (ArgumentException) { }
            catch (DirectoryNotFoundException) { }
            catch (PathTooLongException) { }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            catch (NotSupportedException) { }

            return false;
        }
    }
}

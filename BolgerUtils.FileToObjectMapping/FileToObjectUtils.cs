using System;
using System.Collections.Generic;
using System.IO;

namespace BolgerUtils.FileToObjectMapping
{
    public sealed class FileToObject
    {
        private readonly Dictionary<string, Func<string, object>> _fileContentToObjectDictionary =
            new Dictionary<string, Func<string, object>>();
        private readonly Dictionary<string, (FileInfo fileInfo, object value)> _fileInfoAndValueDictionary =
            new Dictionary<string, (FileInfo, object)>();

        public void Clear()
        {
            _fileInfoAndValueDictionary.Clear();
            _fileContentToObjectDictionary.Clear();
        }

        private string GetKey(string path) => GetKey(path.ToFileInfo());

        private string GetKey(FileInfo fileInfo) => fileInfo.FullName.ToLower();

        public bool IsRegistered(string path) => _fileContentToObjectDictionary.ContainsKey(GetKey(path));

        public T Load<T>(string path) => (T) Map(path, _fileContentToObjectDictionary[GetKey(path)]);

        public T Map<T>(string path, Func<string, T> fileContentToObject)
        {
            var fileInfo = path.ToFileInfo();
            if(!fileInfo.Exists)
                throw new FileNotFoundException(null, path);

            var key = GetKey(fileInfo);

            // ReSharper disable once InvertIf
            if(!_fileInfoAndValueDictionary.TryGetValue(key, out var tuple) ||
                tuple.fileInfo.LastWriteTimeUtc != fileInfo.LastWriteTimeUtc)
                // Add new key+value OR update existing value.
                _fileInfoAndValueDictionary[key] = (fileInfo, fileContentToObject(File.ReadAllText(fileInfo.FullName)));

            return (T) _fileInfoAndValueDictionary[key].value;
        }

        public void Register<T>(string path, Func<string, T> fileContentToObject) where T : class =>
            _fileContentToObjectDictionary.Add(GetKey(path), fileContentToObject);

        public bool UnMap(string path) => _fileInfoAndValueDictionary.Remove(GetKey(path));

        public bool UnRegister(string path)
        {
            UnMap(path);

            return _fileContentToObjectDictionary.Remove(GetKey(path));
        }
    }

    public static class FileToObjectUtils
    {
        public static FileToObject FileToObject { get; } = new FileToObject();

        public static bool IsRegistered(string path) => FileToObject.IsRegistered(path);

        public static T Load<T>(string path) => FileToObject.Load<T>(path);

        public static T Map<T>(string path, Func<string, T> fileContentToObject) =>
            FileToObject.Map(path, fileContentToObject);

        public static void Register<T>(string path, Func<string, T> fileContentToObject) where T : class =>
            FileToObject.Register(path, fileContentToObject);

        public static bool UnMap(string path) => FileToObject.UnMap(path);
        public static bool UnRegister(string path) => FileToObject.UnRegister(path);
    }
}

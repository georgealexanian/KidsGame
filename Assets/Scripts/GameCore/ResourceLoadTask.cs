using System.Threading.Tasks;
using UnityEngine;

namespace GameCore
{
    public class ResourceLoadTask<T> where T : Object
    {
        private readonly string _path;
        private Object _loadResource;
        private bool _resivedRequest;

        public ResourceLoadTask(string path)
        {
            _path = path;
        }

        public async Task<T> LoadAsync()
        {
            var _request = Resources.LoadAsync<T>(_path);
            var countIteration = 0;
            var maxIterationCount = 10000;
            do
            {
                await Task.Delay(10);
                countIteration++;
            } while (!_request.isDone || countIteration > maxIterationCount);

            if (_request.asset == null)
            {
                Debug.LogWarning($"Asset of type {typeof(T)}  cant load by path {_path}!");
            }

            return (T)_request.asset;
        }

        public static async Task<T> LoadAsync(string path)
        {
            return await new ResourceLoadTask<T>(path).LoadAsync();
        }
    }
}
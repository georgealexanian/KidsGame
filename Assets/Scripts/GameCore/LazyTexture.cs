#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace GameCore
{
    public struct LazyTexture
    {
        private Texture _texture;
        private readonly string _path;
        public Texture Get => _texture ? _texture : _texture = AssetDatabase.LoadAssetAtPath<Texture>(_path);

        public LazyTexture(string path)
        {
            _path = path;
            _texture = null;
        }
    }
}
#endif

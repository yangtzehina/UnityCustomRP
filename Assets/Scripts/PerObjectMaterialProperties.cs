using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PerObjectMaterialProperties:MonoBehaviour
    {
        private static int _baseColorId = Shader.PropertyToID("_BaseColor");
        [SerializeField] private Color baseColor = Color.white;
        private static MaterialPropertyBlock _block;

        private void Awake()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            if (_block == null)
            {
                _block = new MaterialPropertyBlock();
            }
            _block.SetColor(_baseColorId,baseColor);
            GetComponent<Renderer>().SetPropertyBlock(_block);
        }
    }
}
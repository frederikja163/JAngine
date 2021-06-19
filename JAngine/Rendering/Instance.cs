using System;
using JAngine.Rendering.LowLevel;

namespace JAngine.Rendering
{
    public class Instance<TData> : IDisposable
        where TData : unmanaged, IVertex
    {
        private readonly ShapeDefinition<TData> _shapeDefinition;
        private readonly int _index;
        private TData _data;
        private bool _isDisposed = false;

        public TData Data
        {
            get => _data;
            set
            {
                _data = value;
                _shapeDefinition.Update(_index, _data);
            }
        }


        public Instance(ShapeDefinition<TData> shapeDefinition, TData data)
        {
            _data = data;
            _shapeDefinition = shapeDefinition;
            _index = _shapeDefinition.Add(this);
        }
        
        public void Dispose()
        {
            if (_isDisposed)
            {
                throw new Exception("This object has already been disposed!");
            }

            _isDisposed = true;
            _shapeDefinition.Remove(_index);
        }
    }
}
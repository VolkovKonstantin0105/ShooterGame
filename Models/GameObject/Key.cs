using System.Numerics;

namespace SGame.Models.GameObject
{
    public class Key : AbstarctObject
    {
        protected override Vector2 _currentPoint { get; set; } = new Vector2(280, 375);
        private bool _isFound = false;
        public bool IsFound
        { 
            get => _isFound;
            set => value = _isFound;
        }
        public Key() {}
    }
}

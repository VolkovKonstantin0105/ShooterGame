using Microsoft.Xna.Framework;
using SGame.Models.Enemy;
using System;
using System.Collections.Generic;
using System.Text;

namespace SGame.GameLogic
{
    public class Poison
    {
        private float _time;
        private float _timer;
        private bool _isActive;
        private bool _isStarted;

        public bool IsActive => _isActive;

        public void Start()
        {
            if (_isStarted)
                return;

            _time = 5f;
            _timer = 0f;
            _isActive = true;
            _isStarted = true;
        }

        public void Update(GameTime gameTime, Player player, int damage)
        {
            if (!_isActive) 
                return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _time -= dt;
            _timer += dt;

            if (_timer >= 1f)
            {
                player.TakeDamage(damage);
                player.UpdateDamageAnimation(gameTime);
                _timer = 0f;
            }

            if (_time <= 0)
            {
                _isActive = false;
                _isStarted = false;
            }
        }
    }
}

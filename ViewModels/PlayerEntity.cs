using System;
using System.Collections.Generic;
using System.Text;

namespace SGame.ViewModels
{
    public sealed class PlayerEntity
    {
        public PlayerEntity() { }
        public int Id { get; set; }  // Первичный ключ
        public int Score { get; set; } //количество собранных монет
        public int CountKills { get; set; } //количество киллов 

    }
}

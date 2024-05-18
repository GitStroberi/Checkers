using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Checkers.MVVM.Model
{
    public class SaveData
    {
        [JsonPropertyName("Board")]
        public Board Board { get; set; }

        [JsonPropertyName("PlayerTurn")]
        public bool PlayerTurn { get; set; }

        [JsonPropertyName("PlayerMoved")]
        public bool PlayerMoved { get; set; }

        [JsonPropertyName("PlayerJumped")]
        public bool PlayerJumped { get; set; }

        [JsonPropertyName("JumpsEnabled")]
        public bool JumpsEnabled { get; set; }

        [JsonPropertyName("GameOver")]
        public bool GameOver { get; set; }

        [JsonPropertyName("RedPieces")]
        public int RedPieces { get; set; }

        [JsonPropertyName("WhitePieces")]
        public int WhitePieces { get; set; }
    }
}

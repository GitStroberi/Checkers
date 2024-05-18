using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Checkers.MVVM.Model
{
    public class WinsData
    {
        [JsonPropertyName("RedWins")]
        public int RedWins { get; set; }
        [JsonPropertyName("WhiteWins")]
        public int WhiteWins { get; set; }
        [JsonPropertyName("RecordPieces")]
        public int RecordPieces { get; set; }
    }
}

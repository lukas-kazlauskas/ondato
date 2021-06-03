using System.ComponentModel.DataAnnotations;

namespace Ondato.Data
{
    public class KeyValuePair
    {
        [Key] [DataType("nvarchar(max)")] public string KeyJson { get; set; }

        [DataType("nvarchar(max)")] public string ValueJson { get; set; }
    }
}
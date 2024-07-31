using System.Text;
using System.Text.Json.Serialization;

namespace HamsterCrack.Dto;

[Serializable]
public sealed record CipherStatus(
    [property: JsonPropertyName("cipher")] string EncodedCipher,
    [property: JsonPropertyName("isClaimed")]
    bool IsClaimed)
{
    public string DecodedCipher
    {
        get
        {
            var decodedCipher = EncodedCipher[..3] + EncodedCipher[4..];
            var decodedCipherBytes = Convert.FromBase64String(decodedCipher);
            return Encoding.UTF8.GetString(decodedCipherBytes);
        }
    }
}
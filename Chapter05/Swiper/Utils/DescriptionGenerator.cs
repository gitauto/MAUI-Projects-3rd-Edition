namespace Swiper.Utils;

internal class DescriptionGenerator
{
    private readonly string[] _adjectives = ["nice", "horrible", "great", "terribly old", "brand new"];
    private readonly string[] _other = ["picture of grandpa", "car", "photo of a forest", "duck"];
    private static readonly Random random = new();

    public string Generate()
    {
        var a = _adjectives[random.Next(_adjectives.Length)];
        var b = _other[random.Next(_other.Length)];
        return $"A {a} {b}";
    }
}

public static class FuzzDataGenerating
{
    private static readonly Random random = new(42); // change for new random generation!

    public static IEnumerable<object[]> Timestamps()
    {
        yield return new object[] { -1L };
        yield return new object[] { 0L };
        yield return new object[] { int.MaxValue };
        yield return new object[] { int.MinValue };
        yield return new object[] { (long)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 1000000000) };
        yield return new object[] { random.NextInt64(-10000000000, 10000000000) };
    }

    public static IEnumerable<object[]> Strings()
    {
        yield return new object[] { "" };
        yield return new object[] { " " };
        yield return new object[] { null! };
        yield return new object[] { "💬🐦✨" };
        yield return new object[] { new string('A', 5000) };
        yield return new object[] { "\t\n\r" };
        yield return new object[] { Guid.NewGuid().ToString() };
    }
}

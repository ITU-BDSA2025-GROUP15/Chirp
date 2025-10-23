public static class FuzzData
{
    private static readonly Random random = new(42); // change for new random generation!

    public static IEnumerable<object[]> Timestamps()
    {
        //test time before unixtime
        yield return new object[] { -1L };
        //test base time
        yield return new object[] { 0L };
        //test max and min values
        yield return new object[] { int.MaxValue };
        yield return new object[] { int.MinValue };
        //test random time
        yield return new object[] { random.NextInt64(-10000000000, 10000000000) };
    }

    public static IEnumerable<object[]> Strings()
    {
        //test space
        yield return new object[] { "" };
        yield return new object[] { " " };
        //test null
        yield return new object[] { null! };
        //test emoji 
        yield return new object[] { "🍆🌚👌🏿" };
        //test long text
        yield return new object[] { new string('A', 5000) };
        //test invis char
        yield return new object[] { "\t\n\r" };
        //test random string
        yield return new object[] { Guid.NewGuid().ToString() };
    }
}

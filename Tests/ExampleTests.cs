using Domain.Domain;

namespace Tests;
public class Tests : TestBase
{
    [SetUp]
    public void Setup() {}

    [Test]
    public void Test1()
    {
        var expected = "Bench press";
        var item = GetDto<Exercise>("benchPress");
        Assert.That(item.Name, Is.EqualTo(expected));
    }
}
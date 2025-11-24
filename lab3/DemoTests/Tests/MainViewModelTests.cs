using NUnit.Framework;
using DemoWpf.ViewModels;

namespace DemoTests.Tests;

[TestFixture]
public class MainViewModelTests
{
    [Test]
    public void FullName_Updates_OnFirstOrLastChange()
    {
        var vm = new MainViewModel();
        vm.FirstName = "Anna";
        vm.LastName = "Nowak";
        Assert.That(vm.FullName, Is.EqualTo("Anna Nowak"));
    }
}

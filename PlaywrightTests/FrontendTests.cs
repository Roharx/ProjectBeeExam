using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    //Run with debugger: PWDEBUG=1 dotnet test
    [Test]
    public async Task CanFieldBeFound()
    {
        await Page.GotoAsync("https://demo.playwright.dev/todomvc/");
        var inputField = Page.GetByPlaceholder("What needs to be done?");
        await Expect(inputField).ToBeVisibleAsync();
        await Expect(inputField).ToBeEditableAsync();
        await Expect(inputField).ToBeEmptyAsync();
    }
    [Test]
    public async Task WrongUsernameAndPassword()
    {
        await Page.GotoAsync("http://localhost:4200/login-component");

        await Page.Locator("#username-bar").ClickAsync();

        await Page.Locator("#username-bar").FillAsync("asd");

        await Page.Locator("#password-bar").ClickAsync();

        await Page.Locator("#password-bar").FillAsync("qwe");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();

        await Expect(Page.GetByText("Wrong username or password!")).ToBeVisibleAsync();
    }

    [Test]
    public async Task RightUsernameAndPassword()
    {
        await Page.GotoAsync("http://localhost:4200/login-component");

        await Page.Locator("#username-bar").ClickAsync();

        await Page.Locator("#username-bar").FillAsync("Bob the manager");

        await Page.Locator("#username-bar").PressAsync("Enter");

        await Page.Locator("#password-bar").FillAsync("asdqwe");

        await Page.Locator("#password-bar").PressAsync("Enter");

        await Expect(Page.GetByText("Bob the manager")).ToBeVisibleAsync();

        await Expect(Page.GetByText("Log out")).ToBeVisibleAsync();
    }

    [Test]
    public async Task CreateFieldCorrectly()
    {
        await Page.GotoAsync("http://localhost:4200/login-component");

        await Page.Locator("#username-bar").ClickAsync();

        await Page.Locator("#username-bar").FillAsync("Bob the manager");

        await Page.Locator("#username-bar").PressAsync("Enter");

        await Page.Locator("#password-bar").FillAsync("asdqwe");

        await Page.Locator("#password-bar").PressAsync("Enter");

        await Page.GetByRole(AriaRole.Button, new() { Name = "+" }).ClickAsync();

        await Page.GetByLabel("Field Name:").ClickAsync();

        await Page.GetByLabel("Field Name:").FillAsync("playwright frontend test field");

        await Page.GetByLabel("Field Location:").ClickAsync();

        await Page.GetByLabel("Field Location:").FillAsync("playwright");

        await Page.GetByText("Select Manager").ClickAsync();

        await Page.GetByRole(AriaRole.Radio, new() { Name = "Bob the manager" }).ClickAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "OK" }).ClickAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Accept" }).ClickAsync();

        await Expect(Page.Locator("div").Filter(new() { HasText = "playwright frontend test field" }).Nth(2)).ToBeVisibleAsync();
    }
}
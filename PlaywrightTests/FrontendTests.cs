using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    private IPage Page;

    private static readonly string BaseUrl =
        Environment.GetEnvironmentVariable("TEST_BASE_URL") ?? "http://localhost:4200";

    private const string _rightUsername = "Bob the manager";
    private const string _rightPassword = "asdqwe";
    private const string _rightManager = "Bob the manager";

    [SetUp]
    public async Task SetUp()
    {
        var browser = await Playwright.Chromium.LaunchAsync();
        var context = await browser.NewContextAsync();
        Page = await context.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await Page.CloseAsync();
    }

    [Test]
    public async Task WrongUsernameAndPassword()
    {
        //go to login page
        await Page.GotoAsync($"{BaseUrl}/login-component");

        //find the field, click and fill out
        async Task ClickAndFillAsync(string locator, string text)
        {
            await Page.Locator(locator).ClickAsync();
            await Page.Locator(locator).FillAsync(text);
        }

        //fill out the username and password fields with wrong credentials
        await ClickAndFillAsync("#username-bar", "asd");
        await ClickAndFillAsync("#password-bar", "qwe");
        
        //click login
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();

        //expect a wrong username or password error message
        await Expect(Page.GetByText("Wrong username or password!")).ToBeVisibleAsync();
    }

    [Test]
    public async Task RightUsernameAndPassword()
    {
        //go to login page
        await Page.GotoAsync($"{BaseUrl}/login-component");

        async Task FillAndPressEnterAsync(string locator, string text)
        {
            await Page.Locator(locator).ClickAsync();
            await Page.Locator(locator).FillAsync(text);
            await Page.Locator(locator).PressAsync("Enter");
        }

        //fill out the username and password fields with the right credentials
        await FillAndPressEnterAsync("#username-bar", _rightUsername);
        await FillAndPressEnterAsync("#password-bar", _rightPassword);

        //after logging in, expect to see the username and a log out button/text
        await Expect(Page.GetByText("Bob the manager")).ToBeVisibleAsync();
        await Expect(Page.GetByText("Log out")).ToBeVisibleAsync();
    }

    [Test]
    public async Task RightUsernameAndPasswordEnterOnly()
    {
        //go to login page
        await Page.GotoAsync($"{BaseUrl}/login-component");
        //click the username field
        await Page.Locator("#username-bar").ClickAsync();

        //fill and press enter only
        async Task FillAndPressEnterAsync(string locator, string text)
        {
            await Page.Locator(locator).FillAsync(text);
            await Page.Locator(locator).PressAsync("Enter");
        }

        //fill out the username and password fields with the right credentials
        await FillAndPressEnterAsync("#username-bar", _rightUsername);
        await FillAndPressEnterAsync("#password-bar", _rightPassword);

        //after logging in, expect to see the username and a log out button/text
        await Expect(Page.GetByText("Bob the manager")).ToBeVisibleAsync();
        await Expect(Page.GetByText("Log out")).ToBeVisibleAsync();
    }

    [Test]
    public async Task CreateFieldCorrectly()
    {
        #region login
        //go to login page
        await Page.GotoAsync($"{BaseUrl}/login-component");

        //find the field, click, fill out and press "Enter"
        async Task FillAndPressEnterAsync(string locator, string text)
        {
            await Page.Locator(locator).ClickAsync();
            await Page.Locator(locator).FillAsync(text);
            await Page.Locator(locator).PressAsync("Enter");
        }
        
        
        //fill out the username and password fields with the right credentials
        await FillAndPressEnterAsync("#username-bar", _rightUsername);
        await FillAndPressEnterAsync("#password-bar", _rightPassword);
        #endregion
        
        //click the add field button
        await Page.Locator("#add-button").ClickAsync();
        
        //locate, click and fill out the field name text field
        await Page.GetByLabel("Field Name:").ClickAsync();
        await Page.GetByLabel("Field Name:").FillAsync("playwright frontend test field");
        
        //locate, click and fill out the field location text field
        await Page.GetByLabel("Field Location:").ClickAsync();
        await Page.GetByLabel("Field Location:").FillAsync("playwright");
        
        //locate and click the select manager button
        await Page.GetByText("Select Manager").ClickAsync();
        
        //select the right manager in the ion-select menu and click ok
        await Page.GetByRole(AriaRole.Radio, new() { Name = _rightManager }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "OK" }).ClickAsync();
        
        //locate and click the accept button to finalize the field creation
        await Page.GetByRole(AriaRole.Button, new() { Name = "Accept" }).ClickAsync();
        
        //expect to see the field that has the test name ("playwright frontend test field") in it
        await Expect(Page.GetByText("playwright frontend test field")).ToBeVisibleAsync();
        
        //cleanup:
        //locate the field that has the test name and click the "..."(edit) button on it
        await Page.GetByText("playwright frontend test field").GetByRole(AriaRole.Button, new() { Name = "..." }).ClickAsync();
        //locate the remove button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Remove" }).ClickAsync();
        //on the popup, locate the accept button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Accept" }).ClickAsync();
        //expect a success message
        await Expect(Page.GetByText("Successfully removed field.")).ToBeVisibleAsync();
    }
}
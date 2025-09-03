using Casbin;
using Xunit;

namespace HRAuthorization.Tests;

public class AuthorizationTests
{
    private readonly Enforcer _enforcer;

    public AuthorizationTests()
    {
        var root = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..");
        var model = Path.Combine(root, "CasbinModel", "rbac_with_domains.conf");
        var policy = Path.Combine(root, "CasbinPolicy", "policy.csv");
        _enforcer = new Enforcer(model, policy);
    }

    [Theory]
    [InlineData("alice", "line-1")]
    [InlineData("alice", "line-2")]
    public async Task HRAdmin_Can_Read_And_Approve(string user, string dom)
    {
        Assert.True(await _enforcer.EnforceAsync(user, dom, "api.leave", "read"));
        Assert.True(await _enforcer.EnforceAsync(user, dom, "api.leave", "approve"));
    }

    [Theory]
    [InlineData("bob", "line-1")]
    [InlineData("bob", "line-2")]
    public async Task HRManager_Can_Read_And_Approve(string user, string dom)
    {
        Assert.True(await _enforcer.EnforceAsync(user, dom, "api.leave", "read"));
        Assert.True(await _enforcer.EnforceAsync(user, dom, "api.leave", "approve"));
    }

    [Theory]
    [InlineData("cathy", "line-1")]
    [InlineData("cathy", "line-2")]
    public async Task HRUser_Can_Read_But_Not_Approve(string user, string dom)
    {
        Assert.True(await _enforcer.EnforceAsync(user, dom, "api.leave", "read"));
        Assert.False(await _enforcer.EnforceAsync(user, dom, "api.leave", "approve"));
    }

    [Theory]
    [InlineData("dave", "line-1")]
    [InlineData("dave", "line-2")]
    public async Task Auditor_Can_Read_But_Not_Approve(string user, string dom)
    {
        Assert.True(await _enforcer.EnforceAsync(user, dom, "api.leave", "read"));
        Assert.False(await _enforcer.EnforceAsync(user, dom, "api.leave", "approve"));
    }
}

// Startup.cs
builder.Services.AddSingleton<CircuitHandler>(x=> new JidelakCircuitHandler(x.GetRequiredService<ILogger<JidelakCircuitHandler>>()));

// MyCircuitHandler.cs
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace MyCms.Services;

public class MyCircuitHandler : CircuitHandler
{
    private readonly List<string> _ids = new();
    private readonly ILogger<MyCircuitHandler> _logger;

    public MyCircuitHandler(ILogger<MyCircuitHandler> logger)
    {
        _logger = logger;
    }

    public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _ids.Add(circuit.Id);
        _logger.LogInformation($"Created circuit. Total {_ids.Count} circuits active.");

        // vhodne misto pro praci s auth
        _authenticationStateProvider.AuthenticationStateChanged += AuthenticationStateProviderOnAuthenticationStateChanged;

        return base.OnCircuitOpenedAsync(circuit, cancellationToken);
    }

    public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _ids.Remove(circuit.Id);
        _logger.LogInformation($"Deleted circuit. Total {_ids.Count} circuits active.");

        return base.OnCircuitClosedAsync(circuit, cancellationToken);
    }

    private void AuthenticationStateProviderOnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        _ = Update(task);

        async Task Update(Task<AuthenticationState> task)
        {
            try
            {
                var state = await task;
                _currentUser.User = state.User;
            }
            catch
            {
            }
        }
    }
}
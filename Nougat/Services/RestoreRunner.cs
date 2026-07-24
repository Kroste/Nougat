using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nougat.Models;

namespace Nougat.Services;

/// <summary>
/// Fuehrt "dotnet restore" fuer das Anker-Projekt aus - portable + je RID.
/// Live-Ausgabe wird an das UI weitergereicht.
/// </summary>
public sealed class RestoreRunner
{
    private readonly ProcessRunner _processRunner;
    private readonly ILogger<RestoreRunner> _logger;

    public RestoreRunner(ProcessRunner processRunner, ILogger<RestoreRunner> logger)
    {
        _processRunner = processRunner;
        _logger = logger;
    }

    public async Task<bool> RestoreAsync(
        string dotnetPath,
        string anchorPath,
        string packagesDirectory,
        IEnumerable<string> targetRids,
        Action<ProgressLine>? onLine,
        CancellationToken ct = default)
    {
        // Portable Restore
        _logger.LogInformation("Restore portable ...");
        var portable = await _processRunner.RunAsync(
            dotnetPath,
            ["restore", anchorPath, "--packages", packagesDirectory, "--verbosity", "minimal"],
            onLine, ct: ct
        ).ConfigureAwait(false);
        if (portable.ExitCode != 0)
        {
            _logger.LogError("Portable restore fehlgeschlagen (ExitCode {Code})", portable.ExitCode);
            return false;
        }

        // Je RID
        foreach (var rid in targetRids)
        {
            _logger.LogInformation("Restore fuer RID {Rid} ...", rid);
            var ridResult = await _processRunner.RunAsync(
                dotnetPath,
                ["restore", anchorPath, "--packages", packagesDirectory, "--runtime", rid, "--verbosity", "minimal"],
                onLine, ct: ct
            ).ConfigureAwait(false);
            if (ridResult.ExitCode != 0)
            {
                _logger.LogError("Restore fuer RID {Rid} fehlgeschlagen (ExitCode {Code})", rid, ridResult.ExitCode);
                return false;
            }
        }

        return true;
    }
}

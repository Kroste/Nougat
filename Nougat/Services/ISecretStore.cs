namespace Nougat.Services;

/// <summary>Ver-/Entschluesselt Secrets fuer die inline-Persistierung in settings.json.</summary>
public interface ISecretStore
{
    /// <summary>Verschluesselt Klartext, gibt "ENC1:&lt;base64&gt;" zurueck (leer wenn null/leer).</summary>
    string? Protect(string? plaintext);

    /// <summary>Entschluesselt "ENC1:..."-Wert; leer/null bleibt leer/null.</summary>
    string? Unprotect(string? encrypted);
}

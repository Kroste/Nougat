namespace Nougat.Models;

public enum ProcessStream { StdOut, StdErr }

/// <summary>Eine Zeile aus stdout/stderr eines gestarteten Prozesses.</summary>
public sealed record ProgressLine(ProcessStream Stream, string Line);

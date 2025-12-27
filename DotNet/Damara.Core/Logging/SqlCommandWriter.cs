// <copyright file="SqlCommandWriter.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.Logging;

/// <summary>
/// SQL Command writer.
/// </summary>
public class SqlCommandWriter : TextWriter
{
    /// <summary>
    /// The DML command type.
    /// </summary>
    public const string DmlCommandType = "DmlCommandType";

    private const string StatementTerminator = "-- Context";
    private readonly ILogger logger;
    private StringBuilder stringBuilder;
    private string dmlCommand;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlCommandWriter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public SqlCommandWriter(ILogger logger)
    {
        this.logger = logger;
        this.stringBuilder = new StringBuilder();
    }

    /// <summary>
    /// Gets the encoding. When overridden in a derived class, returns the character encoding in which the output is written.
    /// </summary>
    public override Encoding Encoding => Encoding.Default;

    /// <summary>
    /// Writes a string to the text stream.
    /// </summary>
    /// <param name="value">The string to write.</param>
    public override void Write(string value)
    {
        if (this.stringBuilder.Length == 0 && string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        if (this.stringBuilder.Length == 0)
        {
            this.dmlCommand = value.Split(' ')[0];
        }

        this.stringBuilder.Append(value);

        if (value.StartsWith(StatementTerminator))
        {
            this.logger
                .ForContext(DmlCommandType, this.dmlCommand)
                .Debug(this.stringBuilder.ToString());
            this.stringBuilder = new StringBuilder();
        }
    }
}
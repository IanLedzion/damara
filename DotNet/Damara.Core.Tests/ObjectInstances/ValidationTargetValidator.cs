// <copyright file="ValidationTargetValidator.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using FluentValidation;

namespace Damara.Core.Tests.ObjectInstances;

/// <summary>
/// Target validator.
/// </summary>
public class ValidationTargetValidator : AbstractValidator<ValidationTarget>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationTargetValidator"/> class.
    /// </summary>
    public ValidationTargetValidator()
    {
        this.RuleFor(name => name.Name)
            .NotNull()
            .Must(name => name == null)
            .WithErrorCode("Entity_Error_Message");
    }
}

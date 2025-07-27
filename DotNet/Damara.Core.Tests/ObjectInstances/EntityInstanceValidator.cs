// <copyright file="EntityInstanceValidator.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using FluentValidation;

namespace Damara.Core.Tests.ObjectInstances;

/// <summary>
/// Provides tests for the <see cref="EntityInstance" /> class.
/// </summary>
/// <seealso cref="Damara.EntityValidator{EntityInstance}" />
public class EntityInstanceValidator : ValidatorBase<EntityInstance, MockUnitOfWork>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityInstanceValidator"/> class.
    /// </summary>
    public EntityInstanceValidator(MockUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
        this.RuleFor(e => e.Name)
            .NotEqual("Invalid");

        this.RuleFor(e => e.Status)
            .Custom((e, context) =>
            {
                var target = context.InstanceToValidate;

                switch (target.Status)
                {
                    case "Error":
                        context.AddError("Error Status");
                        break;

                    case "Warning":
                        context.AddWarning("Warning Status");
                        break;

                    case "Info":
                        context.AddInfo("Info Status");
                        break;

                    default:
                        break;
                }
            });
    }
}

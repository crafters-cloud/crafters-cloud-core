﻿using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.Core.EntityFramework.Seeding;

/// <summary>
/// Base interface for seeding data into the database using Entity Framework migrations.
/// </summary>
[PublicAPI]
public interface ISeeding
{
    void Seed(ModelBuilder modelBuilder);
}
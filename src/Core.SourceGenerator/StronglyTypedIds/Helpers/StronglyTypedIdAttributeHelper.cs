﻿namespace CraftersCloud.Core.SourceGenerator.StronglyTypedIds.Helpers;

public static class StronglyTypedIdAttributeHelper
{
    public const string AttributeName = "StronglyTypedIdAttribute";
    public const string AttributeNamespace = "CraftersCloud.Core.StronglyTypedIds";
    
    public const string ValueKindEnumName = "ValueKind";
    public const string ValueKindEnumText = $$"""
                                          // <auto-generated />
                                          
                                          #pragma warning disable 1591
                                          
                                          namespace {{AttributeNamespace}}
                                          {
                                              public enum ValueKind
                                              {
                                                  Guid = 0,
                                                  Int = 1,
                                              }
                                          }
                                          """;

    public const string AttributeText = $$"""
                                          // <auto-generated />
                                          using System;

                                          #pragma warning disable 1591

                                          namespace {{AttributeNamespace}}
                                          {
                                              [AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
                                              internal sealed class {{AttributeName}}(ValueKind valueKind) : Attribute
                                              {
                                                  public ValueKind ValueKind { get; } = valueKind;
                                              }
                                          }

                                          """;
}
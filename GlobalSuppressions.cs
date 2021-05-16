// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

// Disable these one by one
[assembly: SuppressMessage("Style", "CA1062: Argument Null exception", Justification = "Argument Null exception", Scope = "module")]
[assembly: SuppressMessage("Style", "CA1801: Parameter never used", Justification = "Parameter never used", Scope = "module")]
[assembly: SuppressMessage("Style", "CA2227: Readonly by removing the property setter", Justification = "Readonly by removing the property setter", Scope = "module")]
[assembly: SuppressMessage("Style", "CA1716: Reserved keyword Shared", Justification = "Reserved keyword Shared", Scope = "module")]
[assembly: SuppressMessage("Style", "CA1822: Can be static as it can does not access static data", Justification = "Can be static as it can does not access static data", Scope = "module")]
[assembly: SuppressMessage("Style", "CA1056: Change the type of property to System.Uri", Justification = "Change the type of property to System.Uri", Scope = "module")]
[assembly: SuppressMessage("Style", "SA1600: Element should be documented", Justification = "Element should be documented", Scope = "module")]
[assembly: SuppressMessage("Style", "S1135: Suspicious Comment", Justification = "To be fixed at a later stage", Scope = "module")]
[assembly: SuppressMessage("Style", "SA1615: Element return value should be documented", Justification = "Element return value should be documented", Scope = "module")]
[assembly: SuppressMessage("Style", "SA1602: Enumeration items should be documented", Justification = "Element should be documented", Scope = "module")]

// Valid Suppressions
[assembly: SuppressMessage("Style", "CA1416: This call site is reachable on all platforms.", Justification = "This will be executed only in windows", Scope = "module")]
[assembly: SuppressMessage("Style", "CA1031: Specific exception", Justification = "Specific exception", Scope = "module")]
[assembly: SuppressMessage("Style", "CA1034: Do not nest type or change accessibility", Justification = "Do not nest type or change accessibility", Scope = "module")]
[assembly: SuppressMessage("Style", "CA1303: Resource table instead of literal string", Justification = "Resource table instead of literal string", Scope = "module")]
[assembly: SuppressMessage("Style", "CA1305: Format provider for ToString", Justification = "Format provider for ToString", Scope = "module")]
[assembly: SuppressMessage("Style", "CA1710: End in collection", Justification = "End in collection", Scope = "module")]
[assembly: SuppressMessage("Style", "CA1716: Reserved keyword Shared", Justification = "Reserved keyword Shared", Scope = "module")]
[assembly: SuppressMessage("Style", "CA2007: Consider calling ConfigureAwait on the awaited task", Justification = "Consider calling ConfigureAwait on the awaited task", Scope = "module")]
[assembly: SuppressMessage("Style", "SA0001: XML comment analysis is disabled due to project configuration", Justification = "XML Comment is skipped", Scope = "module")]
[assembly: SuppressMessage("Style", "SA1009: Closing parenthesis preceded by space", Justification = "Decided by team to override", Scope = "module")]
[assembly: SuppressMessage("Style", "SA1101: Prefix local calls with this", Justification = "Skipping explicit usage of 'this' keyword", Scope = "module")]
[assembly: SuppressMessage("Style", "SA1111: Closing parenthesis should be on last line", Justification = "Decided by team to override", Scope = "module")]
[assembly: SuppressMessage("Style", "SA1116: The params should begin on the line after the declaration", Justification = "The params should begin on the line after the declaration", Scope = "module")]
[assembly: SuppressMessage("Style", "SA1128: Put constructor initializers on their own line", Justification = "Put constructor initializers on their own line", Scope = "module")]
[assembly: SuppressMessage("Style", "SA1200: Using directive should appear within a namespace declaration", Justification = "Using directive should appear within a namespace declaration", Scope = "module")]
[assembly: SuppressMessage("Style", "SA1201: A constructor should not follow a property", Justification = "A constructor should not follow a property", Scope = "module")]
[assembly: SuppressMessage("Style", "SA1413: Use trailing comma in multi-line initializers", Justification = "Use trailing comma in multi-line initializers", Scope = "module")]
[assembly: SuppressMessage("Style", "SA1502: Element should not be on a single line", Justification = "Element should not be on a single line", Scope = "module")]
[assembly: SuppressMessage("Style", "SA1512: Single line comments should not be followed by blank line", Justification = "Single line comments should not be followed by blank line", Scope = "module")]
[assembly: SuppressMessage("Style", "SA1513: Closing brace should be followed by blank line", Justification = "Closing brace should be followed by blank line", Scope = "module")]
[assembly: SuppressMessage("Style", "SA1633: The file header is missing or not located at the top of the file", Justification = "The file header is missing or not located at the top of the file", Scope = "module")]
[assembly: SuppressMessage("Style", "CA1304: The behavior of 'string.ToLower()' could vary based on the current user's locale settings. ", Justification = "Not all locale are invariant", Scope = "module")]
[assembly: SuppressMessage("Style", "CA1018: Specify AttributeUsage on FromModelAttribute' could vary based on the current user's locale settings. ", Justification = "Not specifying attribute usage", Scope = "module")]
[assembly: SuppressMessage("Style", "SA1623: The property's documentation summary text should begin with: 'Gets or sets'", Justification = "Dto need not have this prefix in summary", Scope = "module")]

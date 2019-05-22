# Writing Allocation Free Code in C# samples

This repo contains the slides and example code from my "Writing Allocation Free Code in C#" talk. The talk covers why it is important to reduce allocations when possible (spoiler - for throughput), and demonstrates this with a look at "reference semantics for value types", which is a set of features in C# 7.3 for being able to handle value types (`struct`) as references, allowing very cheap allocation and cleanup of objects on the stack, without the cost of having to pass by value, and with the ability to modify the passed values (or make them immutable). More details of this feature can be found in the [Microsoft documentation](https://docs.microsoft.com/en-gb/dotnet/csharp/write-safe-efficient-code).

It also looks at `Span<T>` a type used to represent any block of contiguous memory and provide a type safe array-like API for accessing and modifying that data. This type allows for creating and manipulating allocation free slices of existing data, such as sub-strings. More details on `Span<T>` can be found in [this article](https://msdn.microsoft.com/en-us/magazine/mt814808.aspx).



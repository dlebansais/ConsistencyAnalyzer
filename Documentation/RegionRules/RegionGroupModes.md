# Region Consistency

Regions in C# are free tags used to expand or collapse code. However, within a class they can also be used to organize the code in groups ('regions'). This analyzer recognize different ways of grouping code.

## By interface category

In this mode, regions surround code belonging to the following categories:

+ Public interface (with the `public` keyword)
+ Descendant interface (with the `protected` keyword)
+ Everything else (private code)

In addition, the public interface can be split in additional categories:

+ Constructors
+ Properties
+ Events
+ Methods

Figuring out if this mode is used in a class is fairly easy, since each member falls into one category directly when it's declared. The simple grouping (with 3 categories) is compatible with the more complicated one (with 6) when a class only has public properties, or only public methods (typically static).

## By topic

In this mode, regions surround code specific to features the class implements. Since constructors are always needed regardless of how the object is used later, this mode recogne the following groups:

+ Constructors (public or private)
+ Topic #1
+ Topic #2
+ And so on...

This mode requires topics to be consistent within a hierarchy of classes. For example, if class `A` inherits from `B`, and `B` has methods belonging to topic #1, overrides in `A` are also expected to belong to topic #1.

## By interface implementation

If a class inherits from one or more interface, it can group code as follow:

+ Code belonging to the class itself
+ Implementation of interface Ixxx
+ Implementation of interface Iyyy
+ And so on...

A requirement will then be that all regions (except the first one) follow the same text pattern, with only the name of the interface changing. For instance:

````csharp
#region Implementation of IEnumerable
````

## Mixed mode

Finally, the analyzer recognizes a mixed mode where regions are used by interface category, or by topic (but not both at the same time), and also by interface implementation. This is typically used when the class inherits from `IDisposable`, a fairly common requirement when one of its field or property is also disposable. In this case, the class implementation of IDisposable can have its own region.
 
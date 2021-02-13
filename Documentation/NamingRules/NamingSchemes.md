# Naming Schemes

It is common to expect all names in a program to follow some naming convention. Typical conventions are MACRO_CASE, camelCase, snake_case, among many other.

## Categories

In C#, the following elements are expected to follow some convention, but not all elements should follow the same one.

+ Namespaces
+ Classes, records and structs
+ Enums
+ Interfaces
+ Enum members
+ Delegates
+ Events
+ Methods
+ Properties
+ Fields
+ Method parameters
+ Type parameters

When a name of a particular category of elements, **Methods** for instance, is not compliant with other names of that category, because a scheme has been inferred, the corresponding diagnostic is raised.

## Known formatting schemes

The analyzer recognizes the following schemes:

+ twowords
+ TWOWORDS
+ twoWords
+ TwoWords
+ two_words
+ TWO_WORDS
+ two_Words
+ Two_Words

Since the analyzer doesn't perform semantic analysis of names, it can sometimes fail to detect non-compliance. For example, in the list above, `twowords` is compatible with `twoWords` and will just be interpreted as a single word. However, `twoWords` and `two_Words` are not compatible.
    

# Using Placement and Ordering Rules

## Placement inside or outside namespace

In a program, `using` directives can be placed inside namespaces, or outside. When a majority is placed, say, inside, and some directives appear placed outside, they violate the rule.

For instance, if a majority of using directive are placed inside, the following code results in two violations of the rule.
 
````csharp
using System;
using System.Collections.Generic;

namespace ConsistencyAnalyzer
{
    using System.Threading;
}

````

In the example above, `using System;` and `using System.Collections.Generic;` violate the rule.

If, on the contrary, a majority of using directives in the program were placed outside namespaces, it would be the `using System.Threading;` directive that violates the rule. 

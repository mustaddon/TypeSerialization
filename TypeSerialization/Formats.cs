using System;
using System.Collections.Generic;
using System.Text;

namespace TypeSerialization;

public enum Formats
{
    /// <summary>example: "Dictionary(Int32-String)"</summary>
    UriSafe = 0,

    /// <summary>example: "Dictionary&lt;Int32,String&gt;"</summary>
    CodeLike = 1,
}

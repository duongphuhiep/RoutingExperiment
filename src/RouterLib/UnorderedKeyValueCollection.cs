using System.Collections.Generic;
using System.Collections.Specialized;

namespace Starfruit.RouterLib;

public class UnorderedKeyValueCollection : NameValueCollection
{
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var target = (UnorderedKeyValueCollection)obj;


        if (AllKeys.Length != target.AllKeys.Length)
        {
            return false;
        }

        foreach (var key in AllKeys)
        {
            var sourceValues = GetValues(key);
            var targetValues = target.GetValues(key);

            if (sourceValues?.Length != targetValues?.Length)
            {
                return false;
            }

            if (sourceValues is null)
            {
                continue;
            }

            var targetValuesSet = new HashSet<string>(targetValues);

            foreach (var value in sourceValues)
            {
                if (!targetValuesSet.Contains(value))
                {
                    return false;
                }
            }
        }

        return true;
    }
}

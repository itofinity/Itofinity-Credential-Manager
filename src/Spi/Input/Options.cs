using System.Collections.Generic;
using System.Linq;


namespace Spi.Input
{
    public class Options : Dictionary<string, IEnumerable<string>>
    {
        private IEnumerable<string> _defaultVals = new List<string>();
        public IEnumerable<string> ValuesOrDefault(string key, IEnumerable<string> defaultsVals = null)
        {
            if (this.TryGetValue(key, out IEnumerable<string> values))
            {
                return values;
            }

            return _defaultVals;
        }

        public string ValueOrDefault(string key, int index = 0, string defaultVal = null)
        {
            var vals = ValuesOrDefault(key).ToList();
            if(vals.Count() > index)
            {
                return vals[index];
            }
            
            return defaultVal;
        }
    }
}
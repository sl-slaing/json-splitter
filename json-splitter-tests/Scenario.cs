using json_splitter;
using System.Collections.Generic;

namespace json_splitter_tests
{
    public class Scenario
    {
        public RelatedJsonConfiguration Configuration { get; set; }
        public IEnumerable<string> NdJson { get; set; }
        public Dictionary<string, IEnumerable<string>> ExpectedFiles { get; set; } = new Dictionary<string, IEnumerable<string>>();
    }
}

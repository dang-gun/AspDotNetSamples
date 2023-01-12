using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPascalCase.Model
{
    public class TestModel
    {
        public int IntTest01 { get; set; }
        public string StringTest01 { get; set; }


        public int intTest02 { get; set; }
        public string stringTest02 { get; set; }

        [JsonProperty("TestInt03")]
        public int IntTest03 { get; set; }
        [JsonProperty("TestString03")]
        public string StringTest03 { get; set; }
    }
}

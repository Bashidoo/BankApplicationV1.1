using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BankApplication
{
    public class MyDB<T>
    {
        [JsonPropertyName("users")]
        public List<User<T>> AllUsersFromDB { get; set; }

    }
}

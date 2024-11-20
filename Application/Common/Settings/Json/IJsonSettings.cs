using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Settings.Json
{
    public interface IJsonSettings
    {
        public JsonSerializerOptions JsonSerializerOption { get; init; }
    }
}

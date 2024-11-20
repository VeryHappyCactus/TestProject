using Common.Settings.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Settings
{
    public interface IAppCommonSettings
    {
        public JsonSettings JsonSettings { get; init; }
    }
}

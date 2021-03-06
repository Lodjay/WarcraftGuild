﻿using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Configuration;
using WarcraftGuild.BlizzardApi.Interfaces;
using WarcraftGuild.BlizzardApi.Models;
using WarcraftGuild.Enums;
using WarcraftGuild.Exceptions;

namespace WarcraftGuild.BlizzardApi
{
    public interface IBlizzardApiReader
    {
        Task<T> GetAsync<T>(string query, Namespace? ns = null, string additionalParams = null);
        Task<string> GetJsonAsync(string query, Namespace? ns = null, string additionalParams = null);
        Task<string> SendTokenRequest();
    }
}
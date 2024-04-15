using System;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Snipper.server.Models;
using Snipper.server.Controllers;

namespace Snipper.server.Data
{
    public class SnippetInitializer
    {
        public static void Initialize(IWebHostEnvironment env)
        {
            var snippetsJson = File.ReadAllText(Path.Combine(env.ContentRootPath, "Data", "SeedData.json"));

            var snippetsFromJson = JsonConvert.DeserializeObject<List<Snippet>>(snippetsJson);

            SnippetsController.snippets.AddRange(snippetsFromJson);
            SnippetsController.uniqueId = SnippetsController.snippets.Count();
        }
    }
}

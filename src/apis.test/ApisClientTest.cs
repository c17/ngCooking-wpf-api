using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace apis
{
    public class ApisClientTest
    {
        apis.Client.ApiClient _apiClient;

        public ApisClientTest()
        {
            _apiClient = new apis.Client.ApiClient("http://localhost:5000/api");
        }

        [Fact]
        public async void RecettesGet()
        {
            List<apis.Client.Models.Recette> recettes = await _apiClient.Get<List<apis.Client.Models.Recette>>("recettes");

            Assert.NotEmpty(recettes);
        }
    }
}
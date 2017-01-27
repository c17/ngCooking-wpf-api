using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using System;

namespace apis.test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            apis.Client.ApiClient _apiClient = _apiClient = new apis.Client.ApiClient("http://localhost:5000/api");

            List<apis.Client.Models.Recette> recettes = _apiClient.Get<List<apis.Client.Models.Recette>>("recettes").Result;

            List<apis.Client.Models.Ingredient> ingredients = _apiClient.Get<List<apis.Client.Models.Ingredient>>("ingredients").Result;


            byte[] fileData = System.IO.File.ReadAllBytes("gateau.jpg");
            _apiClient.PostImage(
                "recettes/setimage",
                new Dictionary<String, String>() { { "id", "cake-jambon-olive" } },
                fileData,
                "rawPicture")
                .Wait();
        }
    }
}

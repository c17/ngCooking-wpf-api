
using System;
using System.Collections.Generic;
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

        [Fact]
        public async void IngredientsGet()
        {
            List<apis.Client.Models.Ingredient> ingredients = await _apiClient.Get<List<apis.Client.Models.Ingredient>>("ingredients");

            Assert.NotEmpty(ingredients);
        }


        [Fact]
        public async void CommentairePost()
        {
            apis.Client.Models.Commentaire newComment = new apis.Client.Models.Commentaire 
            {
                UserId = 1,
                RecetteId = "cake-jambon-olive",
                Title = "Un commentaire de test",
                Comment = "Le contenu du commentaire",
                Mark = 5
            };

            await _apiClient.Post<apis.Client.Models.Commentaire>("commentaires", newComment);
        }

        [Fact]
        public async void RecettePostNoImage()
        {
            apis.Client.Models.Recette newRecipe = new apis.Client.Models.Recette 
            {
                Name = "Délicieuse recette de choucroute",
                Ingredients = new List<String> { "Farine", "Banane" },
                Preparation = "Faire macérer le chou dans la bière",
                CreatorId = 1,
                Calories = 300
            };

            await _apiClient.Post<apis.Client.Models.Recette>("recettes", newRecipe);
        }

        [Fact]
        public async void RecetteSetImage()
        {
            byte[] fileData = System.IO.File.ReadAllBytes("gateau.jpg");

            bool result = await _apiClient.PostImage3(
                "recettes/setimage",
                new Dictionary<String, String>() { { "id", "cake-jambon-olive" } },
                fileData,
                "rawPicture");

            Assert.True(result);
        }
    }
}
﻿using Microsoft.AspNetCore.Hosting;
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

        [Fact]
        public async void IngredientsGet()
        {
            List<apis.Client.Models.Ingredient> ingredients = await _apiClient.Get<List<apis.Client.Models.Ingredient>>("ingredients");

            Assert.NotEmpty(ingredients);
        }


        [Fact]
        public async void CommentairePost()
        {
            Models.Commentaire newComment = new Models.Commentaire 
            {
                UserId = 1,
                RecetteId = "cake-jambon-olive",
                Title = "Un commentaire de test",
                Comment = "Le contenu du commentaire",
                Mark = 5
            };

            await _apiClient.Post<Models.Commentaire>("commentaires", newComment);
        }
    }
}
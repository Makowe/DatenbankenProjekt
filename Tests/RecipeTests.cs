using NUnit.Framework;
using api.Controllers;
using api.Model;
using api;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Tests {

    [TestFixture]
    public class RecipeTest {

        private RecipeController recipeController;
        private ComponentController componentController;

        [SetUp]
        public void Setup() {
            this.recipeController = new RecipeController();
            this.componentController = new ComponentController();
        }
        
        /// <summary>
        /// Method checks if only valid recipes can be added to the database
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ValidNewRecipe() {
            // delete recipe and component used for the test
            var component1 = await this.componentController.GetComponentByName("Zutat1 Unittest");
            if(component1 != null) { await this.componentController.DeleteComponentById((int)component1.Id); }

            var recipe1 = await this.recipeController.GetRecipeByName("Rezept Unittest");
            if(recipe1 != null) { await this.recipeController.DeleteRecipeById((int)recipe1.Id); }

            // add component to prepare testing
            Component component = new Component {
                Name = "Zutat1 Unittest"
            };
            var response1 = await this.componentController.AddComponent(component);
            component.Id = response1.Value;
            Console.WriteLine(response1.Message);

            Recipe recipe = new Recipe {
                Name = "",
                People = 2,
                Components = new List<Component>(),
                Instructions = new List<Instruction>(),
                Tags = new List<Tag>()
            };
            recipe.Components.Add(component);

            // method should return invalid because it has empty name
            var valid1 = await this.recipeController.CheckNewRecipeValid(recipe);
            Assert.IsTrue(valid1.Value == 0);

            // method should return invalid because component is null
            var valid2 = await this.recipeController.CheckNewRecipeValid(null);
            Assert.IsTrue(valid2.Value == 0);

            // method should return valid 
            recipe.Name = "Rezept Unittest";
            var valid3 = await this.recipeController.CheckNewRecipeValid(recipe);
            Console.WriteLine(valid3.Message);
            Assert.IsTrue(valid3.Value == 1);

            var response2 = await this.recipeController.AddRecipe(recipe);

            // method should return invalid because the name already exists now
            var valid4 = await this.recipeController.CheckNewRecipeValid(recipe);
            Assert.IsTrue(valid4.Value == 0);

            await this.recipeController.DeleteRecipeById(response2.Value);

            // method should return valid because the existing component with the same name is deleted
            var valid5 = await this.recipeController.CheckNewRecipeValid(recipe);
            Assert.IsTrue(valid5.Value == 1);
            
            await this.componentController.DeleteComponentById((int)component.Id);
        }

        /// <summary>
        /// Method tests if existing operations on existing components are correct (e.g. renaming)
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ValidExistingComponent() {
            //delete possible existing components and recipes that are used for testing
            var component = await this.componentController.GetComponentByName("Zutat1 Unittest");
            if(component != null) { await this.componentController.DeleteComponentById((int)component.Id); }

            var recipe = await this.recipeController.GetRecipeByName("Rezept1 Unittest");
            if(recipe != null) { await this.recipeController.DeleteRecipeById((int)recipe.Id); }
            
            recipe = await this.recipeController.GetRecipeByName("Rezept2 Unittest");
            if(recipe != null) { await this.recipeController.DeleteRecipeById((int)recipe.Id); }

            // add components and recipes to prepare testing
            Component component1 = new Component {
                Name = "Zutat1 Unittest"
            };
            var response0 = await this.componentController.AddComponent(component1);
            component1.Id = response0.Value;

            Recipe recipe1 = new Recipe {
                Name = "Rezept1 Unittest",
                People = 2,
                Components = new List<Component>(),
                Instructions = new List<Instruction>(),
                Tags = new List<Tag>()
            };
            recipe1.Components.Add(component1);
            var response1 = await this.recipeController.AddRecipe(recipe1);
            recipe1.Id = response1.Value;

            Recipe recipe2 = new Recipe {
                Name = "Rezept2 Unittest",
                People = 2,
                Components = new List<Component>(),
                Instructions = new List<Instruction>(),
                Tags = new List<Tag>()
            };
            recipe2.Components.Add(component1);
            var response2 = await this.recipeController.AddRecipe(recipe2);
            recipe2.Id = response2.Value;

            // method should return valid because recipe1 is valid
            var valid1 = await this.recipeController.CheckExistingRecipeValid(recipe1);
            Assert.IsTrue(valid1.Value == 1);

            // method should return invalid because the name is empty
            recipe1.Name = "";
            var valid2 = await this.recipeController.CheckExistingRecipeValid(recipe1);
            Assert.IsTrue(valid2.Value == 0);

            // method should return invalid because component is null
            var valid3 = await this.recipeController.CheckExistingRecipeValid(null);
            Assert.IsTrue(valid3.Value == 0);

            //method should return valid because renaming is allowed
            recipe1.Name = "Rezept3 Unittest";
            var valid4 = await this.recipeController.CheckExistingRecipeValid(recipe1);
            Assert.IsTrue(valid4.Value == 1);

            // renaming into "Rezept2 Unittest" is illegal because another recipe has the same name
            recipe1.Name = "Rezept2 Unittest";
            var valid5 = await this.recipeController.CheckExistingRecipeValid(recipe1);
            Assert.IsTrue(valid5.Value == 0);

            await this.recipeController.DeleteRecipeById((int)recipe2.Id);

            // renaming into "Rezept2 Unittest" should be legal now because the component "Rezept2 Unittest" is deleted
            var valid6 = await this.recipeController.CheckExistingRecipeValid(recipe1);
            Assert.IsTrue(valid6.Value == 1);
            
            await this.componentController.DeleteComponentById((int)component1.Id);

            // method should return invalid because the recipe itself is deleted now
            var valid7 = await this.recipeController.CheckExistingRecipeValid(recipe1);
            Assert.IsTrue(valid7.Value == 0);

            await this.componentController.DeleteComponentById((int)component1.Id);
        }
    }
}
using NUnit.Framework;
using api.Controllers;
using api.Model;
using api;
using System.Threading.Tasks;

namespace Tests {

    [TestFixture]
    public class ComponentTest {

        private ComponentController componentController;

        [SetUp]
        public void Setup() {
            this.componentController = new ComponentController();
        }

        /// <summary>
        /// Method checks if only valid components can be added to the database
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ValidNewComponent() {

            Component component = new Component {
                Name = ""
            };
            // method should return invalid because it has empty name
            var valid1 = await this.componentController.CheckNewComponentValid(component);
            Assert.IsTrue(valid1.Value == 0);

            // method should return invalid because component is null
            var valid2 = await this.componentController.CheckNewComponentValid(null);
            Assert.IsTrue(valid2.Value == 0);

            // method should return valid 
            component.Name = "Zutat Unittest";
            var valid3 = await this.componentController.CheckNewComponentValid(component);
            Assert.IsTrue(valid3.Value == 1);

            var response = await this.componentController.AddComponent(component);

            // method should return invalid because the name already exists now
            var valid4 = await this.componentController.CheckNewComponentValid(component);
            Assert.IsTrue(valid4.Value == 0);

            await this.componentController.DeleteComponentById(response.Value);

            // method should return valid because the existing component with the same name is deleted
            var valid5 = await this.componentController.CheckNewComponentValid(component);
            Assert.IsTrue(valid5.Value == 1);
        }

        /// <summary>
        /// Method tests if existing operations on existing components are correct (e.g. renaming)
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ValidExistingComponent() {
            //delete possible existing components that are used for testing
            var component = await this.componentController.GetComponentByName("Zutat1 Unittest");
            if(component != null) { await this.componentController.DeleteComponentById((int)component.Id); }
            
            component = await this.componentController.GetComponentByName("Zutat2 Unittest");
            if(component != null) { await this.componentController.DeleteComponentById((int)component.Id); }

            // add two seperate components to prepare testing
            Component component1 = new Component {
                Name = "Zutat1 Unittest"
            };
            var response1 = await this.componentController.AddComponent(component1);
            component1.Id = response1.Value;
            Component component2 = new Component {
                Name = "Zutat2 Unittest"
            };
            var response2 = await this.componentController.AddComponent(component2);
            component2.Id = response2.Value;

            // method should return valid because component1 is valid
            var valid1 = await this.componentController.CheckExistingComponentValid(component1);
            Assert.IsTrue(valid1.Value == 1);

            // method should return invalid because the name is empty
            component1.Name = "";
            var valid2 = await this.componentController.CheckExistingComponentValid(component1);
            Assert.IsTrue(valid2.Value == 0);

            // method should return invalid because component is null
            var valid3 = await this.componentController.CheckExistingComponentValid(null);
            Assert.IsTrue(valid3.Value == 0);

            //method should return valid because renaming is allowed
            component1.Name = "Zutat3 Unittest";
            var valid4 = await this.componentController.CheckExistingComponentValid(component1);
            Assert.IsTrue(valid4.Value == 1);

            // renaming into "Zutat2 Unittest" is illegal because another component has the same name
            component1.Name = "Zutat2 Unittest";
            var valid5 = await this.componentController.CheckExistingComponentValid(component1);
            Assert.IsTrue(valid5.Value == 0);

            await this.componentController.DeleteComponentById((int)component2.Id);

            // renaming into "Zutat2 Unittest" should be legal now because the component "Zutat2 Unittest" is deleted
            var valid6 = await this.componentController.CheckExistingComponentValid(component1);
            Assert.IsTrue(valid6.Value == 1);

            await this.componentController.DeleteComponentById((int)component1.Id);

            // method should return invalid because the component itself is deleted now
            var valid7 = await this.componentController.CheckExistingComponentValid(component1);
            Assert.IsTrue(valid7.Value == 0);
        }
    }
}
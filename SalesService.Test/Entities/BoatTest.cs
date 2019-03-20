using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalesService.Entities;
using SalesService.Model.DomainModel;

namespace SalesService.Test.Entities
{
    [TestClass]
    public class BoatTest
    {

        [TestMethod, TestCategory("DBRequired")]
        public void CreateBoatTest()
        {
            var apiBoat = DefaultBoat();
            var boatEntity = BoatData.CreateBoat(apiBoat);
            Assert.IsTrue(boatEntity.Identifier != 0, $"Failed to create boat {apiBoat.BuilderName} {apiBoat.ModelYear} {apiBoat.Model} ");
        }

        [TestMethod, TestCategory("DBRequired")]
        public void CreateAndUpdateBoatTest()
        {
            var apiBoat = DefaultBoat();
            var boatEntity = BoatData.CreateBoat(apiBoat);
            boatEntity.Model = "ChangedModel";
            var updatedBoat = UpdateBoat(boatEntity);
            Assert.IsTrue(boatEntity.Identifier == updatedBoat.Identifier, $"UpdateBoat failed. Id's {boatEntity.Identifier}!={updatedBoat.Identifier }");
            Assert.IsTrue("ChangedModel" == updatedBoat.Model, $"UpdateBoat failed. OriginalModel!={updatedBoat.Model}");
        }

        [TestMethod, TestCategory("DBRequired")]
        public void GetBoatTest()
        {
            var apiBoat = DefaultBoat();
            var boatEntity = BoatData.CreateBoat(apiBoat);
            var boat = BoatData.GetBoatbyId(boatEntity.Identifier);
            Assert.IsNotNull(boat.BuilderName, $"No boat is retrieved");

        }

        [TestMethod, TestCategory("DBRequired")]
        public void GetAllBoatTest()
        {
            var boat1 = DefaultBoat();
            var boatEntity = BoatData.CreateBoat(boat1);
            Assert.IsNotNull(boatEntity, $"CreateBoat failed.");
            var boat2 = DefaultAdditionalBoat();
            var boatEntityAdditional = BoatData.CreateBoat(boat2);
            Assert.IsNotNull(boatEntityAdditional, $"CreateBoat failed.");
            var boats = BoatData.GetBoats();
            Assert.IsTrue(boats.BoatCollection.Count>0, $"All boats are not retrieved");
        }

        [TestMethod, TestCategory("DBRequired")]
        public void CreateAndDeleteBoatTest()
        {
            Boat apiBoat = null;
            apiBoat = DefaultBoat();
            var boatEntity = BoatData.CreateBoat(apiBoat);
            BoatData.DeleteBoatbyId(boatEntity.Identifier);
            var boat = BoatData.GetBoatbyId(boatEntity.Identifier);
            Assert.IsNull(boat, $"The boat is not deleted");
        }

        private Boat UpdateBoat(Boat apiBoat)
        {
            var boatEntity = BoatData.UpdateBoat(apiBoat);
            Assert.IsNotNull(boatEntity.Identifier, "UpdateBoat Failed. Identifier is null");
            return boatEntity;
        }
        private static Boat DefaultBoat()
        {
            Boat boat = new Boat()
            {
                ModelYear = "2016",
                BuilderName = "Kawasaki",
                Model = "Jet SKI STX",
                WatercraftType = "Racing",
                EngineType = "4 CL",
                IsCustomized = false
            };
            return boat;
        }
        

        private static Boat DefaultAdditionalBoat()
        {
            Boat additionalboat = new Boat()
            {
                ModelYear = "2018",
                BuilderName = "Seadoo",
                Model = "RXT-X 300",
                WatercraftType = "Racing",
                EngineType = "3 CL",
                IsCustomized = true
            };
            return additionalboat;
        }
    }
}

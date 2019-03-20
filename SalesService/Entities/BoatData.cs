using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesService.Helper;
using SalesService.Model;
using SalesService.Model.DomainModel;

namespace SalesService.Entities
{
    public class BoatData
    {
        //Get all the boats
        public static Boats GetBoats()
        {
            return ProductHelper.GetBoats();
        }

        //Get a boat based on the id provided
        public static Boat GetBoatbyId(int boatId)
        {
            return ProductHelper.GetBoatsbyId(boatId);
        }

        //Create a boat
        public static Boat CreateBoat(Boat boat)
        {
            return ProductHelper.CreateBoat(boat);

        }

        //Update a boat
        public static Boat UpdateBoat(Boat boat)
        {
            return ProductHelper.UpdateBoat(boat);
        }

        //Delete a boat based on id provided
        public static bool  DeleteBoatbyId(int boatId)
        {
            return ProductHelper.DeleteBoatbyId(boatId);
        }


    }
}

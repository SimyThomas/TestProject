using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesService.Model;
using SalesService.Model.DomainModel;

namespace SalesService.Helper
{
    public class ProductHelper
    {
        public static Boats GetBoats()
        {
            var boats=new Boats();
            try
            { 
                boats = SqlHelper.GetAllBoats("Sales", "GetBoats");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return boats;
        }
        public static Boat GetBoatsbyId(int boatId)
        {
            var sqlParam = boatId;
            var boat = new Boat();
            try
            {
                boat = SqlHelper.GetBoatbyId("Sales", "GetBoatById", sqlParam);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            if (boat != null)
                return boat;
            else
                return null;
        }


        public static Boat CreateBoat(Boat boat)
        {

            try
            {
                boat = SqlHelper.CreateBoat("Sales", "CreateBoats", boat);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return boat.Identifier != null ? boat : null;
        }

        public static Boat UpdateBoat(Boat boatToEdit)
        {
            var sqlParam = boatToEdit.Identifier;
            try
            {
                var getboat = SqlHelper.GetBoatbyId("Sales", "GetBoatById", sqlParam);
                if (getboat.Identifier != null)
                {
                    boatToEdit = SqlHelper.UpdateBoat("Sales", "EditBoat", boatToEdit);
                }
                else return null;
            } 
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            if (boatToEdit != null)
                return boatToEdit;
            else
                return null;
        }
        public static bool DeleteBoatbyId(int boatId)
        {
            var sqlParam = boatId;
            bool isDeleted = false;
            try
            {
                var boat = SqlHelper.GetBoatbyId("Sales", "GetBoatById", sqlParam);
                if (boat!= null)
                {
                    SqlHelper.DeleteBoatbyId("Sales", "DeleteBoatById", sqlParam);
                    isDeleted = true;
                }
                
                return isDeleted;
            }
            catch (Exception ex)
            {
                return isDeleted;
            }

        }

    }
}

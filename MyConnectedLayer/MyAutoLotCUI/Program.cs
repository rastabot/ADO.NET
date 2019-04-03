using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// using our dll
using MyDAL;
// using ADO.NET API
using System.Data;
// using App.config for reading connection string
using System.Configuration;

namespace MyAutoLotCUI
{
    class Program
    {
        static void Main(string[] args)
        {
            // we will use ConfigurationManager to read the 
            // app.config property name 
            string str =
                ConfigurationManager.
                ConnectionStrings["AutoLotCon"].ConnectionString;

            // Create an instance of InventoryDAL
            InventoryDAL invDal = new InventoryDAL();
            invDal.OpenConnection(str);

            // CUI - PL
            bool userDone = false;
            string userCommand = "";
            try
            {
                ShowUserChoices();
                do
                {
                    Console.WriteLine("Enter your selection:");
                    userCommand = Console.ReadLine();
                    switch (userCommand)
                    {
                        case "S":
                            ListAllInventory(invDal);
                            break;
                        case "I":
                            InsertNewCar(invDal);
                            break;
                        case "U":
                            UpdateCar(invDal);
                            break;
                        case "D":
                            DeleteCar(invDal);
                            break;
                        case "SP":
                            SearchCarByName(invDal);
                            break;
                        case "Q":
                            userDone = true;
                            break;
                        default:
                            Console.WriteLine("Please Insert correct value!");
                            break;
                    }
                } while (!userDone);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                invDal.CloseConnection();
            }
        }

        #region BLL - Business Logic

        #region Select Query
        public static void ListAllInventory(InventoryDAL invDal)
        {
            DataTable dt = invDal.GetAllInventory();
            DisplayDataTable(dt);
        }
        // display data table in console
        public static void DisplayDataTable(DataTable dt)
        {
            for (int currRow = 0; currRow < dt.Rows.Count; currRow++)
            {
                //Console.WriteLine();
                for (int currCol = 0; currCol < dt.Columns.Count; currCol++)
                {
                    Console.Write("{0}\t", dt.Rows[currRow][dt.Columns[currCol].ToString()]);
                }
                Console.WriteLine();
            }
        }

        #endregion

        #region Insert Query
        public static void InsertNewCar(InventoryDAL invDal)
        {
            string newMake, newColor, newCarName;

            Console.WriteLine("Insert new Make");
            newMake = Console.ReadLine();
            Console.WriteLine("Insert new Color");
            newColor = Console.ReadLine();
            Console.WriteLine("Insert new CarName");
            newCarName = Console.ReadLine();

            invDal.InsertAuto(newMake, newColor, newCarName);
        }


        #endregion

        #region Update Query

        public static void UpdateCar(InventoryDAL invDal)
        {
            int carId;
            string newCarName;
            Console.WriteLine("Enter Car id");
            carId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter new car name");
            newCarName = Console.ReadLine();

            invDal.UpdateCarName(carId, newCarName);

        }

        #endregion

        #region Delete Query

        public static void DeleteCar(InventoryDAL invDal)
        {
            Console.WriteLine("Enter Car id to delete");
            int id = int.Parse(Console.ReadLine());

            invDal.DeleteCar(id);
        }
        #endregion

        #region Stored procedure

        public static void SearchCarByName(InventoryDAL invDal)
        {
            Console.WriteLine("Enter Car id ");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("CarID: {0}, CarName:{1} ", 
                id, invDal.SearchForCarName(id));

            Console.ReadLine();
        }

        #endregion

        #endregion

        #region PL - Presenation Layer

        public static void ShowUserChoices()
        {
            Console.WriteLine("S: Show all inventory");
            Console.WriteLine("I: Insert new car");
            Console.WriteLine("U: Update car name");
            Console.WriteLine("D: Delete Car");
            Console.WriteLine("SP: Look for a car name based on car id");
            Console.WriteLine("Q: Quit the application");
        }

        #endregion

    }
}

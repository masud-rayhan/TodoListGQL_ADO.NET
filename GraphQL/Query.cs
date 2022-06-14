using System;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;
using HotChocolate.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TodoListGQL.Models;

namespace TodoListGQL.GraphQL
{
    public class Query
    {
        
        string connectionString = ""; 
        public Query(IConfiguration configuration)
        {
            
            connectionString = configuration.GetConnectionString("DefaultConnection");


        }


        //[UseDbContext(typeof(ApiDbContext))]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public List<ItemList> GetList() => GetListFromDB().ToList();




        /*[UseDbContext(typeof(ApiDbContext))]*/
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public List<ItemData> GetDatas() => GetDataFromDB().ToList();
        
       




        public List<ItemList> GetListFromDB()
        {
            List<ItemList> lists =new List<ItemList>() ;
            try{
                using(var connection = new SqlConnection(connectionString))
                {
                    

                    SqlCommand command = new SqlCommand("select * from Lists  ", connection);
                    connection.Open();
                    var dataFromDB= command.ExecuteReader();


                    while (dataFromDB.Read()){
                        ItemList itemList= new ItemList();
                        itemList.Id= Convert.ToInt32(dataFromDB["Id"]);
                        itemList.Name= dataFromDB["Name"].ToString();


                        var connection1 = new SqlConnection(connectionString);
                        string query = "select * from Items Where ListId = " + itemList.Id;
                        connection1.Open();
                        SqlCommand command1 = new SqlCommand(query, connection1);
                        
                        var dataFromDB1 = command1.ExecuteReader();

                        while (dataFromDB1.Read())
                        {
                            ItemData itemData= new ItemData();
                            itemData.Id= Convert.ToInt32(dataFromDB1["Id"]);
                            itemData.Title = dataFromDB1["Title"].ToString();
                            itemData.Description= dataFromDB1["Description"].ToString();
                            
                            itemList.ItemDatas.Add(itemData);
                        }

                        lists.Add(itemList);

                        connection1.Close();
                    }

                    connection.Close();
                    


                }
                
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
            }

             return lists.ToList();

        }


        public List<ItemData> GetDataFromDB()
        {
            List<ItemData> lists = new List<ItemData>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {


                    SqlCommand command = new SqlCommand("select * from Items ", connection);
                    connection.Open();
                    var dataFromDB = command.ExecuteReader();


                    while (dataFromDB.Read())
                    {
                        ItemData itemData = new ItemData();
                        
                        itemData.Id= Convert.ToInt32(dataFromDB["Id"]);
                        itemData.Title= dataFromDB["Title"].ToString();
                        itemData.Description= dataFromDB["Description"].ToString();
                        itemData.ListId = Convert.ToInt32(dataFromDB["ListId"]);

                        lists.Add(itemData);

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lists.ToList();

        }



    }
}
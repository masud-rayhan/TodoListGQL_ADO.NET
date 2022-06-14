using System;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TodoListGQL.GraphQL.DataItem;
using TodoListGQL.GraphQL.Lists;
using TodoListGQL.Models;


namespace TodoListGQL.GraphQL
{
    public class Mutation
    {
        string connectionString = "";

        public Mutation(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<AddListPayload> AddListAsync(AddListInput input)
        {
            var list = new ItemList
            {
                Name = input.name
            };

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Lists(Name) Values('" + list.Name + "')";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch(Exception ex)
            {

            }

            return new AddListPayload(list);
        }


       /* [UseDbContext(typeof(ApiDbContext))]
        public async Task<AddListPayload> AddListAsync(AddListInput input, [ScopedService] ApiDbContext context)
        {
            var list = new ItemList
            {
                Name = input.name
            };

            context.Lists.Add(list);
            await context.SaveChangesAsync();

            return new AddListPayload(list);
        }*/







        /*[UseDbContext(typeof(ApiDbContext))]
        public async Task<AddItemPayload> AddItemAsync(AddItemInput input, [ScopedService] ApiDbContext context)
        {
            var item = new ItemData
            {
                IsDone = input.isDone,
                Description = input.description,
                ListId = input.listId,
                Title = input.title
            };

            context.Items.Add(item);
            await context.SaveChangesAsync();

            return new AddItemPayload(item);
        }*/
    }
    
}


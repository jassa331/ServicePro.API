using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServicePro.Core.DTOs;
using ServicePro.Core.Entities;
using ServicePro.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.Infrastructure.Repositories
{
    public class Productservice
    {
        private readonly AppDbContext context;

        public Productservice(AppDbContext context)
        {
            this.context = context;
        }
        public async Task updateProduct(updateProductDTO productUpdate)
        {
            using IDbConnection db =
                new SqlConnection(context.Database.GetDbConnection().ConnectionString);

            using SqlCommand cmd = new SqlCommand("product_update_sp", (SqlConnection)db);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", productUpdate.Id); 
            cmd.Parameters.AddWithValue("@Name", productUpdate.Name);
            cmd.Parameters.AddWithValue("@Description", productUpdate.Description);
            cmd.Parameters.AddWithValue("@Price", productUpdate.Price);
            cmd.Parameters.AddWithValue("@Category", productUpdate.Category);
            cmd.Parameters.AddWithValue("@Images", productUpdate.Images);

            db.Open();
            await cmd.ExecuteNonQueryAsync();
        }

    }
}

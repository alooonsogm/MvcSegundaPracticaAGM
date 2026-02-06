using Microsoft.Data.SqlClient;
using MvcSegundaPracticaAGM.Models;
using System.Data;

namespace MvcSegundaPracticaAGM.Repositories
{
    public class RepositoryComics
    {
        private DataTable tablaComics;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryComics()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=COMICS;Persist Security Info=True;User ID=sa;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            string sql = "SELECT * FROM COMICS";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            this.tablaComics = new DataTable();
            ad.Fill(this.tablaComics);
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() select datos;
            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
            {
                Comic comic = new Comic();
                comic.IdComic = row.Field<int>("IDCOMIC");
                comic.Nombre = row.Field<string>("NOMBRE");
                comic.Imagen = row.Field<string>("IMAGEN");
                comic.Descripcion = row.Field<string>("DESCRIPCION");
                comics.Add(comic);
            }
            return comics;
        }

        public Comic FindComic(int idComic)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() where datos.Field<int>("IDCOMIC") == idComic select datos;

            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                var row = consulta.First();
                Comic comic = new Comic();
                comic.IdComic = row.Field<int>("IDCOMIC");
                comic.Nombre = row.Field<string>("NOMBRE");
                comic.Imagen = row.Field<string>("IMAGEN");
                comic.Descripcion = row.Field<string>("DESCRIPCION");
                return comic;
            }
        }

        public async Task InsertComicAsync(string nombre, string imagen, string descripcion)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() select datos;
            int maximoIdComic = consulta.Max(z => z.Field<int>("IDCOMIC"));
            maximoIdComic = maximoIdComic + 1;

            string sql = "insert into COMICS values (@id, @nombre, @imagen, @descripcion)";

            this.com.Parameters.AddWithValue("@id", maximoIdComic);
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@imagen", imagen);
            this.com.Parameters.AddWithValue("@descripcion", descripcion);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
    }
}

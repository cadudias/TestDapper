using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using TestDapper.Models;

namespace TestDapper.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration configuration)
        {
            _config = configuration;
        }

        // buscando usando o Dapper.Contrib pra facilitar as operações de CRUD
        // GetAll é um método de estensão do Dapper.Contrib
        public IActionResult Index()
        {
            using (SqlConnection conexao = new SqlConnection(
                _config.GetConnectionString("ExemplosDapper")))
            {
                var states = conexao.GetAll<Estado>();

                return View(states);
            }
        }

        // busca estados com a query sem usar o Dapper.Contrib
        public IActionResult GetStates()
        {
            using (SqlConnection conexao = new SqlConnection(
                _config.GetConnectionString("ExemplosDapper")))
            {
                var sql = "SELECT E.SiglaEstado, E.NomeEstado " +
                    "FROM dbo.Estados E " +
                    "INNER JOIN dbo.Regioes R ON R.IdRegiao = E.IdRegiao " +
                    "ORDER BY E.NomeEstado";

                var states = conexao.Query<Estado>(sql);

                return View(states);
            }
        }

        // exemplo buscando de uma stored procedure
        public Estado GetDetalhesEstadoProcedure(string siglaEstado)
        {
            using (SqlConnection conexao = new SqlConnection(
                _config.GetConnectionString("ExemplosDapper")))
            {
                return conexao.QueryFirstOrDefault<Estado>(
                    "dbo.PRC_SEL_DETALHES_ESTADO",
                    new
                    {
                        CodEstado = siglaEstado
                    },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
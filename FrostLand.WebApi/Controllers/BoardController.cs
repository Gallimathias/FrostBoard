using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FrostLand.Model;
using FrostLand.SqlLite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrostLand.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BoardController : Controller
    {
        [HttpGet]
        public Board Get([FromHeader]int id) => DatabaseManager.MainDatabase.Boards.Find(id);

        [HttpGet("[action]")]
        public ActionResult GetBoard([FromHeader]int id)
        {
            string fileContent;

            var board = DatabaseManager.MainDatabase.Boards.Find(id);

            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FrostLand.WebApi.Templates.board.template")))
            {
                fileContent = reader.ReadToEnd();

                foreach (var prop in board.GetType().GetProperties())
                    fileContent = fileContent.Replace("@" + prop.Name, prop.GetValue(board)?.ToString());
            }

            return Content(fileContent, "text/html");
        }

        [HttpPost("[action]")]
        public void Create([FromBody]Board board)
        {
            DatabaseManager.MainDatabase.Boards.Add(board);
            DatabaseManager.MainDatabase.SaveChangesAsync();
        }

        [HttpGet("[action]")]
        public List<Board> GetBoards([FromHeader]int parent = -1)
        {
            bool checkBoard(Board board)
            {
                if (parent < 0)
                    return true;
                else if (parent == 0)
                    return board.Parent == null;
                else if (board.Parent == null)
                    return false;
                else
                    return board.Parent.Id == parent;
            }

            return DatabaseManager.MainDatabase.Boards.Where(checkBoard).ToList();
        }
    }
}
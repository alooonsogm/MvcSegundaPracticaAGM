using Microsoft.AspNetCore.Mvc;
using MvcSegundaPracticaAGM.Models;
using MvcSegundaPracticaAGM.Repositories;

namespace MvcSegundaPracticaAGM.Controllers
{
    public class ComicsController : Controller
    {
        private RepositoryComics repo;

        public ComicsController()
        {
            this.repo = new RepositoryComics();
        }

        public IActionResult Index()
        {
            List<Comic> comics = this.repo.GetComics();
            return View(comics);
        }
        public IActionResult Details(int id)
        {
            Comic comic = this.repo.FindComic(id);
            return View(comic);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Nombre, string Imagen, string Descripcion)
        {
            await this.repo.InsertComicAsync(Nombre, Imagen, Descripcion);
            return RedirectToAction("Index");
        }
    }
}
